using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blaster : MonoBehaviour
{
    PlayerHealthMetric playerHealthMetric;
    public MeshRenderer weaponMesh;
    void Awake()
    {
        playerHealthMetric = GetComponentInParent<PlayerHealthMetric>();
        weaponMesh = GetComponentInChildren<MeshRenderer>();
            if(playerHealthMetric.playerData.hasBlaster == false)
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
