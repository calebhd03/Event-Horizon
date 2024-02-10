using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class NexusGun : MonoBehaviour
{
    public MeshRenderer weaponMesh;
    public StarterAssetsInputs starterAssetsInputs;
    public ThirdPersonShooterController TPSC;
    void Start()
    {
        weaponMesh = GetComponentInChildren<MeshRenderer>();
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
