using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.UI;

public class ThirdPersonShooterController : MonoBehaviour 
{
        [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
        [SerializeField] private float normalSensitivity;
        [SerializeField] private float aimSensitivity;
        [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
        [SerializeField] private Transform pfBulletProjectile;
        [SerializeField] private Transform pfBlackHoleProjectile;
        [SerializeField] private Transform pfShotgunProjectile;
        [SerializeField] private Transform spawnBulletPosition;
        [SerializeField] private Transform spawnBulletPositionOg;
        [SerializeField] private Transform spawnBulletPositionCrouch;
        [SerializeField] private Transform debugTransform;
       // private Animator animator;
        
        [SerializeField] private int equippedWeapon;
        [SerializeField] private float shotgunCooldown = 1.0f;
        [SerializeField] private float shotgunSpreadAngle = 3f; // Spread angle for shotgun pellets
        private float lastShotgunTime;

        public GameObject crouchedWeaponObject;
        public GameObject originalWeaponObject;
        private bool isCrouching;

        [Header("Weapon Game Objects")]
        public GameObject standardWeaponObject;
        public GameObject blackHoleWeaponObject;
        public GameObject shotgunWeaponObject;

        private Quaternion originalRotation;

        private ThirdPersonController thirdPersonController;
        private StarterAssetsInputs starterAssetsInputs;

        [Header("Weapon Settings")]
        public int[] allWeapons = new int[]{0, 1, 2};

        private float shotCooldown;
        private float currentCooldown;
        public float standardCooldown;
        public float blackHoleCooldown;
        public Image cooldownMeter;

        

        public int standardAmmo;
        public int blackHoleAmmo;
        public int shotgunAmmo;
        public Text ammoCounter;

        [Header("Scanner Necessities")]
        public GameObject Scanningobject;
        public GameObject Scannercamera;
        public GameObject ScannerZoomCamera;
        public bool Scanenabled = false;

        [Header("Gun Audio")]
        [SerializeField] private AudioClip blasterSound ;
        //[SerializeField] private AudioClip blasterReloadSound;
        [SerializeField] private AudioClip shotgunSound;
        //[SerializeField] private AudioClip shotgunReloadSound;
        [SerializeField] private AudioClip blackHoleSound;
        //[SerializeField] private AudioClip blackHoleReloadSound;
        //[SerializeField] private AudioClip blackHoleChargeSound;

        [Header("Gun Affects  ")]
        [SerializeField] private ParticleSystem blassterFlash;
        [SerializeField] private ParticleSystem shotgunFlash;








    private void Awake()
        {
            originalRotation = transform.rotation;
            thirdPersonController = GetComponent<ThirdPersonController>();
            starterAssetsInputs = GetComponent<StarterAssetsInputs>();
          //  animator = GetComponent<Animator>();
            UpdateAmmoCount();
            currentCooldown = standardCooldown;
            isCrouching = false;
            SwitchWeaponObject(originalWeaponObject);
        }

        private void Update()
        {
            Scanning scnScr = Scanningobject.GetComponent<Scanning>();
            ScanCam scnCam = Scannercamera.GetComponent<ScanCam>();
            ScanZoom scnzCam = ScannerZoomCamera.GetComponent<ScanZoom>();
            ThirdPersonController TPC = GetComponent<ThirdPersonController>();
            Vector3 mouseWorldPosition = Vector3.zero;

            Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
            Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
            {
                debugTransform.position = raycastHit.point;
                mouseWorldPosition = raycastHit.point;
            }

            if (starterAssetsInputs.crouch)
            {
                if (!isCrouching)
                {
                    isCrouching = true;
                    SwitchWeaponObject(crouchedWeaponObject);
                }
            }
            else
            {
                if (isCrouching)
                {
                    isCrouching = false;
                    SwitchWeaponObject(originalWeaponObject);
                }
            }

        if (starterAssetsInputs.aim)
        {
            if (scnScr.Scan == false)
            {
                aimVirtualCamera.gameObject.SetActive(true);
                thirdPersonController.SetSensitivity(aimSensitivity);
                thirdPersonController.SetRotateOnMove(false);
            }

            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;
          // animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * 10f));

            // Use Lerp to smoothly interpolate between the original rotation and a tilted rotation
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(aimDirection), Time.deltaTime * 5f);

            // Disable all weapon objects first
            standardWeaponObject.SetActive(false);
            blackHoleWeaponObject.SetActive(false);
            shotgunWeaponObject.SetActive(false);

            // Activate the game object for the currently equipped weapon
            switch (equippedWeapon)
            {
                case 0:
                    standardWeaponObject.SetActive(true);
                    // Rotate the weapon object to point at the center of the screen
                    Vector3 weaponDirection = mouseWorldPosition - standardWeaponObject.transform.position;
                    standardWeaponObject.transform.forward = weaponDirection.normalized;
                    break;
                case 1:
                    blackHoleWeaponObject.SetActive(true);
                    // Rotate the weapon object to point at the center of the screen
                    Vector3 weaponDirection2 = mouseWorldPosition - blackHoleWeaponObject.transform.position;
                    blackHoleWeaponObject.transform.forward = weaponDirection2.normalized;
                    break;
                case 2:
                    shotgunWeaponObject.SetActive(true);
                    // Rotate the weapon object to point at the center of the screen
                    Vector3 weaponDirection3 = mouseWorldPosition - shotgunWeaponObject.transform.position;
                    shotgunWeaponObject.transform.forward = weaponDirection3.normalized;
                    break;
            }
                }
                    else
                {
                    aimVirtualCamera.gameObject.SetActive(false);
                    thirdPersonController.SetSensitivity(normalSensitivity);
                    thirdPersonController.SetRotateOnMove(true);
                 // animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 10f));
                    // Set the character's rotation back to its original rotation when not aiming
                    transform.rotation = Quaternion.Lerp(transform.rotation, originalRotation, Time.deltaTime * 5f);

                    // Disable all weapon objects when not aiming
                    standardWeaponObject.SetActive(false);
                    blackHoleWeaponObject.SetActive(false);
                    shotgunWeaponObject.SetActive(false);
                }

            if (starterAssetsInputs.scroll != Vector2.zero)
                {

                //equippedWeapon = equippedWeapon++;
                
                equippedWeapon = starterAssetsInputs.scroll.y > 0 ? equippedWeapon - 1 : equippedWeapon + 1;
                if (equippedWeapon > allWeapons.Length - 1)
                {
                    equippedWeapon = 0;
                }
                else if (equippedWeapon < 0)
                {
                    equippedWeapon = allWeapons.Length - 1;
                }
                shotCooldown = currentCooldown;
                UpdateAmmoCount();
                Debug.Log(equippedWeapon);

        switch (equippedWeapon)
        {
            case 0:
                if (starterAssetsInputs.aim)
                {
                    standardWeaponObject.SetActive(true);
                    blackHoleWeaponObject.SetActive(false);
                    shotgunWeaponObject.SetActive(false);
                }
                break;
            case 1:
                if (starterAssetsInputs.aim)
                {
                    standardWeaponObject.SetActive(false);
                    blackHoleWeaponObject.SetActive(true);
                    shotgunWeaponObject.SetActive(false);
                }
                break;
            case 2:
                if (starterAssetsInputs.aim)
                {
                    standardWeaponObject.SetActive(false);
                    blackHoleWeaponObject.SetActive(false);
                    shotgunWeaponObject.SetActive(true);
                }
                break;
            default:
                break;

            }
        }

            if (starterAssetsInputs.shoot)
            {
                Vector3 aimDir = (mouseWorldPosition - spawnBulletPosition.position).normalized;

                if (scnScr.Scan == false && shotCooldown >= currentCooldown)
                {
                    if (equippedWeapon == 0 && standardAmmo > 0)//Standard Projectile Shoot
                    {
                        Instantiate(pfBulletProjectile, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
                        standardAmmo -= 1;
                        currentCooldown = standardCooldown;
                        thirdPersonController.SwitchCameraTarget();
                        AudioSource.PlayClipAtPoint(blasterSound, spawnBulletPosition.position);
                        blassterFlash.Play();


                }
                else if (equippedWeapon == 1 && blackHoleAmmo > 0)//Black Hole Projectile Shoot
                    {
                        Instantiate(pfBlackHoleProjectile, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
                        blackHoleAmmo -= 1;
                        currentCooldown = blackHoleCooldown;
                        thirdPersonController.SwitchCameraTarget();
                        AudioSource.PlayClipAtPoint(blackHoleSound, spawnBulletPosition.position);


                }
                else if (equippedWeapon == 2 && shotgunAmmo > 0)
                    {        
                        shotgunAmmo -= 1;
                        currentCooldown = shotgunCooldown;
                        thirdPersonController.SwitchCameraTarget();
                        AudioSource.PlayClipAtPoint(shotgunSound, spawnBulletPosition.position);
                        shotgunFlash.Play();



                    for (int i = 0; i < 4; i++) // Fire 4 pellets in a cone
                        {
                            // Calculate a random spread angle within the specified shotgunSpreadAngle
                            float horizontalSpread = Random.Range(-shotgunSpreadAngle, shotgunSpreadAngle);
                            float verticalSpread = Random.Range(-shotgunSpreadAngle, shotgunSpreadAngle);

                            // Calculate the direction to the target
                            Vector3 directionToTarget = (mouseWorldPosition - spawnBulletPosition.position).normalized;

                            // Create a spreadDirection by rotating the direction to the target by the spread angles
                            Vector3 spreadDirection = Quaternion.Euler(verticalSpread, horizontalSpread, 0) * directionToTarget;

                            // Instantiate the shotgun pellet with the randomized direction
                            Instantiate(pfShotgunProjectile, spawnBulletPosition.position, Quaternion.LookRotation(spreadDirection, Vector3.up));
                        }
                    }
                    UpdateAmmoCount();
                    shotCooldown = 0;
                    //thirdPersonController.Recoil(0.1f);
                }
                starterAssetsInputs.shoot = false;
                
            }

            if (shotCooldown <= currentCooldown)
            {
                cooldownMeter.transform.localScale = new Vector3((shotCooldown / currentCooldown) * 0.96f, 0.8f, 1);
            }
            shotCooldown += Time.deltaTime;

    if (starterAssetsInputs.scan)
            {
                TPC.MoveSpeed = 0;
                TPC.SprintSpeed = 0;
                starterAssetsInputs.scan = true;

                scnScr.ScanCamPriority();
                
                if (starterAssetsInputs.scan == true)
                {
                    starterAssetsInputs.scan = false;

                }
                if (scnScr.Scan == false)
                {
                        TPC.MoveSpeed = TPC.NormalMovespeed;
                        TPC.SprintSpeed = TPC.NormalSprintSpeed;
                }
            }

            if (starterAssetsInputs.scanobj && scnScr.Scan == true)
            {
                
                scnCam.ScanObj();

            }
            if(starterAssetsInputs.scanaim)
            {
                starterAssetsInputs.scanaim = true;
                //Debug.Log("scanzoom pressed");

                scnzCam.ScanZoomPriority();

                if (starterAssetsInputs.scanaim == true)
                {
                    starterAssetsInputs.scanaim = false;
                }
            }
        }

    public void AddAmmo(int ammoType, int ammoAmount)
        {
            if (ammoType == 0)
            {
                standardAmmo += ammoAmount;
                UpdateAmmoCount();
            }
            else if (ammoType == 1)
            {
                blackHoleAmmo += ammoAmount;
                UpdateAmmoCount();
            }
            else if (ammoType == 2)
            {
                shotgunAmmo += ammoAmount;
                UpdateAmmoCount();
            }
        }

        public void UpdateAmmoCount()
        {
            if (equippedWeapon == 0)
            {
                ammoCounter.text = "Ammo: " + standardAmmo;
            }
            else if (equippedWeapon == 1)
            {
                ammoCounter.text = "Ammo: " + blackHoleAmmo;
            }
            else if (equippedWeapon == 2)
            {
                ammoCounter.text = "Ammo: " + shotgunAmmo;
            }
        }

         public void SwitchWeaponObject(GameObject newWeaponObject)
        {
            // Disable all weapon objects
            originalWeaponObject.SetActive(false);
            crouchedWeaponObject.SetActive(false);

            // Enable the specified weapon object
            newWeaponObject.SetActive(true);
                    // Disable all weapon objects
 

            // Update the transform of the weapon game objects based on the active weapon
            if (newWeaponObject == originalWeaponObject)
            {
                spawnBulletPosition = spawnBulletPositionOg; // Use the crouch bullet position
            }
            else if (newWeaponObject == crouchedWeaponObject)
            {
                spawnBulletPosition = spawnBulletPositionCrouch; // Use the crouch bullet position
            }

            // Update the transform of the weapon game objects based on the active weapon
            if (newWeaponObject == originalWeaponObject)
            {
                // Set the transforms for the original weapon here
                standardWeaponObject.transform.position = originalWeaponObject.transform.position;
                standardWeaponObject.transform.rotation = originalWeaponObject.transform.rotation;
                // Update other weapon transforms similarly if needed
                shotgunWeaponObject.transform.position = originalWeaponObject.transform.position;
                shotgunWeaponObject.transform.rotation = originalWeaponObject.transform.rotation;
                blackHoleWeaponObject.transform.position = originalWeaponObject.transform.position;
                blackHoleWeaponObject.transform.rotation = originalWeaponObject.transform.rotation;
            }
            else if (newWeaponObject == crouchedWeaponObject)
            {
                // Set the transforms for the crouched weapon here
                standardWeaponObject.transform.position = crouchedWeaponObject.transform.position;
                standardWeaponObject.transform.rotation = crouchedWeaponObject.transform.rotation;
                // Update other weapon transforms similarly if needed
                shotgunWeaponObject.transform.position = crouchedWeaponObject.transform.position;
                shotgunWeaponObject.transform.rotation = crouchedWeaponObject.transform.rotation;
                blackHoleWeaponObject.transform.position = crouchedWeaponObject.transform.position;
                blackHoleWeaponObject.transform.rotation = crouchedWeaponObject.transform.rotation;
            }
        }

    }