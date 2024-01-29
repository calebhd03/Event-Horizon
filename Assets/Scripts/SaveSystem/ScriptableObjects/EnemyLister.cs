using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLister : MonoBehaviour
{
    void OnEnable()
    {
        EnemyData.Add(this);
    }
    
    void OnDestroy()
    {
        EnemyData.Remove(this);
    }
}
