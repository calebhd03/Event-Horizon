using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blaster : MonoBehaviour
{
    public MeshRenderer weaponMesh;
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
