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
        Debug.LogWarning("Enemy positions initialized and saved.");
    }

    public void LoadEnemyLocations(int sceneIndex)
    {
        Debug.LogWarning("Load Enemy Locations");

        // Check if the number of enemies has been saved
        if (PlayerPrefs.HasKey("Scene" + sceneIndex + "EnemyPositions"))
        {
            string json = PlayerPrefs.GetString("Scene" + sceneIndex + "EnemyPositions");
            Vector3[] savedPositions = JsonUtility.FromJson<Vector3[]>(json);

            // Find all objects with the "EnemyLister" script
            EnemyLister[] enemies = FindObjectsOfType<EnemyLister>();

            // Iterate through enemies and set them to their saved positions
            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i].transform.position = savedPositions[i];
                UnityEngine.AI.NavMeshAgent navMeshAgent = enemies[i].GetComponent<UnityEngine.AI.NavMeshAgent>();
                if (navMeshAgent != null)
                {
                    // If the enemy has a NavMeshAgent, warp it to the saved position
                    navMeshAgent.Warp(savedPositions[i]);
                }
            }

            Debug.LogWarning("Enemy locations loaded successfully.");
        }
        else
        {
            Debug.LogWarning("No saved enemy positions found for scene " + sceneIndex);
        }
    }
}