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
    [SerializeField] private float shotgunCooldown = 3.0f;
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
    Vector3 aimDir = Vector3.zero;

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

        Vector3 worldAimTarget = mouseWorldPosition;
        worldAimTarget.y = transform.position.y;
        aimDir = (worldAimTarget - transform.position).normalized; // Assign value to aimDir
        transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * 20f);
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
        }
        else if (equippedWeapon == 1) // Black Hole Projectile Shoot
        {
            Vector3 direction = (mouseWorldPosition - spawnBulletPosition.position).normalized;
            Instantiate(pfBlackHoleProjectile, spawnBulletPosition.position, Quaternion.LookRotation(direction, Vector3.up));
        }
        else if (equippedWeapon == 2) // Shotgun Projectile Shoot
        {
            if (Time.time - lastShotgunTime >= shotgunCooldown) // Check cooldown
            {
                for (int i = 0; i < 4; i++) // Fire 4 pellets in a cone
                {
                    Vector3 spreadDirection = Quaternion.Euler(0, Random.Range(-15f, 15f), 0) * aimDir;
                    Instantiate(pfShotgunProjectile, spawnBulletPosition.position, Quaternion.LookRotation(spreadDirection, Vector3.up));
                }
                lastShotgunTime = Time.time;
            }
        }
        starterAssetsInputs.shoot = false;
    }
}
}
