using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.InputSystem;
using TMPro;

public class ThirdPersonShooterController : MonoBehaviour 
{

    [SerializeField] public CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform debugTransform;
    [SerializeField] private Transform pfBulletProjectile;
    [SerializeField] private Transform spawnBulletPosition;

    private ThirdPersonController thirdPersonController;
    private StarterAssetsInputs starterAssetsInputs;


    [Header("Scanner Necessities")]
    public GameObject Scanningobject;
    public GameObject Scannercamera;
    public GameObject ScannerZoomCamera;
    public bool Scanenabled = false;
    


    private void Awake()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();

    }

    private void Update()
    {
        Scanning scnScr = Scanningobject.GetComponent<Scanning>();
        ScanCam scnCam = Scannercamera.GetComponent<ScanCam>();

        Vector3 mouseWorldPosition = Vector3.zero;

        Vector2 screenCenterPoint = new Vector2(Screen.width /2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if(Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            debugTransform.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
        }


        if(starterAssetsInputs.aim){


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
        } else {
            aimVirtualCamera.gameObject.SetActive(false);
            thirdPersonController.SetSensitivity(normalSensitivity);
            thirdPersonController.SetRotateOnMove(true);
        }

        if (starterAssetsInputs.shoot)
        {
             // Projectile Shoot
            Vector3 aimDir = (mouseWorldPosition - spawnBulletPosition.position).normalized;

            if (scnScr.Scan == false)
            {
            Instantiate(pfBulletProjectile, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
            }
            starterAssetsInputs.shoot = false;
        }
        
        if (starterAssetsInputs.scan)
        {
            ThirdPersonController TPC = GetComponent<ThirdPersonController>();
            TPC.MoveSpeed = 0;
            starterAssetsInputs.scan = true;

            scnScr.ScanCamPriority();
            
            if (starterAssetsInputs.scan == true)
            {
                starterAssetsInputs.scan = false;

            }
            if (scnScr.Scan == false)
            {
                    TPC.MoveSpeed = TPC.NormalMovespeed;
            }
        }

        if (starterAssetsInputs.scanobj)
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
            ScanZoom scnzCam = ScannerZoomCamera.GetComponent<ScanZoom>();
            scnzCam.ScanZoomPriority();

            if (starterAssetsInputs.scanaim == true)
            {
                starterAssetsInputs.scanaim = false;
            }
        }
    }
    
}
