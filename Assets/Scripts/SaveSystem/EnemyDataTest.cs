using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDataTest : MonoBehaviour
{
    public EnemyData enemyData;

    private void Start()
    {
        // Create an instance of EnemyData
        enemyData = ScriptableObject.CreateInstance<EnemyData>();

        // Now you can access the functionality and data of enemyData as needed
        // For example:
        enemyData.Add(0, gameObject, null, 0, Vector3.zero);
    }
}
