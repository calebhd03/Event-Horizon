
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    // List of all enemy prefabs, indexed by their type
    public GameObject[] enemyPrefabs;
    private List<EnemySaveData> enemyDataList;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveEnemyLocations(int sceneIndex)
    {
        Debug.LogWarning("SaveEnemyLoc");
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Vector3[] enemyPositions = new Vector3[enemies.Length];

        for (int i = 0; i < enemies.Length; i++)
        {
            enemyPositions[i] = enemies[i].transform.position;
        }

        // Save enemy positions as JSON
        string json = JsonUtility.ToJson(enemyPositions);
        PlayerPrefs.SetString("Scene" + sceneIndex + "EnemyLocations", json);
        PlayerPrefs.SetInt("Scene" + sceneIndex + "HasBeenPlayed", 1);
    }

    public void LoadEnemyLocations(int sceneIndex)
    {
        Debug.LogWarning("LoadEnemyLoc");
        // Check if enemy locations have been saved
        if (PlayerPrefs.HasKey("Scene" + sceneIndex + "EnemyLocations"))
        {
            // Retrieve saved enemy positions JSON
            string json = PlayerPrefs.GetString("Scene" + sceneIndex + "EnemyLocations");
            Vector3[] enemyPositions = JsonUtility.FromJson<Vector3[]>(json);

            // Find all enemies with the "Enemy" tag
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            // Make sure the number of saved positions matches the number of enemies
            if (enemyPositions.Length == enemies.Length)
            {
                for (int i = 0; i < enemies.Length; i++)
                {
                    // Set each enemy's position to its corresponding saved position
                    enemies[i].transform.position = enemyPositions[i];
                }
            }
            else
            {
                Debug.LogWarning("Number of saved enemy positions does not match the number of enemies in the scene.");
            }
        }
        else
        {
            Debug.LogWarning("No saved enemy locations found for scene " + sceneIndex);
        }
    }

    public void SetEnemyData(List<EnemySaveData> enemyData)
    {
        Debug.LogWarning("Set enemy data called");
        enemyDataList = enemyData;
    }
}