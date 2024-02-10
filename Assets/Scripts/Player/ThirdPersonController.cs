  using UnityEngine;
  using UnityEngine.SceneManagement;
#if ENABLE_INPUT_SYSTEM 
using UnityEngine.InputSystem;
using System.Collections;
#endif

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM 
    [RequireComponent(typeof(PlayerInput))]
#endif
    public class ThirdPersonController : MonoBehaviour
    {
        [Header("Player")]
        public PlayerData playerData;
        [Tooltip("Move speed of the character in m/s")]
        public float MoveSpeed = 2.0f;

        [Tooltip("Sprint speed of the character in m/s")]
        public float SprintSpeed = 5.335f;

        [Tooltip("How fast the character turns to face movement direction")]
        [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;

        [Tooltip("Acceleration and deceleration")]
        public float SpeedChangeRate = 10.0f;
        public float Sensitivity = 1f;

        public AudioClip LandingAudioClip;
        public AudioClip[] FootstepAudioClips;
        [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

        [Space(10)]
        [Tooltip("The height the player can jump")]
        public float JumpHeight = 1.2f;

        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        public float Gravity = -15.0f;

        [Space(10)]
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        public float JumpTimeout = 0.50f;

        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        public float FallTimeout = 0.15f;

        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        public bool Grounded = true;

        [Tooltip("Useful for rough ground")]
        public float GroundedOffset = -0.14f;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float GroundedRadius = 0.28f;

        [Tooltip("What layers the character uses as ground")]
        public LayerMask GroundLayers;

        [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject DefaultCameraTarget;
        public GameObject RecoilCameraTarget;
        private GameObject CurrentCameraTarget; 


        [Tooltip("How far in degrees can you move the camera up")]
        public float TopClamp = 70.0f;

        [Tooltip("How far in degrees can you move the camera down")]
        public float BottomClamp = -30.0f;

        [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
        public float CameraAngleOverride = 0.0f;

        [Tooltip("For locking the camera position on all axis")]
        public bool LockCameraPosition = false;

        private Vector3 lastForwardDirection = Vector3.forward;
        private float lastTargetRotation;
        private float noInputTransitionSpeed = 0.2f; 
        public float rotationSmoothTime = 0.1f; // Tweak the value
        private Vector3 rotationVelocity = Vector3.zero;

        //DeathAudio
        public AudioClip deathAudio;
        AudioSource audioSource;
        public GameObject deathScreen;
        public bool deathbool = false;



        // cinemachine
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        // player
        public float _speed;
        private float _animationBlend;
        private float _targetRotation ;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;
       // private float rotationVelocity;

        // Define a rotation speed for the transition
        public float RotationSpeed = 5.0f;

        // Store the previous input direction
        private Vector3 previousInputDirection = Vector3.forward;

        // timeout deltatime
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        // animation IDs
        private int _animIDSpeed;
        private int _animIDGrounded;
        private int _animIDJump;
        private int _animIDFreeFall;
        private int _animIDMotionSpeed;
        private int _animIDCrouch;

#if ENABLE_INPUT_SYSTEM 
        private PlayerInput _playerInput;
#endif
        private Animator _animator;
        private CharacterController _controller;
        private StarterAssetsInputs _input;
        private SaveSystemTest saveSystemTest;  //Save System Test Inputs
        private PlayerHealthMetric healthMetrics;
        public Progress progressScript; 
        public SceneTransitionController sceneTransition;
        private GameObject _mainCamera;
        private bool _rotateOnMove = true;

        [Header ("Dev Controls")]
        public Transform teleportLocation1;
        public Transform teleportLocation2;
        public Transform teleportLocation3;

        public PauseMenuScript pauseMenuScript;

        private const float _threshold = 0.01f;

        private bool _hasAnimator;

        private bool IsCurrentDeviceMouse
        {
            get
            {
#if ENABLE_INPUT_SYSTEM
                return _playerInput.currentControlScheme == "KeyboardMouse";
#else
				return false;
#endif
            }
        }


        private void Awake()
        {
            // get a reference to our main camera
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
                 CurrentCameraTarget = DefaultCameraTarget;
            }
            audioSource = GetComponent<AudioSource>();
            deathScreen.SetActive(false);
            playerData.currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        }

        private void Start()
        {
            _cinemachineTargetYaw = CurrentCameraTarget.transform.rotation.eulerAngles.y;

            _hasAnimator = TryGetComponent(out _animator);
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<StarterAssetsInputs>();
            healthMetrics = GetComponent<PlayerHealthMetric>();
            saveSystemTest = GetComponent<SaveSystemTest>();    //Save System Test Inputs
#if ENABLE_INPUT_SYSTEM 
            _playerInput = GetComponent<PlayerInput>();
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif

            AssignAnimationIDs();

            // reset our timeouts on start
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;
        }

        private void Update()
        {
            _hasAnimator = TryGetComponent(out _animator);
            playerData.playerPosition = transform.position;
            Pause();
            JumpAndGravity();
            GroundedCheck();
            Move();
            SaveTestInputs();
            Crouch();
            Teleport();
        }

        private void LateUpdate()
        {
            CameraRotation();
        }

        private void AssignAnimationIDs()
        {
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDGrounded = Animator.StringToHash("Grounded");
            _animIDJump = Animator.StringToHash("Jump");
            _animIDFreeFall = Animator.StringToHash("FreeFall");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
            _animIDCrouch = Animator.StringToHash("CrouchAnim");
        }

        private void GroundedCheck()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);

            // Check if the ground layer is not the EnemyColider layer or Enemy layer
            if (Grounded)
            {
                Collider[] colliders = Physics.OverlapSphere(spherePosition, GroundedRadius, GroundLayers);
                foreach (Collider collider in colliders)
                {
                    int enemyLayer = LayerMask.NameToLayer("Enemy");
                    int enemyColliderLayer = LayerMask.NameToLayer("EnemyColider");

                    if (collider.gameObject.layer == enemyColliderLayer || collider.gameObject.layer == enemyLayer)
                    {
                        Grounded = false;
                        break; // Exit the loop if an enemy collider is found
                    }
                }
            }

            // update animator if using character
            if (_hasAnimator)
            {
                _animator.SetBool(_animIDGrounded, Grounded);
            }
        }
        
        private void CameraRotation()
        {
            LogSystem logSystem = FindObjectOfType<LogSystem>();
            // if there is an input and camera position is not fixed
            if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition && pauseMenuScript.paused == false && logSystem.log == false)
            {
                //Don't multiply mouse input by Time.deltaTime;
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier * Sensitivity;
                _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier * Sensitivity;
            }

            // clamp our rotations so our values are limited 360 degrees
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // Cinemachine will follow this target
            CurrentCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
                _cinemachineTargetYaw, 0.0f);
        }
        private void Move()
        {
            if (pauseMenuScript.paused == false && deathbool == false)
            {
                // Set target speed based on move speed, sprint speed, and if sprint is pressed
                float targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;

                // A simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

                // Note: Vector2's == operator uses approximation so is not floating point error-prone and is cheaper than magnitude
                // If there is no input, set the target speed to 0
                if (_input.move == Vector2.zero) targetSpeed = 0.0f;

                // A reference to the player's current horizontal velocity
                float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

                float speedOffset = 0.1f;
                float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

                // Accelerate or decelerate to target speed
                if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                    currentHorizontalSpeed > targetSpeed + speedOffset)
                {
                    // Creates a curved result rather than a linear one, giving a more organic speed change
                    // Note T in Lerp is clamped, so we don't need to clamp our speed
                    _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);

                    // Round speed to 3 decimal places
                    _speed = Mathf.Round(_speed * 1000f) / 1000f;
                }
                else
                {
                    _speed = targetSpeed;
                }

                _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
                if (_animationBlend < 0.01f) _animationBlend = 0f;

                // Get the camera's forward direction without the vertical component
                Vector3 cameraForward = Vector3.Scale(_mainCamera.transform.forward, new Vector3(1, 0, 1));

                // Transform the input direction based on the camera's rotation
                Vector3 inputDirection = Quaternion.LookRotation(cameraForward) * new Vector3(_input.move.x, 0, _input.move.y);

                if (inputDirection != Vector3.zero)
                {
                    // Check if aiming or shooting, and adjust RotateOnMove accordingly
                    if (_input.aim || _input.shoot)
                    {
                        _rotateOnMove = false;
                    }
                    else
                    {
                        _rotateOnMove = true;

                        // Use Quaternion.LookRotation to calculate the target rotation
                        Quaternion targetRotationQuaternion = Quaternion.LookRotation(inputDirection.normalized, Vector3.up);

                        if (_rotateOnMove)
                        {
                            // Use SmoothDamp for rotation
                            Vector3 currentEulerAngles = transform.eulerAngles;
                            Vector3 targetEulerAngles = targetRotationQuaternion.eulerAngles;

                            // Use Vector3.SmoothDamp to smooth the rotation
                            Vector3 smoothDampedEulerAngles = Vector3.SmoothDamp(currentEulerAngles, targetEulerAngles, ref rotationVelocity, rotationSmoothTime);

                            // Apply the smoothed rotation
                            transform.rotation = Quaternion.Euler(smoothDampedEulerAngles);
                        }

                        // Gradually adjust the forward direction based on the current and previous input direction
                        Vector3 smoothedForward = Vector3.Slerp(previousInputDirection, inputDirection.normalized, RotationSpeed * Time.deltaTime);
                        previousInputDirection = smoothedForward;

                        // Set the player's forward direction to the smoothed direction
                        transform.forward = smoothedForward;

                        // Update last target rotation while there is input
                        lastTargetRotation = targetRotationQuaternion.eulerAngles.y;
                    }
                }
                else if (_rotateOnMove)
                {
                    // If there's no input, set the forward direction to the last target rotation without interpolation
                    transform.rotation = Quaternion.Euler(0.0f, lastTargetRotation, 0.0f);
                }

                // Move the player
                _controller.Move(inputDirection.normalized * (_speed * Time.deltaTime) +
                                new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

                // Update animator if using character
                if (_hasAnimator)
                {
                    _animator.SetFloat(_animIDSpeed, _animationBlend);
                    _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
                }
            }
        }
            private Vector3 GetAimDirection()
        {
            // Use the mouse position to determine the aim direction
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane groundPlane = new Plane(Vector3.up, transform.position);
            float distance;

            if (groundPlane.Raycast(ray, out distance))
            {
                Vector3 mouseWorldPosition = ray.GetPoint(distance);
                Vector3 aimDirection = (mouseWorldPosition - transform.position).normalized;
                return aimDirection;
            }

            return Vector3.zero;
        }


        private void JumpAndGravity()
        {
            if (Grounded)
            {
                // reset the fall timeout timer
                _fallTimeoutDelta = FallTimeout;

                // update animator if using character
                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDJump, false);
                    _animator.SetBool(_animIDFreeFall, false);
                }

                // stop our velocity dropping infinitely when grounded
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                // Jump
                if (!_controller.isGrounded)
                {
                    _input.jump = false;
                }
                if (_input.jump && _jumpTimeoutDelta <= 0.0f)
                {
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                    // update animator if using character
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDJump, true);
                    }
                }

                // jump timeout
                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                // reset the jump timeout timer
                _jumpTimeoutDelta = JumpTimeout;

                // fall timeout
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    // update animator if using character
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDFreeFall, true);
                    }
                }

                // if we are not grounded, do not jump
                _input.jump = false;
            }

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (Grounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
                GroundedRadius);
        }

        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                if (FootstepAudioClips.Length > 0)
                {
                    var index = Random.Range(0, FootstepAudioClips.Length);
                    AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_controller.center), FootstepAudioVolume);
                }
            }
        }

        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center), FootstepAudioVolume);
            }
        }

        public void SetSensitivity(float newSensitivity)
        {
            Sensitivity = newSensitivity;
        }

        public void SetRotateOnMove(bool newRotateOnMove)
        {
            _rotateOnMove = newRotateOnMove;
        }

        public void SwitchCameraTarget()
        {
            // Swap the transform positions of DefaultCameraTarget and RecoilCameraTarget
            Vector3 tempPosition = DefaultCameraTarget.transform.position;
            DefaultCameraTarget.transform.position = RecoilCameraTarget.transform.position;
            RecoilCameraTarget.transform.position = tempPosition;



            // Automatically swap positions again after 0.1 seconds
            StartCoroutine(AutoSwapCameraTargetPositions(0.1f));
        }

        private IEnumerator AutoSwapCameraTargetPositions(float delay)
        {
            yield return new WaitForSeconds(delay);

            // Swap the transform positions of DefaultCameraTarget and RecoilCameraTarget
            Vector3 tempPosition = DefaultCameraTarget.transform.position;
            DefaultCameraTarget.transform.position = RecoilCameraTarget.transform.position;
            RecoilCameraTarget.transform.position = tempPosition;

        }

        private void SaveTestInputs() //Save System Test Inputs
        {
            if (_input.save)
            {
                _input.save = false;
                saveSystemTest.SaveGame();
                Debug.Log("Save Input Pressed!");
            }

            if (_input.load)
            {
                _input.load = false;
                saveSystemTest.LoadGame();               
                Debug.Log("Load Input Pressed!");

                if (progressScript != null)
                {
                    progressScript.ResetProgress();
                }
            }

            
            if (playerData.currentHealth <= 0)
            {
                
                if (deathbool != true)
                {
                    audioSource.PlayOneShot(deathAudio);
                    deathbool = true;
                }
                else
                {
                    _input.pause = false;
                }
            Time.timeScale = 0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            deathScreen.SetActive(true);
            LockCameraPosition = true;
            }

            if (_input.value)
            {
                _input.value = false;
                saveSystemTest.TestValue();
            }
        }


        public void Crouch()
        {
            if (_input.crouch == true)
            {
                _animator.SetBool(_animIDCrouch, true);
            }
            else if(_input.crouch == false)
            {
                _animator.SetBool(_animIDCrouch, false);
            }
        } 

        public void Pause()
        {
            if(_input.pause && deathbool == false)
            {
                _input.pause = false;
                pauseMenuScript.SetPause();
                Debug.Log("Pause input!");
            }
        }
           private void Teleport()
        {
            if (_input.teleport1)
            {
                TeleportToLocation(teleportLocation1);
            }
            else if (_input.teleport2)
            {
                TeleportToLocation(teleportLocation2);
            }
            else if (_input.teleport3)
            {
                TeleportToLocation(teleportLocation3);
            }
        }

        private void TeleportToLocation(Transform targetTransform)
        {
            if (targetTransform != null)
            {
                // Teleport the player to the specified location (keeping the current rotation)
                transform.position = targetTransform.position;

                // Reset the CharacterController to ensure it updates its internal state
                ResetCharacterController();
            }
        }

        private void ResetCharacterController()
        {
            // Ensure the CharacterController is not null
            if (_controller != null)
            {
                // Reset various properties of the CharacterController
                _controller.enabled = false;
                _controller.enabled = true;
            }
        }
        
    }
}