
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
    Debug.Log("SaveEnemyLoc");

        // Find all enemies with the "Enemy" tag
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // Filter out child objects
        List<GameObject> validEnemies = new List<GameObject>();
        foreach (GameObject enemy in enemies)
        {
            if (enemy.transform.parent == null || enemy.transform.parent.tag != "Enemy")
            {
                // Add only if it's not a child object
                validEnemies.Add(enemy);
            }
        }

        // Log the count of enemies found in the scene after filtering
        Debug.Log("Number of enemies found in the scene: " + validEnemies.Count);

        // Store the positions of enemies
        Vector3[] enemyPositions = new Vector3[validEnemies.Count];
        for (int i = 0; i < validEnemies.Count; i++)
        {
            enemyPositions[i] = validEnemies[i].transform.position;
        }

        // Log the count of enemy positions being saved
        Debug.Log("Number of enemy positions being saved: " + enemyPositions.Length);

        // Save enemy positions as JSON
        string json = JsonUtility.ToJson(enemyPositions);
        PlayerPrefs.SetString("Scene" + sceneIndex + "EnemyLocations", json);
        PlayerPrefs.SetInt("Scene" + sceneIndex + "HasBeenPlayed", 1);
    }
    

    public void LoadEnemyLocations(int sceneIndex)
    {
        Debug.Log("Loading enemy locations...");

        // Check if enemy locations have been saved
        if (PlayerPrefs.HasKey("Scene" + sceneIndex + "EnemyLocations"))
        {
            // Retrieve saved enemy positions JSON
            string json = PlayerPrefs.GetString("Scene" + sceneIndex + "EnemyLocations");
            Vector3[] enemyPositions = JsonUtility.FromJson<Vector3[]>(json);

            // Find all enemies with the "Enemy" tag
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            // Log the number of saved enemy positions and the number of enemies in the scene
            Debug.Log("Number of saved enemy positions: " + enemyPositions.Length);
            Debug.Log("Number of enemies found in the scene before loading: " + enemies.Length);

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