using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Collections;
using TMPro;
public class ThirdPersonShooterController : MonoBehaviour 
{
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
        public Image cooldownMeter;

        
        public bool reloading;
        private int ammoDifference;
        public int standardAmmo;
        public int standardAmmoLoaded;
        public int standardAmmoMax;
        public float standardReloadTime;

        public int blackHoleAmmo;
        public int blackHoleAmmoLoaded;
        public int blackHoleAmmoMax;
        public float blackHoleReloadTime;

        public int shotgunAmmo;
        public int shotgunAmmoLoaded;
        public int shotgunAmmoMax;
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
        private Animator animator;

        //movement while scanning
        private SkinnedMeshRenderer playermesh;

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
        }

        private void Update()
        {
            UpdateIcon();
            
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
            }

        if (starterAssetsInputs.scroll != Vector2.zero)
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
            default:
                break;

            }
        }

        if (starterAssetsInputs.switchWeapon && Time.time - lastSwitchTime >= switchCoolDown)
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

        if (starterAssetsInputs.blaster)
        {
            EquipBlaster();
        }

        if (starterAssetsInputs.blackHoleGun)
        {
            EquipBlackHoleGun();
        }

        if (starterAssetsInputs.shotgun)
        {
            EquipShotgun();
        }

        if(starterAssetsInputs.reload)
        {
            Reload();
        }

        if (starterAssetsInputs.shoot)
        {
            Vector3 aimDir = (mouseWorldPosition - spawnBulletPosition.position).normalized;

            if (scnScr.Scan == false && shotCooldown >= currentCooldown && !reloading)
            {
                //play shoot animation
                animator.SetTrigger("Shoot");

                if (equippedWeapon == 0 && standardAmmoLoaded > 0)//Standard Projectile Shoot
                {
                    Instantiate(pfBulletProjectile, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
                    standardAmmoLoaded -= 1;
                    currentCooldown = standardCooldown;
                    thirdPersonController.SwitchCameraTarget();
                    AudioSource.PlayClipAtPoint(blasterSound, spawnBulletPosition.position);
                    blassterFlash.Play();
                }
                else if (equippedWeapon == 1 )
                {
                    if (starterAssetsInputs.swapBHG  )
                    {
                        if (blackHoleAmmoLoaded > 0 || blackHoleAmmoLoaded <= 0)
                        {
                        
                         BhgIcon.SetActive(false);
                        Instantiate(pfWallProjectile, spawnBlackHoleBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
                        currentCooldown = blackHoleCooldown;
                        thirdPersonController.SwitchCameraTarget();
                        AudioSource.PlayClipAtPoint(wallBulletSound, spawnBlackHoleBulletPosition.position);
                        }
                    }
                    else if (blackHoleAmmoLoaded > 0 )
                    {
                       // BhgIcon.SetActive(true);
                        // Black Hole Projectile Shoot
                        if (isCharged)
                        {

                            Instantiate(pfBlackHoleProjectile, spawnBlackHoleBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
                            blackHoleAmmoLoaded -= 1;
                            currentCooldown = blackHoleCooldown;
                            thirdPersonController.SwitchCameraTarget();
                            AudioSource.PlayClipAtPoint(blackHoleSound, spawnBlackHoleBulletPosition.position);
                            BHGfiring();
                            chargeTime = 0;
                            isCharged = false;
                        }
                        else
                        {
                            AudioSource.PlayClipAtPoint(blackHoleChargeSound, spawnBlackHoleBulletPosition.position);
                            BHGcharging();
                        }

                    }


                }
                else if (equippedWeapon == 2 && shotgunAmmoLoaded > 0)
                {        
                    shotgunAmmoLoaded -= 1;
                    currentCooldown = shotgunCooldown;
                    thirdPersonController.SwitchCameraTarget();
                    AudioSource.PlayClipAtPoint(shotgunSound, spawnShotgunBulletPosition.position);
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

        //
        shotCooldown += Time.deltaTime;
        if (shotCooldown >= currentCooldown)
            animator.ResetTrigger("Shoot");

        if (isCharging == true)
        {
            chargeTime += Time.unscaledDeltaTime * chargeSpeed;
        }

        if (chargeTime >= 3f)
        {
            isCharged = true;
            isCharging = false;
        }

    if (starterAssetsInputs.scan)
            {
                playermesh.enabled = false;
                starterAssetsInputs.scan = true;

                scnScr.ScanCamPriority();
                
                if (starterAssetsInputs.scan == true)
                {
                    starterAssetsInputs.scan = false;

                }
                if (scnScr.Scan == false)
                {
                    playermesh.enabled = true;
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

            if (standardAmmoLoaded == 0 || shotgunAmmoLoaded == 0 || blackHoleAmmoLoaded == 0)
            {
                Reload();
            }
        }

    public void EquipBlaster()
    {
        animator.SetTrigger("BlasterSwitch");
        animator.ResetTrigger("BHSwitch");
        animator.ResetTrigger("ShotgunSwitch");
        if (starterAssetsInputs.aim)
        {
            //animator.SetTrigger("aimGun");
            standardWeaponObject.SetActive(true);
            blackHoleWeaponObject.SetActive(false);
            shotgunWeaponObject.SetActive(false);
        }
        else if (!starterAssetsInputs.aim)
        {
            animator.ResetTrigger("aimGun");
        }
        equippedWeapon = 0;
        shotCooldown = currentCooldown;
        RefreshWeaponIcons();
        UpdateAmmoCount();
        Debug.Log(equippedWeapon);
    }
    public void EquipShotgun()
    {
        animator.SetTrigger("ShotgunSwitch");
        animator.ResetTrigger("BlasterSwitch");
        animator.ResetTrigger("BHSwitch");
        if (starterAssetsInputs.aim)
        {
            animator.SetTrigger("aimGun");
            standardWeaponObject.SetActive(false);
            blackHoleWeaponObject.SetActive(false);
            shotgunWeaponObject.SetActive(true);
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
        if (starterAssetsInputs.aim)
        {
            animator.SetTrigger("aimGun");
            standardWeaponObject.SetActive(false);
            blackHoleWeaponObject.SetActive(false);
            shotgunWeaponObject.SetActive(true);
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
                break;
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

        private void addDevAmmo()
        {
            int ammoToAdd = 50;

            switch (equippedWeapon)
            {
                case 0:
                    standardAmmoLoaded = ammoToAdd;
                    break;
                case 1:
                    blackHoleAmmoLoaded = ammoToAdd;
                    break;
                case 2:
                    shotgunAmmoLoaded = ammoToAdd;
                    break;
            }

            UpdateAmmoCount();
            Debug.Log("Ammo added: " + ammoToAdd);
        }

        public void UpdateAmmoCount()
        {
            if (equippedWeapon == 0)
            {
                totalAmmoCounter.text = standardAmmo.ToString();
                loadedAmmoCounter.text = standardAmmoLoaded.ToString();
            }
            else if (equippedWeapon == 1)
            {
                totalAmmoCounter.text =  blackHoleAmmo.ToString();
                loadedAmmoCounter.text = blackHoleAmmoLoaded.ToString();
            }
            else if (equippedWeapon == 2)
            {
                totalAmmoCounter.text = shotgunAmmo.ToString();
                loadedAmmoCounter.text = shotgunAmmoLoaded.ToString();
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
           /* if (newWeaponObject == originalWeaponObject)
            {
                spawnBulletPosition = spawnBulletPositionOg; // Use the crouch bullet position
            }
            else if (newWeaponObject == crouchedWeaponObject)
            {
                spawnBulletPosition = spawnBulletPositionCrouch; // Use the crouch bullet position
            }*/

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
        newParticleSystem.GetComponent<BHG>().tpsc = this;
        newParticleSystem.Play();

        yield return new WaitForSeconds(duration);

        Destroy(newParticleSystem.gameObject);
    }

    private void Reload()
    {
        if (standardAmmoLoaded != standardAmmoMax || blackHoleAmmoLoaded != blackHoleAmmoMax || shotgunAmmoLoaded != shotgunAmmoMax)
        {
        reloading = true;

        Debug.Log("Reloading!");
        if(equippedWeapon == 0 && standardAmmo > 0 && standardAmmoLoaded < standardAmmoMax)
        {
            StartCoroutine(ReloadTimer(standardReloadTime));
            AudioSource.PlayClipAtPoint(blasterReloadSound, spawnBulletPosition.position);
            ammoDifference = standardAmmoMax - standardAmmoLoaded;
            standardAmmoLoaded += standardAmmo;
            standardAmmo -= ammoDifference;
            if (standardAmmo < 0)
            {
                standardAmmo = 0;
            }
            else
            {
                standardAmmoLoaded = standardAmmoMax;
            }
        }
        else if(equippedWeapon == 1 && blackHoleAmmo > 0 && blackHoleAmmoLoaded < blackHoleAmmoMax)
        {
            StartCoroutine(ReloadTimer(blackHoleReloadTime));
            AudioSource.PlayClipAtPoint(blackHoleReloadSound, spawnBlackHoleBulletPosition.position);
            ammoDifference = blackHoleAmmoMax - blackHoleAmmoLoaded;
            blackHoleAmmoLoaded += blackHoleAmmo;
            blackHoleAmmo -= ammoDifference;
            if (standardAmmo < 0)
            {
                blackHoleAmmo = 0;
            }
            else
            {
                blackHoleAmmoLoaded = blackHoleAmmoMax;
            }
        }
        else if(equippedWeapon == 2 && shotgunAmmo > 0 && shotgunAmmoLoaded < shotgunAmmoMax)
        {
            StartCoroutine(ReloadTimer(shotgunReloadTime));
            AudioSource.PlayClipAtPoint(shotgunReloadSound, spawnShotgunBulletPosition.position);
            ammoDifference = shotgunAmmoMax - shotgunAmmoLoaded;
            shotgunAmmoLoaded += shotgunAmmo;
            shotgunAmmo -= ammoDifference;
            if (shotgunAmmo < 0)
            {
                shotgunAmmo = 0;
            }
            else
            {
                shotgunAmmoLoaded = shotgunAmmoMax;
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
        if((starterAssetsInputs.swapBHG &&equippedWeapon == 1 ))
         {
             BhgIcon.SetActive(false);
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
}