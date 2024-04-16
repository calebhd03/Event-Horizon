using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class NexusGun : MonoBehaviour
{
    public MeshRenderer weaponMesh;
    //public StarterAssetsInputs starterAssetsInputs;
    //public ThirdPersonShooterController TPSC;
    [SerializeField]PlayerHealthMetric playerHealthMetric;
    void Awake()
    {
        playerHealthMetric = GetComponentInParent<PlayerHealthMetric>();
        weaponMesh = GetComponentInChildren<MeshRenderer>();  
    }
    void Start()
    {    
            if(playerHealthMetric.playerData.hasNexus == false)
            {
                DisableMesh();
            }
            else
            {
                EnableMesh();
            }
    }

    public void EnableMesh()
    {
        weaponMesh.enabled = true;
    }
    public void DisableMesh()
    {
        weaponMesh.enabled = false;
    }
}
