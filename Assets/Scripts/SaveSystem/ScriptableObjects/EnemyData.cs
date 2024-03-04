using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class EnemySaveData
{
    public int enemyType;
    public Vector3 position;
    public float health;

    public EnemySaveData(int enemyType, Vector3 position, float health)
    {
        this.enemyType = enemyType;
        this.position = position;
        this.health = health;
    }
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EnemyDataScriptableObject", order = 1)]
public class EnemyData : ScriptableObject
{
    public Dictionary<int, List<EnemySaveData>> enemiesData = new Dictionary<int, List<EnemySaveData>>();

    public void SaveEnemyData(int sceneIndex, List<GameObject> enemies)
    {
        List<EnemySaveData> saveData = new List<EnemySaveData>();

        foreach (GameObject enemy in enemies)
        {
            // Retrieve necessary data from the enemy GameObject
            // For demonstration, let's assume all enemies have the same type and health
            int enemyType = 1; // Set a default enemy type
            Vector3 position = enemy.transform.position;
            float health = 100f; // Set a default health value

            saveData.Add(new EnemySaveData(enemyType, position, health));
        }

        enemiesData[sceneIndex] = saveData;
    }

    public void LoadEnemyData(int sceneIndex)
    {
        if (enemiesData.ContainsKey(sceneIndex))
        {
            foreach (EnemySaveData enemyData in enemiesData[sceneIndex])
            {
                // Recreate enemy GameObject using saved data
                // For demonstration, we won't create actual enemy objects here
                Debug.Log("Enemy Type: " + enemyData.enemyType + ", Position: " + enemyData.position + ", Health: " + enemyData.health);
            }
        }
        else
        {
            Debug.LogWarning("No saved enemy data found for scene " + sceneIndex);
        }
    }
}