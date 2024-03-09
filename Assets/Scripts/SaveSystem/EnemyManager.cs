
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    // List of all enemy prefabs, indexed by their type
    public GameObject[] enemyPrefabs;
    private List<EnemySaveData> enemyDataList = new List<EnemySaveData>();
    private List<Vector3> initialEnemyPositions = new List<Vector3>();

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

    private void Start()
    {
        // Pass the scene index when initializing enemy positions
        InitializeEnemyPositions(SceneManager.GetActiveScene().buildIndex);
    }

    private void InitializeEnemyPositions(int sceneIndex)
    {
        // Find all objects with the "EnemyLister" script
        EnemyLister[] enemies = FindObjectsOfType<EnemyLister>();

        // Clear previous data
        initialEnemyPositions.Clear();
        enemyDataList.Clear();

        // Save the initial positions into the list
        foreach (EnemyLister enemy in enemies)
        {
            initialEnemyPositions.Add(enemy.transform.position);
            float health = enemy.GetComponent<HealthMetrics>().currentHealth;
            enemyDataList.Add(new EnemySaveData(0, enemy.transform.position, health)); // Assuming enemy type is 0
        }

        // Convert the list to JSON and save it to PlayerPrefs
        string json = JsonUtility.ToJson(initialEnemyPositions.ToArray());
        PlayerPrefs.SetString("Scene" + sceneIndex + "EnemyPositions", json);
        PlayerPrefs.SetInt("Scene" + sceneIndex + "HasBeenPlayed", 1);
    }

    public void SaveEnemyLocations(int sceneIndex)
    {
        // Retrieve the initial positions JSON from PlayerPrefs
        string json = PlayerPrefs.GetString("Scene" + sceneIndex + "EnemyPositions");
        Vector3[] initialPositions = JsonUtility.FromJson<Vector3[]>(json);

        // Find all objects with the "EnemyLister" script
        EnemyLister[] enemies = FindObjectsOfType<EnemyLister>();

        // Make sure the number of initial positions matches the number of valid enemies
        if (initialPositions.Length == enemies.Length)
        {
            // Save enemy positions as JSON
            string positionsJson = JsonUtility.ToJson(initialPositions);
            PlayerPrefs.SetString("Scene" + sceneIndex + "EnemyPositions", positionsJson);
            PlayerPrefs.SetInt("Scene" + sceneIndex + "HasBeenPlayed", 1);
        }
        else
        {
            Debug.LogWarning("Number of initial enemy positions does not match the number of valid enemies in the scene.");
        }
    }

    public void LoadEnemyLocations(int sceneIndex)
    {
        // Check if enemy locations have been saved
        if (PlayerPrefs.HasKey("Scene" + sceneIndex + "EnemyPositions"))
        {
            // Retrieve saved enemy positions JSON
            string json = PlayerPrefs.GetString("Scene" + sceneIndex + "EnemyPositions");
            Vector3[] enemyPositions = JsonUtility.FromJson<Vector3[]>(json);

            // Find all objects with the "EnemyLister" script
            EnemyLister[] enemies = FindObjectsOfType<EnemyLister>();

            // Make sure the number of saved positions matches the number of valid enemies
            if (enemyPositions.Length == enemies.Length)
            {
                for (int i = 0; i < enemies.Length; i++)
                {
                    // Set each valid enemy's position to its corresponding saved position
                    enemies[i].transform.position = enemyPositions[i];
                }
            }
            else
            {
                Debug.LogWarning("Number of saved enemy positions does not match the number of valid enemies in the scene.");
            }
        }
        else
        {
            Debug.LogWarning("No saved enemy positions found for scene " + sceneIndex);
        }
    }

    public void SetEnemyData(List<EnemySaveData> enemyData)
    {
        Debug.LogWarning("Set enemy data called");
        enemyDataList = enemyData;
    }
}