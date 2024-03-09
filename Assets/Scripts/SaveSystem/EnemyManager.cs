
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
        // Find all parent objects with the "Enemy" tag
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        List<GameObject> validEnemies = new List<GameObject>();

        // Filter out child objects and store only parent objects
        foreach (GameObject enemy in enemies)
        {
            if (enemy.transform.parent == null || !enemy.transform.parent.CompareTag("Enemy"))
            {
                validEnemies.Add(enemy);
            }
        }

        // Clear previous data
        initialEnemyPositions.Clear();
        enemyDataList.Clear();

        // Save the initial positions into the list
        foreach (GameObject enemy in validEnemies)
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

        // Find all parent objects with the "Enemy" tag
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        List<GameObject> validEnemies = new List<GameObject>();

        // Filter out child objects and store only parent objects
        foreach (GameObject enemy in enemies)
        {
            if (enemy.transform.parent == null || !enemy.transform.parent.CompareTag("Enemy"))
            {
                validEnemies.Add(enemy);
            }
        }

        // Make sure the number of initial positions matches the number of valid enemies
        if (initialPositions.Length == validEnemies.Count)
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

            // Find all parent objects with the "Enemy" tag
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            List<GameObject> validEnemies = new List<GameObject>();

            // Filter out child objects and store only parent objects
            foreach (GameObject enemy in enemies)
            {
                if (enemy.transform.parent == null || !enemy.transform.parent.CompareTag("Enemy"))
                {
                    validEnemies.Add(enemy);
                }
            }

            // Make sure the number of saved positions matches the number of valid enemies
            if (enemyPositions.Length == validEnemies.Count)
            {
                for (int i = 0; i < validEnemies.Count; i++)
                {
                    // Set each valid enemy's position to its corresponding saved position
                    validEnemies[i].transform.position = enemyPositions[i];
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