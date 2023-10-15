using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.InputSystem;
using TMPro;
using UnityEditor.Callbacks;
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

    private ThirdPersonController thirdPersonController;
    private StarterAssetsInputs starterAssetsInputs;
    
    [Header("Weapon Settings")]
    public int[] allWeapons = new int[]{0, 1, 2};
    private int equippedWeapon;

    private float shotCooldown;
    private float currentCooldown;
    public float standardCooldown;
    public float blackHoleCooldown;
    public float shotgunCooldown;
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
    

    [SerializeField] private int equippedWeapon;
    [SerializeField] private float shotgunCooldown = 1.0f;
    [SerializeField] private float shotgunSpreadAngle = 3f; // Spread angle for shotgun pellets
    private float lastShotgunTime;
    private ThirdPersonController thirdPersonController;
    private StarterAssetsInputs starterAssetsInputs;
    

    private void Awake()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        UpdateAmmoCount();
        currentCooldown = standardCooldown;
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
            mouseWorldPosition = raycastHit.point;
        }


        if(starterAssetsInputs.aim)
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

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime *20f);
        } 
        else 
        {
            aimVirtualCamera.gameObject.SetActive(false);
            thirdPersonController.SetSensitivity(normalSensitivity);
            thirdPersonController.SetRotateOnMove(true);
        }

        if (starterAssetsInputs.scroll != Vector2.zero)
        {
            equippedWeapon = starterAssetsInputs.scroll.y > 0 ? equippedWeapon -= 1 : equippedWeapon += 1;
            if (equippedWeapon > allWeapons.Length - 1)
            {
                equippedWeapon = 0;
            if (equippedWeapon == 0)
            {
                equippedWeapon = 1;
                Debug.Log("Black Hole Gun Equipped");
            }
            else if (equippedWeapon == 1)
            {
                equippedWeapon = 2;
                Debug.Log("Shotgun Equipped");
            }
            else if (equippedWeapon == 2)
            {
                equippedWeapon = 0;
                Debug.Log("Standard Gun Equipped");
            }
            else if (equippedWeapon < 0)
            {
                equippedWeapon = allWeapons.Length - 1;
            }
            shotCooldown = currentCooldown;
            UpdateAmmoCount();
            Debug.Log(equippedWeapon);
        }

        if (starterAssetsInputs.shoot)
        {
            if (equippedWeapon == 0) // Standard Projectile Shoot
            {
                Vector3 direction = (mouseWorldPosition - spawnBulletPosition.position).normalized;
                Instantiate(pfBulletProjectile, spawnBulletPosition.position, Quaternion.LookRotation(direction, Vector3.up));

            if (scnScr.Scan == false && shotCooldown >= currentCooldown)
            {
                if(equippedWeapon == 0 && standardAmmo > 0)//Standard Projectile Shoot
                {
                    Instantiate(pfBulletProjectile, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
                    standardAmmo -= 1;
                    currentCooldown = standardCooldown;
                    UpdateAmmoCount();
                }
                else if (equippedWeapon == 1 && blackHoleAmmo > 0)//Black Hole Projectile Shoot
                {
                    Instantiate(pfBlackHoleProjectile, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
                    blackHoleAmmo -= 1;
                    currentCooldown = blackHoleCooldown;
                    UpdateAmmoCount();
                }
                else if (equippedWeapon == 2 && shotgunAmmo > 0)
                {
                    shotgunAmmo -= 1;
                    currentCooldown = shotgunCooldown;
                    UpdateAmmoCount();
                }
                shotCooldown = 0;
                          thirdPersonController.Recoil(0.1f);
                
            }
            else if (equippedWeapon == 1) // Black Hole Projectile Shoot
            {
                Vector3 direction = (mouseWorldPosition - spawnBulletPosition.position).normalized;
                Instantiate(pfBlackHoleProjectile, spawnBulletPosition.position, Quaternion.LookRotation(direction, Vector3.up));
            }
            else if (equippedWeapon == 2) // Shotgun Projectile Shoot
            {
               if (equippedWeapon == 2) // Shotgun Projectile Shoot
            {
                if (Time.time - lastShotgunTime >= shotgunCooldown) // Check cooldown
                {
                    thirdPersonController.Recoil(0.2f);
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
                    lastShotgunTime = Time.time;
                }
            }
            }
            starterAssetsInputs.shoot = false;
        }

        if(shotCooldown <= currentCooldown)
        {
            cooldownMeter.transform.localScale = new Vector3 ((shotCooldown / currentCooldown) * 0.96f, .8f, 1);
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
        else{
            scnCam.DisableScript();
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
        if(equippedWeapon == 0)
        {
            ammoCounter.text = "Ammo: " + standardAmmo;
        }
        else if(equippedWeapon == 1)
        {
            ammoCounter.text = "Ammo: " + blackHoleAmmo;
        }
        else if(equippedWeapon == 2)
        {
            ammoCounter.text = "Ammo: " + shotgunAmmo;
        }
    }

    
} 
    }
