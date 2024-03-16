using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavChange : MonoBehaviour
{
    [SerializeField] private NavMeshObstacle Hole;
    private void OnEnable()
    {
        Hole.enabled = true;
    }
}
