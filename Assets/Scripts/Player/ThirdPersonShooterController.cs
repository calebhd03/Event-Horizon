using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Collections;
using TMPro;
public class ThirdPersonShooterController : MonoBehaviour 
{
    public PlayerData playerData;
        [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
        [SerializeField] private float normalSensitivity;
        [SerializeField] private float aimSensitivity;
        [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
        [SerializeField] private Transform pfBulletProjectile;
        [SerializeField] private Transform pfBlackHoleProjectile;
        [SerializeField] private Transform pfShotgunProjectile;
        [SerializeField] private Transform pfWallProjectile;
        [SerializeField] private Transform spawnBulletPosition;
        [SerializeField] private Transform spawnShotgunBulletPosition;
        [SerializeField] public Transform spawnBlackHoleBulletPosition;
        [SerializeField] private Transform spawnBulletPositionOg;
        [SerializeField] private Transform spawnBulletPositionCrouch;
        [SerializeField] private Transform debugTransform;
        
        //private Animator animator;
        
        [SerializeField] private int equippedWeapon;
        [SerializeField] private float shotgunCooldown = 1.0f;
        [SerializeField] private float shotgunSpreadAngle = 3f; // Spread angle for shotgun pellets
        private float lastShotgunTime;

        public GameObject blasterHolster;
        public GameObject BHGHolster;
        public GameObject shotgunHolster;
        public GameObject crouchedWeaponObject;
        public GameObject originalWeaponObject;
        public GameObject BhgIcon;
        private bool isCrouching;

        [Header("Weapon Game Objects")]
        public GameObject standardWeaponObject;
        public GameObject blackHoleWeaponObject;
        public GameObject shotgunWeaponObject;

        private float lastSwitchTime;
        private float switchCoolDown = 0.5f;

        private Quaternion originalRotation;

        private ThirdPersonController thirdPersonController;
        private StarterAssetsInputs starterAssetsInputs;

        [Header("Weapon Settings")]
        public int[] allWeapons = new int[]{0, 1, 2};

        private float shotCooldown;
        private float currentCooldown;
        public float standardCooldown;
        public float blackHoleCooldown;
        public float knifeCoolDown;
        public bool knifeSlash = false;
        public Image cooldownMeter;

        
        public bool reloading;
        private int ammoDifference;

        public float standardReloadTime;
        public float blackHoleReloadTime;
        public float shotgunReloadTime;

        public TextMeshProUGUI loadedAmmoCounter;
        public TextMeshProUGUI totalAmmoCounter;

        [Header("Weapon Icons")]
        public GameObject bhgEquipped;
        public GameObject bhgSlot1;
        public GameObject bhgSlot2;

        public GameObject blasterEquipped;
        public GameObject blasterSlot1;
        public GameObject blasterSlot2;
        
        public GameObject shotgunEquipped;
        public GameObject shotgunSlot1;
        public GameObject shotgunSlot2;

        public GameObject ammountCountIcon;
        

        [Header("Scanner Necessities")]
        public GameObject Scanningobject;
        public GameObject Scannercamera;
        public GameObject ScannerZoomCamera;
        public bool Scanenabled = false;

        [Header("Gun Audio")]
        [SerializeField] private AudioClip blasterSound ;
        [SerializeField] private AudioClip blasterReloadSound;
        [SerializeField] private AudioClip shotgunSound;
        [SerializeField] private AudioClip shotgunReloadSound;
        [SerializeField] private AudioClip blackHoleSound;
        [SerializeField] private AudioClip wallBulletSound;
        [SerializeField] private AudioClip blackHoleReloadSound;
        [SerializeField] private AudioClip blackHoleChargeSound;
        [SerializeField] private AudioClip weaponSwitchSound;

        [Header("Gun Effects")]
        [SerializeField] private ParticleSystem blassterFlash;
        [SerializeField] private ParticleSystem shotgunFlash;
        public ParticleSystem charging, firing, cooldown;
        public Transform chargingSpawnLocation, firingSpawnLocation, cooldownSpawnLocation;

        private bool isCharging;
        private bool isCharged;
        private float chargeTime;
        private float chargeSpeed = 1f;
        public bool BHGTool = false;
        private Animator animator;

        //movement while scanning
        private SkinnedMeshRenderer playermesh;
        //scripts
        PauseMenuScript pauseMenuScript;
        LogSystem logSystem;

        //Sound
        AudioSource audioSource;


        private void Awake()
        {
            animator = GetComponent<Animator>();
            playermesh = GetComponentInChildren<SkinnedMeshRenderer>();
            originalRotation = transform.rotation;
            thirdPersonController = GetComponent<ThirdPersonController>();
            starterAssetsInputs = GetComponent<StarterAssetsInputs>();
            //animator = GetComponent<Animator>();
            UpdateAmmoCount();
            currentCooldown = standardCooldown;
            isCrouching = false;
            SwitchWeaponObject(originalWeaponObject);
            RefreshWeaponIcons();
            EquipBlaster();
            SettingsScript settings = FindObjectOfType<SettingsScript>();
            pauseMenuScript = FindObjectOfType<PauseMenuScript>();
            logSystem = FindObjectOfType<LogSystem>();
            audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            UpdateIcon();
            
            Scanning scnScr = Scanningobject.GetComponent<Scanning>();
            ScanCam scnCam = Scannercamera.GetComponent<ScanCam>();
            ScanZoom scnzCam = ScannerZoomCamera.GetComponent<ScanZoom>();
            ThirdPersonController TPC = GetComponent<ThirdPersonController>();
            NexusGun nxgun = GetComponentInChildren<NexusGun>();
            Shotgun sgun = GetComponentInChildren<Shotgun>();
            Blaster bgun = GetComponentInChildren<Blaster>();
            Vector3 mouseWorldPosition = Vector3.zero;

            Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
            Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
            {
                debugTransform.position = raycastHit.point;
                mouseWorldPosition = raycastHit.point;
            }
            else
            {
                debugTransform.position = ray.GetPoint(99f);
                mouseWorldPosition = ray.GetPoint(99f);
            }

            if (starterAssetsInputs.ammo)
            {
                addDevAmmo();
                Debug.Log("Ammo Dev");
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
            
            if (starterAssetsInputs.quit)
            {
                Application.Quit();
            }

            if (scnScr.Scan == false)
            {
                if (starterAssetsInputs.aim)
                {

                        aimVirtualCamera.gameObject.SetActive(true);
                        thirdPersonController.SetSensitivity(aimSensitivity);
                        thirdPersonController.SetRotateOnMove(false);
                    

                    Vector3 worldAimTarget = mouseWorldPosition;
                    worldAimTarget.y = transform.position.y;
                    Vector3 aimDirection = (worldAimTarget - transform.position).normalized;
                // animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * 10f));

                    // Use Lerp to smoothly interpolate between the original rotation and a tilted rotation
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(aimDirection), Time.deltaTime * 5f);

                    // Disable all weapon objects first
                    //standardWeaponObject.SetActive(false);
                    //blackHoleWeaponObject.SetActive(false);
                    //shotgunWeaponObject.SetActive(false);

                    // Activate the game object for the currently equipped weapon
                    //switch (equippedWeapon)
                   // {
                    /*
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
                            break;*/
                    //}
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
                            //standardWeaponObject.SetActive(false);
                            //blackHoleWeaponObject.SetActive(false);
                            //shotgunWeaponObject.SetActive(false);
                        }
            }

        if (starterAssetsInputs.scroll != Vector2.zero && pauseMenuScript.paused == false && thirdPersonController.deathbool == false && logSystem.log == false)
            {

            //equippedWeapon = equippedWeapon++;
                
            /*   equippedWeapon = starterAssetsInputs.scroll.y > 0 ? equippedWeapon - 1 : equippedWeapon + 1;
            if (equippedWeapon > allWeapons.Length - 1)
            {
                equippedWeapon = 0;
            }
            else if (equippedWeapon < 0)
            {
                equippedWeapon = allWeapons.Length - 1;
            }*/

            // new weapon selecting
            equippedWeapon = starterAssetsInputs.scroll.y > 0 ? equippedWeapon = 0 : equippedWeapon = 2;
            shotCooldown = currentCooldown;
            UpdateAmmoCount();
            RefreshWeaponIcons();
            Debug.Log(equippedWeapon);
                

        switch (equippedWeapon)
        {
            case 0:
                    EquipBlaster();
                break;
            case 1:
                    EquipBlackHoleGun();
                break;
            case 2:
                    EquipShotgun();
                break;
                case 3:
                    EquipKnife();
                    break; 
            default:
                break;

            }
        }

        if (starterAssetsInputs.switchWeapon && Time.time - lastSwitchTime >= switchCoolDown && pauseMenuScript.paused == false && thirdPersonController.deathbool == false && logSystem.log == false)
        {
            if (equippedWeapon != 0)
            {
                EquipBlaster();
            }
            else
            {
                EquipShotgun();
            }
            lastSwitchTime = Time.time;
            shotCooldown = currentCooldown;
            UpdateAmmoCount();
            Debug.Log(equippedWeapon);
        }

        if (starterAssetsInputs.blaster && pauseMenuScript.paused == false && thirdPersonController.deathbool == false && logSystem.log == false)
        {
            EquipBlaster();
        }

        if (starterAssetsInputs.blackHoleGun && pauseMenuScript.paused == false && thirdPersonController.deathbool == false && logSystem.log == false)
        {
            EquipBlackHoleGun();
        }

        if (starterAssetsInputs.swapBHG && pauseMenuScript.paused == false && thirdPersonController.deathbool == false && logSystem.log == false)
        {
            if (equippedWeapon == 1)
            {
                BHGTool = !BHGTool;
            }

            if (starterAssetsInputs.swapBHG == true)
            {
                starterAssetsInputs.swapBHG = false;

            }
        }

        if (starterAssetsInputs.shotgun && pauseMenuScript.paused == false && thirdPersonController.deathbool == false && logSystem.log == false)
        {
            EquipShotgun();
        }

        if (starterAssetsInputs.knife && pauseMenuScript.paused == false && thirdPersonController.deathbool == false && logSystem.log == false)
        {
            EquipKnife();
        }

        if (starterAssetsInputs.reload && pauseMenuScript.paused == false && thirdPersonController.deathbool == false && logSystem.log == false)
        {
            Reload();
        }

        if (starterAssetsInputs.shoot && pauseMenuScript.paused == false && thirdPersonController.deathbool == false && logSystem.log == false)
        {
            Vector3 aimDir = (mouseWorldPosition - spawnBulletPosition.position).normalized;

            if (scnScr.Scan == false && shotCooldown >= currentCooldown && !reloading)
            {
                //play shoot animation
                animator.SetTrigger("Shoot");

                if (equippedWeapon == 0 && playerData.standardAmmoLoaded > 0)//Standard Projectile Shoot
                {
                    Instantiate(pfBulletProjectile, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
                    playerData.standardAmmoLoaded -= 1;
                    currentCooldown = standardCooldown;
                    thirdPersonController.SwitchCameraTarget();
                    audioSource.PlayOneShot(blasterSound);
                    blassterFlash.Play();
                }
                if (equippedWeapon == 1 )
                {   
                    if (BHGTool == true)
                    {
                        
                        if (playerData.nexusAmmoLoaded > 0 || playerData.nexusAmmoLoaded <= 0)
                        {
                        
                         BhgIcon.SetActive(false);
                        Instantiate(pfWallProjectile, spawnBlackHoleBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
                        currentCooldown = blackHoleCooldown;
                        thirdPersonController.SwitchCameraTarget();
                        audioSource.PlayOneShot(wallBulletSound);
                        }
                    }
                    else if (playerData.nexusAmmoLoaded > 0 )
                    {
                        
                       // BhgIcon.SetActive(true);
                        // Black Hole Projectile Shoot
                        if (isCharged)
                        {

                            Instantiate(pfBlackHoleProjectile, spawnBlackHoleBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
                            playerData.nexusAmmoLoaded -= 1;
                            currentCooldown = blackHoleCooldown;
                            thirdPersonController.SwitchCameraTarget();
                            audioSource.PlayOneShot(blackHoleSound);
                            BHGfiring();
                            chargeTime = 0;
                            isCharged = false;
                        }
                        else
                        {
                            audioSource.PlayOneShot(blackHoleChargeSound);
                            BHGcharging();
                        }

                    }


                }
                if (equippedWeapon == 2 && playerData.shotgunAmmoLoaded > 0)
                {        
                    playerData.shotgunAmmoLoaded -= 1;
                    currentCooldown = shotgunCooldown;
                    thirdPersonController.SwitchCameraTarget();
                    audioSource.PlayOneShot(shotgunSound);
                    shotgunFlash.Play();
                    for (int i = 0; i < 4; i++) // Fire 4 pellets in a cone
                    {
                        // Calculate a random spread angle within the specified shotgunSpreadAngle
                        float horizontalSpread = Random.Range(-shotgunSpreadAngle, shotgunSpreadAngle);
                        float verticalSpread = Random.Range(-shotgunSpreadAngle, shotgunSpreadAngle);

                        // Calculate the direction to the target
                        Vector3 directionToTarget = (mouseWorldPosition - spawnShotgunBulletPosition.position).normalized;

                        // Create a spreadDirection by rotating the direction to the target by the spread angles
                        Vector3 spreadDirection = Quaternion.Euler(verticalSpread, horizontalSpread, 0) * directionToTarget;

                        // Instantiate the shotgun pellet with the randomized direction
                        Instantiate(pfShotgunProjectile, spawnShotgunBulletPosition.position, Quaternion.LookRotation(spreadDirection, Vector3.up));
                    }
                }

                else if(equippedWeapon == 3)
                {
                    currentCooldown = knifeCoolDown;
                    Debug.Log("Knife Animatoion");
                    knifeSlash = true;
                    Debug.Log("Knife Slash is true");
                }
                UpdateAmmoCount();
                shotCooldown = 0;
                //thirdPersonController.Recoil(0.1f);
            }
            starterAssetsInputs.shoot = false;  
        }

        if (shotCooldown <= currentCooldown && currentCooldown != 0)
        {
            cooldownMeter.transform.localScale = new Vector3((shotCooldown / currentCooldown) * 0.96f, 0.8f, 1);
        }

        //
        shotCooldown += Time.deltaTime;
        if (shotCooldown >= currentCooldown)
        {
            animator.ResetTrigger("Shoot");
            knifeSlash = false;
        }

        if (isCharging == true)
        {
            chargeTime += Time.unscaledDeltaTime * chargeSpeed;
        }

        if (chargeTime >= 3f)
        {
            isCharged = true;
            isCharging = false;
        }
    
    if (starterAssetsInputs.scan && pauseMenuScript.paused == false && thirdPersonController.deathbool == false && logSystem.log == false)
            {
                DisablePlayerMesh();
                starterAssetsInputs.scan = true;
                // remove if and else if statement and leave following line for log to open not in scanner. 
                scnScr.SwitchCamPriority();
                /*if (logSystem.log == false)
                {
                    scnScr.SwitchCamPriority();
                }
                
                else if (logSystem == true)
                {
                    scnScr.ScanCamPriority();
                    logSystem.CloseLog();
                }*/
                
                if (starterAssetsInputs.scan == true)
                {
                    starterAssetsInputs.scan = false;

                }

                if (scnScr.Scan == false)
                {
                    EnablePlayerMesh();
                }
            }

            if (starterAssetsInputs.scanobj && scnScr.Scan == true)
            {
                scnCam.ScanObj();
            }
            else 
            {
                scnCam.StopScanObj();
            }

            if (scnScr.Scan == false)
            {
                starterAssetsInputs.scanaim = false;
            }
            if(starterAssetsInputs.scanaim && scnScr.Scan == true)
            {
                starterAssetsInputs.scanaim = true;
                //Debug.Log("scanzoom pressed");
                scnzCam.ScanZoomPriority();

                if (starterAssetsInputs.scanaim == true)
                {
                    starterAssetsInputs.scanaim = false;
                }
            }

            if (playerData.standardAmmoLoaded == 0 || playerData.shotgunAmmoLoaded == 0 || (playerData.nexusAmmoLoaded == 0 && BHGTool == false))
            {
                Reload();
            }
            
            // remove scnScr.scan == true from if statement to remove requirement of being in scan mode.
            if (starterAssetsInputs.log && pauseMenuScript.paused == false && thirdPersonController.deathbool == false)// && scnScr.Scan == true)
            {
                if (logSystem.log == false)
                {
                    logSystem.SetLog();
                    DisablePlayerMesh();
                   // Debug.LogWarning("this");
                                    // for scanner not used
                    scnScr.ScanCamPriority();
                }
                else
                {
                    logSystem.CloseLog();
                    EnablePlayerMesh();
                   // Debug.LogWarning("this2");
                                    // for scanner not used
                    scnScr.SwitchCamPriority();
                }
                starterAssetsInputs.log = true;
                // for scanner == true
                //scnScr.ScanCamPriority();

                
                
                
                if (starterAssetsInputs.log == true)
                {
                    starterAssetsInputs.log = false;
                }
            }
        }
        

    public void EquipBlaster()
    {
        animator.SetTrigger("BlasterSwitch");
        animator.ResetTrigger("BHSwitch");
        animator.ResetTrigger("ShotgunSwitch");

        //sets blaster weapon position to in hand
        standardWeaponObject.transform.parent = originalWeaponObject.transform;
        standardWeaponObject.transform.position = originalWeaponObject.transform.position;
        standardWeaponObject.transform.localEulerAngles = new Vector3(-90, 0, 90);

        //resets BHG weapon positions
        blackHoleWeaponObject.transform.parent = BHGHolster.transform;
        blackHoleWeaponObject.transform.position = BHGHolster.transform.position;
        blackHoleWeaponObject.transform.localEulerAngles = new Vector3(90, 0, -45);

        //resets shotgun weapon positions
        shotgunWeaponObject.transform.parent = shotgunHolster.transform;
        shotgunWeaponObject.transform.position = shotgunHolster.transform.position;
        shotgunWeaponObject.transform.localEulerAngles = new Vector3(0, 90, 0);


        if (starterAssetsInputs.aim)
        {
            animator.SetTrigger("aimGun");
            //standardWeaponObject.SetActive(true);
            //blackHoleWeaponObject.SetActive(false);
            //shotgunWeaponObject.SetActive(false);
        }
        else if (!starterAssetsInputs.aim)
        {
            animator.ResetTrigger("aimGun");
        }
        equippedWeapon = 0;
        shotCooldown = currentCooldown;
        RefreshWeaponIcons();
        UpdateAmmoCount();
        //Debug.Log(equippedWeapon);
    }
    public void EquipShotgun()
    {
        animator.SetTrigger("ShotgunSwitch");
        animator.ResetTrigger("BlasterSwitch");
        animator.ResetTrigger("BHSwitch");


        //resets Blaster weapon positions
        standardWeaponObject.transform.parent = blasterHolster.transform;
        standardWeaponObject.transform.position = blasterHolster.transform.position;
        standardWeaponObject.transform.localEulerAngles = new Vector3(0, 0, 0);

        //resets BHG weapon positions
        blackHoleWeaponObject.transform.parent = BHGHolster.transform;
        blackHoleWeaponObject.transform.position = BHGHolster.transform.position;
        blackHoleWeaponObject.transform.localEulerAngles = new Vector3(90, 0, -45);

        //sets shotgun position to in hand
        shotgunWeaponObject.transform.parent = originalWeaponObject.transform;
        shotgunWeaponObject.transform.position = originalWeaponObject.transform.position;
        shotgunWeaponObject.transform.localEulerAngles = new Vector3(-90, 0, 90);

        if (starterAssetsInputs.aim)
        {
            animator.SetTrigger("aimGun");
            //standardWeaponObject.SetActive(false);
            //blackHoleWeaponObject.SetActive(false);
            //shotgunWeaponObject.SetActive(true);
        }
        else if (!starterAssetsInputs.aim)
        {
            animator.ResetTrigger("aimGun");
        }
        equippedWeapon = 2;
        shotCooldown = currentCooldown;
        RefreshWeaponIcons();
        UpdateAmmoCount();
        Debug.Log(equippedWeapon);
    }
    public void EquipBlackHoleGun()
    {
        animator.SetTrigger("BHSwitch");
        animator.ResetTrigger("BlasterSwitch");
        animator.ResetTrigger("ShotgunSwitch");

        //resets blaster weapon positions
        standardWeaponObject.transform.parent = blasterHolster.transform;
        standardWeaponObject.transform.position = blasterHolster.transform.position;
        standardWeaponObject.transform.localEulerAngles = new Vector3(0, 0, 0);

        //sets blaster weapon position to in hand
        blackHoleWeaponObject.transform.parent = originalWeaponObject.transform;
        blackHoleWeaponObject.transform.position = originalWeaponObject.transform.position;
        blackHoleWeaponObject.transform.localEulerAngles = new Vector3(-90, 0, 90);

        //resets shotgun weapon positions
        shotgunWeaponObject.transform.parent = shotgunHolster.transform;
        shotgunWeaponObject.transform.position = shotgunHolster.transform.position;
        shotgunWeaponObject.transform.localEulerAngles = new Vector3(0, 90, 0);

        if (starterAssetsInputs.aim)
        {
            animator.SetTrigger("aimGun");
            //standardWeaponObject.SetActive(false);
            //blackHoleWeaponObject.SetActive(false);
            //shotgunWeaponObject.SetActive(true);
        }
        else if (!starterAssetsInputs.aim)
        {
            animator.ResetTrigger("aimGun");
        }
        equippedWeapon = 1;
        shotCooldown = currentCooldown;
        RefreshWeaponIcons();
        UpdateAmmoCount();
        Debug.Log(equippedWeapon);
    }
    public void EquipKnife()
    {
        Debug.Log("Knife Equipped");
        //animator.SetTrigger("KnifeSwitch");
        animator.ResetTrigger("BHSwitch");
        animator.ResetTrigger("BlasterSwitch");
        animator.ResetTrigger("ShotgunSwitch");

        //resets blaster weapon positions
        standardWeaponObject.transform.parent = blasterHolster.transform;
        standardWeaponObject.transform.position = blasterHolster.transform.position;
        standardWeaponObject.transform.localEulerAngles = new Vector3(0, 0, 0);

        //resets shotgun weapon positions
        shotgunWeaponObject.transform.parent = shotgunHolster.transform;
        shotgunWeaponObject.transform.position = shotgunHolster.transform.position;
        shotgunWeaponObject.transform.localEulerAngles = new Vector3(0, 90, 0);

        //resets BHG weapon positions
        blackHoleWeaponObject.transform.parent = BHGHolster.transform;
        blackHoleWeaponObject.transform.position = BHGHolster.transform.position;
        blackHoleWeaponObject.transform.localEulerAngles = new Vector3(90, 0, -45);
        equippedWeapon = 3;
        RefreshWeaponIcons();
        shotCooldown = currentCooldown;
    }
    public void RefreshWeaponIcons()
    {
        switch (equippedWeapon)
        {
            case 0:
                blasterEquipped.SetActive(true);
                blasterSlot1.SetActive(false);
                blasterSlot2.SetActive(false);

                shotgunEquipped.SetActive(false);
                shotgunSlot1.SetActive(false);
                shotgunSlot2.SetActive(true);

                bhgEquipped.SetActive(false);
                bhgSlot1.SetActive(true);
                bhgSlot2.SetActive(false);

                ammountCountIcon.SetActive(true);
                break;
            case 1:
                blasterEquipped.SetActive(false);
                blasterSlot1.SetActive(true);
                blasterSlot2.SetActive(false);

                shotgunEquipped.SetActive(false);
                shotgunSlot1.SetActive(false);
                shotgunSlot2.SetActive(true);

                bhgEquipped.SetActive(true);
                bhgSlot1.SetActive(false);
                bhgSlot2.SetActive(false);

                ammountCountIcon.SetActive(true);
                break;
            case 2:
                blasterEquipped.SetActive(false);
                blasterSlot1.SetActive(false);
                blasterSlot2.SetActive(true);

                shotgunEquipped.SetActive(true);
                shotgunSlot1.SetActive(false);
                shotgunSlot2.SetActive(false);

                bhgEquipped.SetActive(false);
                bhgSlot1.SetActive(true);
                bhgSlot2.SetActive(false);

                ammountCountIcon.SetActive(true);
                break;
            case 3:
                Debug.Log("Knife Icon");
                blasterEquipped.SetActive(false);
                blasterSlot1.SetActive(false);
                blasterSlot2.SetActive(false);

                shotgunEquipped.SetActive(false);
                shotgunSlot1.SetActive(false);
                shotgunSlot2.SetActive(false);

                bhgEquipped.SetActive(false);
                bhgSlot1.SetActive(false);
                bhgSlot2.SetActive(false);

                ammountCountIcon.SetActive(false);
                break;
        }
    }

    public void AddAmmo(int ammoType, int ammoAmount)
        {
            if (ammoType == 0)
            {
                playerData.standardAmmo += ammoAmount;
                UpdateAmmoCount();
            }
            else if (ammoType == 1)
            {
                playerData.nexusAmmo += ammoAmount;
                UpdateAmmoCount();
            }
            else if (ammoType == 2)
            {
                playerData.shotgunAmmo += ammoAmount;
                UpdateAmmoCount();
            }
        }

        private void addDevAmmo()
        {
            int ammoToAdd = 50;

            switch (equippedWeapon)
            {
                case 0:
                    playerData.standardAmmoLoaded = ammoToAdd;
                    break;
                case 1:
                    playerData.nexusAmmoLoaded = ammoToAdd;
                    break;
                case 2:
                    playerData.shotgunAmmoLoaded = ammoToAdd;
                    break;
            }

            UpdateAmmoCount();
            Debug.Log("Ammo added: " + ammoToAdd);
        }

        public void UpdateAmmoCount()
        {
            if (equippedWeapon == 0)
            {
                totalAmmoCounter.text = playerData.standardAmmo.ToString();
                loadedAmmoCounter.text = playerData.standardAmmoLoaded.ToString();
            }
            else if (equippedWeapon == 1)
            {
                totalAmmoCounter.text =  playerData.nexusAmmo.ToString();
                loadedAmmoCounter.text = playerData.nexusAmmoLoaded.ToString();
            }
            else if (equippedWeapon == 2)
            {
                totalAmmoCounter.text = playerData.shotgunAmmo.ToString();
                loadedAmmoCounter.text = playerData.shotgunAmmoLoaded.ToString();
            }
        }
        

         public void SwitchWeaponObject(GameObject newWeaponObject)
        {
            reloading = false;
            // Disable all weapon objects
            originalWeaponObject.SetActive(false);
            crouchedWeaponObject.SetActive(false);

            // Enable the specified weapon object
            newWeaponObject.SetActive(true);
                    // Disable all weapon objects
 

            // Update the transform of the weapon game objects based on the active weapon
           /* if (newWeaponObject == originalWeaponObject)
            {
                spawnBulletPosition = spawnBulletPositionOg; // Use the crouch bullet position
            }
            else if (newWeaponObject == crouchedWeaponObject)
            {
                spawnBulletPosition = spawnBulletPositionCrouch; // Use the crouch bullet position
            }*/

            // Update the transform of the weapon game objects based on the active weapon
            /*
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
            */
        }

    public void BHGcharging()
    {
        isCharging = true;
        StartCoroutine(PlayForDuration(charging, chargingSpawnLocation, 3f));
        
    }

    public void BHGfiring()
    {
        StopAllCoroutines();
        charging.Stop();
        StartCoroutine(PlayForDuration(firing, firingSpawnLocation, 2f));
        StartCoroutine(PlayForDuration(cooldown, cooldownSpawnLocation, 2f));
        isCharged = false;
    }

    public void BHGcooldown()
    {
        StartCoroutine(PlayForDuration(cooldown, cooldownSpawnLocation, 2f));
    }

    IEnumerator PlayForDuration(ParticleSystem particleSystem, Transform spawnLocation, float duration)
    {
        ParticleSystem newParticleSystem = Instantiate(particleSystem, spawnLocation.position, spawnLocation.rotation);
        if(newParticleSystem.GetComponent<BHG>() != null) newParticleSystem.GetComponent<BHG>().tpsc = this;
        newParticleSystem.Play();

        yield return new WaitForSeconds(duration);

        Destroy(newParticleSystem.gameObject);
    }

    private void Reload()
    {
        if (playerData.standardAmmoLoaded != playerData.standardAmmoMax || playerData.nexusAmmoLoaded != playerData.nexusAmmoMax || playerData.shotgunAmmoLoaded != playerData.shotgunAmmoMax)
        {
        reloading = true;

        Debug.Log("Reloading!");
        if(equippedWeapon == 0 && playerData.standardAmmo > 0 && playerData.standardAmmoLoaded < playerData.standardAmmoMax)
        {
            StartCoroutine(ReloadTimer(standardReloadTime));
            audioSource.PlayOneShot(blasterReloadSound);
            ammoDifference = playerData.standardAmmoMax - playerData.standardAmmoLoaded;
            playerData.standardAmmoLoaded += playerData.standardAmmo;
            playerData.standardAmmo -= ammoDifference;
            if (playerData.standardAmmo < 0)
            {
                playerData.standardAmmo = 0;
            }
            else
            {
                playerData.standardAmmoLoaded = playerData.standardAmmoMax;
            }
        }
        else if(equippedWeapon == 1 && playerData.nexusAmmo > 0 && playerData.nexusAmmoLoaded < playerData.nexusAmmoMax && BHGTool == false)
        {
            StartCoroutine(ReloadTimer(blackHoleReloadTime));
            audioSource.PlayOneShot(blackHoleReloadSound);
            ammoDifference = playerData.nexusAmmoMax - playerData.nexusAmmoLoaded;
            playerData.nexusAmmoLoaded += playerData.nexusAmmo;
            playerData.nexusAmmo -= ammoDifference;
            if (playerData.standardAmmo < 0)
            {
                playerData.nexusAmmo = 0;
            }
            else
            {
                playerData.nexusAmmoLoaded = playerData.nexusAmmoMax;
            }
        }
        else if(equippedWeapon == 2 && playerData.shotgunAmmo > 0 && playerData.shotgunAmmoLoaded < playerData.shotgunAmmoMax)
        {
            StartCoroutine(ReloadTimer(shotgunReloadTime));
            audioSource.PlayOneShot(shotgunReloadSound);
            ammoDifference = playerData.shotgunAmmoMax - playerData.shotgunAmmoLoaded;
            playerData.shotgunAmmoLoaded += playerData.shotgunAmmo;
            playerData.shotgunAmmo -= ammoDifference;
            if (playerData.shotgunAmmo < 0)
            {
                playerData.shotgunAmmo = 0;
            }
            else
            {
                playerData.shotgunAmmoLoaded = playerData.shotgunAmmoMax;
            }
        }
        UpdateAmmoCount();
        }
    }

    IEnumerator ReloadTimer(float reloadTime)
    {
        yield return new WaitForSeconds(reloadTime);
        reloading = false;
    }

    private void UpdateIcon()
    {
        if(BHGTool == true && equippedWeapon == 1 )
         {
             BhgIcon.SetActive(false);
             reloading = false;
         }
         else if( equippedWeapon == 1)
        {
           BhgIcon.SetActive(true);
         }
        else
        {
         BhgIcon.SetActive(false);
        }
    }
    public void EnablePlayerMesh()
    {
        NexusGun nxgun = GetComponentInChildren<NexusGun>();
        Shotgun sgun = GetComponentInChildren<Shotgun>();
        Blaster bgun = GetComponentInChildren<Blaster>();
        playermesh.enabled = true;
        nxgun.EnableMesh();
        bgun.EnableMesh();
        sgun.EnableMesh();
    }
    public void DisablePlayerMesh()
    {
        NexusGun nxgun = GetComponentInChildren<NexusGun>();
        Shotgun sgun = GetComponentInChildren<Shotgun>();
        Blaster bgun = GetComponentInChildren<Blaster>();
        playermesh.enabled = false;
        nxgun.DisableMesh();
        bgun.DisableMesh();
        sgun.DisableMesh();
    }

    public void changeSens(float newChangeSens)
    {
        normalSensitivity = newChangeSens;
    }
}