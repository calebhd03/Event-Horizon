using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyLister : MonoBehaviour 
{
    public EnemyData enemyData;
    public HealthMetrics healthMetrics;
    public int enemyType; // Add a variable to hold the enemy type
    [SerializeField] public int listIndex;
    public int sceneIndex; // Change 'scene' to 'sceneIndex'
    public bool wasDeadInLastSave = false; // Variable to track if the enemy was dead in the last save

    void OnEnable()
    {
        healthMetrics = GetComponent<HealthMetrics>();
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        Vector3 lastSavedPosition = transform.position; // Get the last saved position
        List<GameObject> enemies = new List<GameObject>(); // Create a list to hold the enemies
        enemies.Add(gameObject); // Add this enemy to the list
        enemyData.SaveEnemyData(sceneIndex, enemies); // Use 'SaveEnemyData' method
    }

    void OnDestroy()
    {
        // Save if the enemy was dead in the last save
        string deadKey = "Scene" + sceneIndex + "EnemyWasDead" + listIndex;
        PlayerPrefs.SetInt(deadKey, wasDeadInLastSave ? 1 : 0);
        
        if (!wasDeadInLastSave)
        {
            enemyData.enemiesData[sceneIndex].RemoveAll(data => data.position == transform.position); // Remove the enemy data from the list
        }
    }

    // Example to display current health
    void Update()
    {
        // Check if the enemy is dead and update wasDeadInLastSave accordingly
        if (!wasDeadInLastSave && healthMetrics.currentHealth <= 0)
        {
            wasDeadInLastSave = true;
        }

        // Access current health from the EnemyVariables object in enemyData
        if (enemyData.enemiesData.ContainsKey(sceneIndex))
        {
            foreach (var enemy in enemyData.enemiesData[sceneIndex])
            {
                // Debug.Log("Current health: " + enemy.health);
            }
        }
    }
}