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
        // For example, you can call the SaveEnemyData method passing the scene index and list of enemy GameObjects
        int sceneIndex = 0; // Change this to the appropriate scene index
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        enemyData.SaveEnemyData(sceneIndex, new List<GameObject>(enemies));
    }
}
