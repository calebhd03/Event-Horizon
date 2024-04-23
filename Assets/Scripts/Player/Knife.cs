using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    PlayerHealthMetric playerHealthMetric;
    public MeshRenderer weaponMesh;
    void Awake()
    {
        playerHealthMetric = GetComponentInParent<PlayerHealthMetric>();
        EnableMesh();
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
