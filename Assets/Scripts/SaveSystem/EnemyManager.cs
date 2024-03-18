using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyManager : MonoBehaviour
{
   public static EnemyManager instance;
    public int index;

    // List of all enemy prefabs, indexed by their type
    public GameObject[] enemyPrefabs;
    private List<Vector3> initialEnemyPositions = new List<Vector3>();
    private List<GameObject> enemyGameObjects = new List<GameObject>();

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

        // Save enemy locations once on start
        SaveEnemyLocations(SceneManager.GetActiveScene().buildIndex);

        // Instantiate enemy objects at initial positions
       // InstantiateEnemyObjects();
    }

    private void InitializeEnemyPositions(int sceneIndex)
    {
        // Find all objects with the "EnemyLister" script
        EnemyLister[] enemies = FindObjectsOfType<EnemyLister>();

        // Clear previous data
        initialEnemyPositions.Clear();

        // Save the initial positions into the list
        foreach (EnemyLister enemy in enemies)
        {
            initialEnemyPositions.Add(enemy.transform.position);
        }
    }

    private void SaveEnemyLocations(int sceneIndex)
    {
        Debug.Log("Save Enemy Locations");

        // Save each enemy's position individually
        for (int i = 0; i < initialEnemyPositions.Count; i++)
        {
            string positionKey = "Scene" + sceneIndex + "EnemyPosition" + i;
            PlayerPrefs.SetString(positionKey, initialEnemyPositions[i].x + "," + initialEnemyPositions[i].y + "," + initialEnemyPositions[i].z);
        }

        // Save the number of enemies for reference
        PlayerPrefs.SetInt("Scene" + sceneIndex + "NumEnemies", initialEnemyPositions.Count);

        // Flag to indicate that the data has been saved
        PlayerPrefs.SetInt("Scene" + sceneIndex + "HasBeenPlayed", 1);

        Debug.Log("Enemy locations saved successfully.");
    }

    private void InstantiateEnemyObjects()
    {
        // Instantiate enemy game objects at initial positions
        for (int i = 0; i < initialEnemyPositions.Count; i++)
        {
            GameObject enemyPrefab = enemyPrefabs[i % enemyPrefabs.Length]; // Choose enemy prefab based on index
            GameObject enemyObject = Instantiate(enemyPrefab, initialEnemyPositions[i], Quaternion.identity);
            enemyGameObjects.Add(enemyObject);
        }
    }

    public void LoadEnemyLocations(int sceneIndex)
    {
        Debug.Log("Load Enemy Locations");

        // Check if the number of enemies has been saved
        if (PlayerPrefs.HasKey("Scene" + sceneIndex + "NumEnemies"))
        {
            int numEnemies = PlayerPrefs.GetInt("Scene" + sceneIndex + "NumEnemies");

            // Clear the list to remove references to previous enemy game objects
            enemyGameObjects.Clear();

            // Store references to the original enemy game objects
            EnemyLister[] originalEnemies = FindObjectsOfType<EnemyLister>();

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

                        // Find the prefab for the enemy
                        GameObject enemyPrefab = enemyPrefabs[i % enemyPrefabs.Length]; // Ensure looping through enemyPrefabs if fewer than numEnemies

                        // Instantiate the enemy prefab at the saved position
                        GameObject newEnemy = Instantiate(enemyPrefab, position, Quaternion.identity);

                        // Add the instantiated enemy to the list
                        enemyGameObjects.Add(newEnemy);
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

            // Destroy the original enemy game objects from the scene
            foreach (EnemyLister originalEnemy in originalEnemies)
            {
                Destroy(originalEnemy.gameObject);
            }

            Debug.Log("Enemy locations loaded successfully.");
        }
        else
        {
            Debug.Log("No saved enemy positions found for scene " + sceneIndex);
        }
    }
}