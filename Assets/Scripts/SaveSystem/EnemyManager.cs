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
        Debug.Log("Enemy positions initialized and saved.");
    }

    public void SaveEnemyLocations(int sceneIndex)
    {
        Debug.Log("Save Enemy Locations");

        // Find all objects with the "EnemyLister" script
        EnemyLister[] enemies = FindObjectsOfType<EnemyLister>();

        Debug.Log("Number of enemies to save: " + enemies.Length);

        // Save each enemy's position individually
        for (int i = 0; i < enemies.Length; i++)
        {
            string positionKey = "Scene" + sceneIndex + "EnemyPosition" + i;
            PlayerPrefs.SetString(positionKey, enemies[i].transform.position.x + "," + enemies[i].transform.position.y + "," + enemies[i].transform.position.z);
        }

        // Save the number of enemies for reference
        PlayerPrefs.SetInt("Scene" + sceneIndex + "NumEnemies", enemies.Length);

        // Flag to indicate that the data has been saved
        PlayerPrefs.SetInt("Scene" + sceneIndex + "HasBeenPlayed", 1);

        Debug.Log("Enemy locations saved successfully.");
    }

    public void LoadEnemyLocations(int sceneIndex)
    {

        Debug.Log("Load Enemy Locations");

        // Check if the number of enemies has been saved
        if (PlayerPrefs.HasKey("Scene" + sceneIndex + "NumEnemies"))
        {
            int numEnemies = PlayerPrefs.GetInt("Scene" + sceneIndex + "NumEnemies");

            // Load each enemy's position individually
            for (int i = 0; i < numEnemies; i++)
            {
                string positionKey = "Scene" + sceneIndex + "EnemyPosition" + i;
                if (PlayerPrefs.HasKey(positionKey))
                {
                    string[] positionString = PlayerPrefs.GetString(positionKey).Split(',');
                    if (positionString.Length == 3)
                    {
                        float x = float.Parse(positionString[0]);
                        float y = float.Parse(positionString[1]);
                        float z = float.Parse(positionString[2]);
                        Vector3 position = new Vector3(x, y, z);

                        Debug.Log("Retrieved enemy position: " + position); // Log the retrieved position

                        // Find the parent objects with the "Enemy" tag
                        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");
                        foreach (GameObject enemyObject in enemyObjects)
                        {
                            // Set the position of both parent and child GameObjects
                            Transform[] allChildren = enemyObject.GetComponentsInChildren<Transform>();
                            foreach (Transform child in allChildren)
                            {
                                UnityEngine.AI.NavMeshAgent navMeshAgent = child.GetComponent<UnityEngine.AI.NavMeshAgent>();
                                if (navMeshAgent != null)
                                {
                                    // Set the destination of the NavMeshAgent
                                    navMeshAgent.Warp(position);
                                }
                            }
                        }
                    }
                    else
                    {
                        Debug.Log("Invalid position format for enemy position key: " + positionKey);
                    }
                }
                else
                {
                    Debug.Log("Position key not found: " + positionKey);
                }
            }

            Debug.Log("Enemy locations loaded successfully.");
        }
        else
        {
            Debug.Log("No saved enemy positions found for scene " + sceneIndex);
        }
    }

    public void SetEnemyData(List<EnemySaveData> enemyData)
    {
        Debug.Log("Set enemy data called");
        enemyDataList = enemyData;
    }
}