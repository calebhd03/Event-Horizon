using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Collections;
using System;
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
        [SerializeField] private Transform pfPlasmaProjectile;
        [SerializeField] private Transform pfOGBHGProjectile;
        [SerializeField] private Transform pfPullBHGProjectile;

        //[SerializeField] private Transform pfShotgunProjectile;
        [SerializeField] private Transform pfWallProjectile;
        [SerializeField] private Transform spawnBulletPosition;
        //[SerializeField] private Transform spawnShotgunBulletPosition;
        [SerializeField] public Transform spawnBlackHoleBulletPosition;
        [SerializeField] private Transform spawnBulletPositionOg;
        [SerializeField] private Transform spawnBulletPositionCrouch;
        [SerializeField] private Transform debugTransform;
        
        //private Animator animator;
        
        [SerializeField] private int equippedWeapon;
        //[SerializeField] private float shotgunCooldown = 1.0f;
        //[SerializeField] private float shotgunSpreadAngle = 3f; // Spread angle for shotgun pellets
        //private float lastShotgunTime;

        //public GameObject shotgunHolster;
        public GameObject BhgIcon;
        private bool isCrouching;

        [Header("Weapon Game Objects")]
        public GameObject standardWeaponObject;
        public GameObject blackHoleWeaponObject;
        //public GameObject shotgunWeaponObject;

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

        [Range(0.0f, 90.0f)]
        [SerializeField] private float plasmaSpreadAngle;
        [Range(0.0f, 1.0f)]
        [SerializeField] private float plasmaShotOffset;

        
        public bool reloading;
        private int ammoDifference;

        public float standardReloadTime;
        public float blackHoleReloadTime;
        //public float shotgunReloadTime;

        public TextMeshProUGUI loadedAmmoCounter;
        public TextMeshProUGUI totalAmmoCounter;

        [Header("Weapon Icons")]
        public GameObject bhgEquipped;
        public GameObject bhgSlot1;
        public GameObject bhgSlot2;

        public GameObject blasterEquipped;
        public GameObject blasterSlot1;
        public GameObject blasterSlot2;

        //public GameObject shotgunEquipped;
        //public GameObject shotgunSlot1;
        //public GameObject shotgunSlot2;

        public GameObject knifeEquipped;
        public GameObject knifeSlot1;
        public GameObject knifeSlot2;

        public GameObject ammountCountIcon;
        

        [Header("Scanner Necessities")]
        public GameObject Scanningobject;
        public GameObject Scannercamera;
        public GameObject ScannerZoomCamera;
        public bool Scanenabled = false;

        [Header("Gun Audio")]
        [SerializeField] private AudioClip blasterSound;
        [SerializeField] private AudioClip plasmaBlasterSound;
        [SerializeField] private AudioClip blasterReloadSound;
        //[SerializeField] private AudioClip shotgunSound;
        //[SerializeField] private AudioClip shotgunReloadSound;
        [SerializeField] private AudioClip blackHoleSound;
        [SerializeField] private AudioClip wallBulletSound;
        [SerializeField] private AudioClip blackHoleReloadSound;
        [SerializeField] private AudioClip blackHoleChargeSound;
        [SerializeField] private AudioClip weaponSwitchSound;

        [Header("Gun Effects")]
        [SerializeField] private ParticleSystem blasterFlash;
       //[SerializeField] private ParticleSystem shotgunFlash;
        public ParticleSystem charging, firing, cooldown;
        public Transform chargingSpawnLocation, firingSpawnLocation, cooldownSpawnLocation;

        private bool isCharging;
        private bool isCharged;
        private float chargeTime;
        private float chargeSpeed = 1f;
        public bool BHGTool = false;
        private Animator animator;

        //movement while scanning
        [SerializeField]private SkinnedMeshRenderer playermesh;
        [SerializeField] private MeshRenderer healthBar;
        //scripts
        PauseMenuScript pauseMenuScript;
        LogSystem logSystem;

        //Sound
        AudioSource audioSource;
        //skilltree
        SkillTree skillTree;

        //weapon mesh
        public NexusGun nxgun;
        //Shotgun sgun;
        private Coroutine reloadCoroutine = null;
        public Blaster bgun;
        [SerializeField] MiniCore miniCore;
        [SerializeField] Scanning scnScr;
        [SerializeField] ScanCam scnCam;
        [SerializeField] ScanZoom scnzCam;

    private void Awake()
        {
            miniCore = GetComponentInParent<MiniCore>();
            animator = GetComponent<Animator>();
            playermesh = GetComponentInChildren<SkinnedMeshRenderer>();
            originalRotation = transform.rotation;
            thirdPersonController = GetComponent<ThirdPersonController>();
            starterAssetsInputs = GetComponent<StarterAssetsInputs>();
            //animator = GetComponent<Animator>();
            UpdateAmmoCount();
            currentCooldown = standardCooldown;
            isCrouching = false;
            RefreshWeaponIcons();
            //EquipBlaster();
            SettingsScript settings = FindObjectOfType<SettingsScript>();
            pauseMenuScript = miniCore.GetComponentInChildren<PauseMenuScript>();
            logSystem = miniCore.GetComponentInChildren<LogSystem>();
            audioSource = GetComponent<AudioSource>();
            skillTree = GetComponent<SkillTree>();
            nxgun = GetComponentInChildren<NexusGun>();
            //Shotgun sgun = GetComponentInChildren<Shotgun>();
            bgun = GetComponentInChildren<Blaster>();
            scnScr = miniCore.GetComponentInChildren<Scanning>();
            scnCam = miniCore.GetComponentInChildren<ScanCam>();
            scnzCam = miniCore.GetComponentInChildren<ScanZoom>();

        }

        private void Update()
        {
            UpdateIcon();

            animator.SetInteger("EquippedWeapon", equippedWeapon);


            //Scanning scnScr = Scanningobject.GetComponent<Scanning>();
            //ScanCam scnCam = Scannercamera.GetComponent<ScanCam>();
            //ScanZoom scnzCam = ScannerZoomCamera.GetComponent<ScanZoom>();
            ThirdPersonController TPC = GetComponent<ThirdPersonController>();
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
                }
            }
            else
            {
                if (isCrouching)
                {
                    isCrouching = false;
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
                    thirdPersonController.SetSensitivity(normalSensitivity);
                    aimVirtualCamera.gameObject.SetActive(false);
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

        if ((starterAssetsInputs.scroll != Vector2.zero || starterAssetsInputs.switchWeapon) && pauseMenuScript.paused == false && Time.time - lastSwitchTime >= switchCoolDown && thirdPersonController.deathbool == false && logSystem.log == false && playerData.hasNexus == true && playerData.hasBlaster == true)
        {
            //equippedWeapon = equippedWeapon++;
                
            equippedWeapon = starterAssetsInputs.scroll.y > 0 ? equippedWeapon - 1 : equippedWeapon + 1;
            if (equippedWeapon < 0)
            {
                equippedWeapon = 2;
            }
            else if(equippedWeapon > 2)
            {
                equippedWeapon = 0;
            }

            // new weapon selecting
            shotCooldown = currentCooldown;
            UpdateAmmoCount();
            RefreshWeaponIcons();
            //Debug.Log(equippedWeapon);

            switch (equippedWeapon)
            {
                case 0:
                        if(playerData.hasBlaster == true)
                        {
                            EquipBlaster();
                            }
                    break;
                case 1:
                        if(playerData.hasNexus == true)
                        {
                            EquipBlackHoleGun();
                        }
                    break;
                case 2:
                        EquipKnife();
                        break; 
                default:
                    break;

            }
            lastSwitchTime = Time.time;
        }

        if (starterAssetsInputs.blaster && pauseMenuScript.paused == false && thirdPersonController.deathbool == false && logSystem.log == false && playerData.hasBlaster == true)
        {
            EquipBlaster();
        }

        if (starterAssetsInputs.blackHoleGun && pauseMenuScript.paused == false && thirdPersonController.deathbool == false && logSystem.log == false && playerData.hasNexus == true)
        {
            EquipBlackHoleGun();
        }

        if (starterAssetsInputs.swapBHG && pauseMenuScript.paused == false && thirdPersonController.deathbool == false && logSystem.log == false && skillTree.bHGTool == true && playerData.hasNexus == true)
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

                if (equippedWeapon == 0 && playerData.standardAmmoLoaded > 0 && playerData.hasBlaster == true)//Standard Projectile Shoot
                {
                    playerData.standardAmmoLoaded -= 1;
                    currentCooldown = standardCooldown;
                    thirdPersonController.SwitchCameraTarget();
                    blasterFlash.Play();

                    if(!playerData.SavePlasmaUpgrade)
                    {
                        Instantiate(pfBulletProjectile, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
                        audioSource.PlayOneShot(blasterSound);
                    }
                    else
                    {
                        Vector3 plasmaSpreadR = Quaternion.Euler(0, plasmaSpreadAngle, 0) * aimDir;
                        Vector3 plasmaSpreadL = Quaternion.Euler(0, -plasmaSpreadAngle, 0) * aimDir;
                        Instantiate(pfPlasmaProjectile, spawnBulletPosition.position + new Vector3 (plasmaShotOffset, 0, 0), Quaternion.LookRotation(plasmaSpreadL, Vector3.up));
                        Instantiate(pfPlasmaProjectile, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
                        Instantiate(pfPlasmaProjectile, spawnBulletPosition.position - new Vector3 (plasmaShotOffset, 0, 0), Quaternion.LookRotation(plasmaSpreadR, Vector3.up));
                        audioSource.PlayOneShot(plasmaBlasterSound);
                    }
                }
                if (equippedWeapon == 1 && playerData.hasNexus == true)
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
                            if (!isCharging)
                            {
                                audioSource.PlayOneShot(blackHoleChargeSound);
                                BHGcharging();
                            }
                        }

                    }

                }
                /*if (equippedWeapon == 2 && playerData.shotgunAmmoLoaded > 0)
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
                }*/

                else if(equippedWeapon == 2)
                {
                    currentCooldown = knifeCoolDown;
                    Debug.Log("Knife Animatoion");
                    knifeSlash = true;
                    Debug.Log("Knife Slash is true");

                    float maxDistanceKnife = 3f;
                    Vector3 raycastOffset = new Vector3(0f, 1.22f, 0f);
                    Vector3 raycastOrigin = transform.position + raycastOffset;

                    Vector3 raycastOffset2 = new Vector3(0f, 1f, 0f);
                    RaycastHit hit;
                    if (Physics.Raycast(raycastOrigin, transform.forward, out hit, maxDistanceKnife))
                    {
                        // Check if the ray has hit the target object
                        if (hit.collider.gameObject.CompareTag("Enemy"))
                        {
                            Debug.Log("KNIFE RAYCAST HIT ON ENEMY");

                            // Do whatever you need to do when a collision occurs
                            regularPoint regularDamage = hit.collider.GetComponentInChildren<regularPoint>();
                            weakPoint criticalDamage = hit.collider.GetComponentInChildren<weakPoint>();
                            if (regularDamage != null)
                            {
                                regularDamage.KnifeDamageFunction();
                                Debug.Log("KNIFE DAMAGE BABY");
                            }
                          
                            else if(criticalDamage != null)
                            {
                                criticalDamage.KnifeDamageFunction();
                                Debug.Log("MORE KNIFE DAMAGE BABY");
                            }
                        }
                    }

                    Debug.DrawRay(raycastOrigin, transform.forward * maxDistanceKnife, Color.black);
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

        if (starterAssetsInputs.scanobj && scnScr.Scan == true && logSystem.log == false)
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

        if ((playerData.standardAmmoLoaded == 0 && playerData.standardAmmo != 0) || /*playerData.shotgunAmmoLoaded == 0 ||*/ (playerData.nexusAmmoLoaded == 0 && BHGTool == false && playerData.nexusAmmo != 0))
        {
            Reload();
        }
        
        // remove scnScr.scan == true from if statement to remove requirement of being in scan mode.
        if (starterAssetsInputs.log && pauseMenuScript.paused == false && thirdPersonController.deathbool == false)// && scnScr.Scan == true)
        {
            if ((logSystem.log == false && scnScr.Scan == false) || (logSystem.log == false && scnScr.Scan == true))
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
                scnScr.MainCamPriority();
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
        //animator.ResetTrigger("ShotgunSwitch");

        //resets shotgun weapon positions
        /*shotgunWeaponObject.transform.parent = shotgunHolster.transform;
        shotgunWeaponObject.transform.position = shotgunHolster.transform.position;
        shotgunWeaponObject.transform.localEulerAngles = new Vector3(0, 90, 0);*/


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
    }

    public void EquipBlackHoleGun()
    {
        animator.SetTrigger("BHSwitch");
        animator.ResetTrigger("BlasterSwitch");
        //animator.ResetTrigger("ShotgunSwitch");

        //resets shotgun weapon positions
        /*shotgunWeaponObject.transform.parent = shotgunHolster.transform;
        shotgunWeaponObject.transform.position = shotgunHolster.transform.position;
        shotgunWeaponObject.transform.localEulerAngles = new Vector3(0, 90, 0);*/

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
    }
    public void EquipKnife()
    {
        Debug.Log("Knife Equipped");
        //animator.SetTrigger("KnifeSwitch");
        animator.ResetTrigger("BHSwitch");
        animator.ResetTrigger("BlasterSwitch");
        //animator.ResetTrigger("ShotgunSwitch");


        //resets shotgun weapon positions
        /*shotgunWeaponObject.transform.parent = shotgunHolster.transform;
        shotgunWeaponObject.transform.position = shotgunHolster.transform.position;
        shotgunWeaponObject.transform.localEulerAngles = new Vector3(0, 90, 0);*/

        equippedWeapon = 2;
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

                /*shotgunEquipped.SetActive(false);
                shotgunSlot1.SetActive(false);
                shotgunSlot2.SetActive(true);*/

                bhgEquipped.SetActive(false);
                bhgSlot1.SetActive(true);
                bhgSlot2.SetActive(false);
                
                knifeEquipped.SetActive(false);
                knifeSlot1.SetActive(false);
                knifeSlot2.SetActive(true);

                ammountCountIcon.SetActive(true);
                break;
            case 1:
                blasterEquipped.SetActive(false);
                blasterSlot1.SetActive(true);
                blasterSlot2.SetActive(false);

                /*shotgunEquipped.SetActive(false);
                shotgunSlot1.SetActive(false);
                shotgunSlot2.SetActive(true);*/

                bhgEquipped.SetActive(true);
                bhgSlot1.SetActive(false);
                bhgSlot2.SetActive(false);

                knifeEquipped.SetActive(false);
                knifeSlot1.SetActive(false);
                knifeSlot2.SetActive(true);

                ammountCountIcon.SetActive(true);
                break;
            case 2:
                Debug.Log("Knife Icon");
                blasterEquipped.SetActive(false);
                blasterSlot1.SetActive(true);
                blasterSlot2.SetActive(false);

                bhgEquipped.SetActive(false);
                bhgSlot1.SetActive(false);
                bhgSlot2.SetActive(true);

                knifeEquipped.SetActive(true);
                knifeSlot1.SetActive(false);
                knifeSlot2.SetActive(false);

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
                //playerData.shotgunAmmo += ammoAmount;
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
                totalAmmoCounter.text = "";
                loadedAmmoCounter.text = "";
            }
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
        if(newParticleSystem.GetComponent<BHGCharge>() != null) newParticleSystem.GetComponent<BHGCharge>().tpsc = this;
        newParticleSystem.Play();

        yield return new WaitForSeconds(duration);

        Destroy(newParticleSystem.gameObject);
    }

    private void Reload()
    {
        // Check if already reloading to prevent multiple reloads at the same time
        if (reloading)
            return;

        reloading = true;
        Debug.Log("Reloading!");

        float reloadTime = 0f;
        AudioClip reloadSound = null;

        switch (equippedWeapon)
        {
            case 0:
                if (playerData.standardAmmo > 0 && playerData.standardAmmoLoaded < playerData.standardAmmoMax)
                {
                    ammoDifference = playerData.standardAmmoMax - playerData.standardAmmoLoaded;
                    int ammoToLoad = Math.Min(playerData.standardAmmo, ammoDifference);
                    playerData.standardAmmoLoaded += ammoToLoad;
                    playerData.standardAmmo -= ammoToLoad;

                    reloadTime = standardReloadTime;
                    reloadSound = blasterReloadSound;
                }
                break;
            case 1:
                if (playerData.nexusAmmo > 0 && playerData.nexusAmmoLoaded < playerData.nexusAmmoMax)
                {
                    ammoDifference = playerData.nexusAmmoMax - playerData.nexusAmmoLoaded;
                    int ammoToLoad = Math.Min(playerData.nexusAmmo, ammoDifference);
                    playerData.nexusAmmoLoaded += ammoToLoad;
                    playerData.nexusAmmo -= ammoToLoad;

                    reloadTime = blackHoleReloadTime;
                    reloadSound = blackHoleReloadSound;
                }
                break;
            // Implement additional weapon cases as needed
        }

        if (reloadTime > 0 && reloadSound != null)
        {
            reloadCoroutine = StartCoroutine(ReloadTimer(reloadTime));
            audioSource.PlayOneShot(reloadSound);
        }
        else
        {
            reloading = false; // No reload needed, reset immediately
        }

        UpdateAmmoCount(); // Ensure that ammo counts are updated after reloading
    }

    IEnumerator ReloadTimer(float reloadTime)
    {
        yield return new WaitForSeconds(reloadTime);
        reloading = false;
        reloadCoroutine = null;
    }

    private void InterruptReload()
    {
        if (reloadCoroutine != null)
        {
            StopCoroutine(reloadCoroutine);
            reloadCoroutine = null;
        }
        reloading = false;
        Debug.Log("Reloading interrupted and reset.");
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
        playermesh.enabled = true;
        healthBar.enabled = true;
        if(playerData.hasNexus == true)
        {
        nxgun.EnableMesh();
        }
        if(playerData.hasBlaster == true)
        {
        bgun.EnableMesh();
        }
        //sgun.EnableMesh();
    }
    public void DisablePlayerMesh()
    {
        playermesh.enabled = false;
        healthBar.enabled = false;
        nxgun.DisableMesh();
        bgun.DisableMesh();
        //sgun.DisableMesh();
    }

    public void changeSens(float newChangeSens)
    {
        normalSensitivity = newChangeSens;
    }
    public void changeAimSens(float newChangeSens)
    {
        aimSensitivity = newChangeSens;
    }
    public void EnableNXGunMesh()
    {
        nxgun.EnableMesh();
    }
    public void EnableBGunMesh()
    {
        bgun.EnableMesh();
    }
}