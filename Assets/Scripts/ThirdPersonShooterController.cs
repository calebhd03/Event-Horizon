using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.InputSystem;

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
       
    }

    private void Update()
    {
        Vector3 mouseWorldPosition = Vector3.zero;

        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            mouseWorldPosition = raycastHit.point;
        }

        if (starterAssetsInputs.aim)
        {
            aimVirtualCamera.gameObject.SetActive(true);
            thirdPersonController.SetSensitivity(aimSensitivity);
            thirdPersonController.SetRotateOnMove(false);
        }
        else
        {
            aimVirtualCamera.gameObject.SetActive(false);
            thirdPersonController.SetSensitivity(normalSensitivity);
            thirdPersonController.SetRotateOnMove(true);
        }

        if (starterAssetsInputs.scroll != Vector2.zero)
        {
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
        }

        if (starterAssetsInputs.shoot)
        {
            if (equippedWeapon == 0) // Standard Projectile Shoot
            {
                Vector3 direction = (mouseWorldPosition - spawnBulletPosition.position).normalized;
                Instantiate(pfBulletProjectile, spawnBulletPosition.position, Quaternion.LookRotation(direction, Vector3.up));

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

    }

    
} 