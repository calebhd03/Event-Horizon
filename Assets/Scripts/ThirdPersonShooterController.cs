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

    [SerializeField] public CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform debugTransform;
    [SerializeField] private Transform pfBulletProjectile;
    [SerializeField] private Transform pfBlackHoleProjectile;
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

        Vector2 screenCenterPoint = new Vector2(Screen.width /2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if(Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            debugTransform.position = raycastHit.point;
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
            Vector3 aimDir = (mouseWorldPosition - spawnBulletPosition.position).normalized;

            if (scnScr.Scan == false && shotCooldown >= currentCooldown)
            {
                if(equippedWeapon == 0 && standardAmmo > 0)//Standard Projectile Shoot
                {
                    Instantiate(pfBulletProjectile, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
                    standardAmmo -= 1;
                    currentCooldown = standardCooldown;
                }
                else if (equippedWeapon == 1 && blackHoleAmmo > 0)//Black Hole Projectile Shoot
                {
                    Instantiate(pfBlackHoleProjectile, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
                    blackHoleAmmo -= 1;
                    currentCooldown = blackHoleCooldown;
                }
                else if (equippedWeapon == 2 && shotgunAmmo > 0)
                {
                    //insert shotgun effect
                    shotgunAmmo -= 1;
                    currentCooldown = shotgunCooldown;
                }
                UpdateAmmoCount();
                shotCooldown = 0;
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
