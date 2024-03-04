using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyLister : MonoBehaviour 
{
    public EnemyData enemyData;
    public HealthMetrics healthMetrics;
    public int enemyType; // Add a variable to hold the enemy type
    [SerializeField] private int listIndex;
    public int scene;

    void OnEnable()
    {
        healthMetrics = GetComponent<HealthMetrics>();
        scene = SceneManager.GetActiveScene().buildIndex;
        Vector3 lastSavedPosition = transform.position; // Get the last saved position
        enemyData.Add(scene, gameObject, healthMetrics, enemyType, lastSavedPosition); // Pass lastSavedPosition when adding to EnemyData
    }
    
    void OnDestroy()
    {
        enemyData.Remove(scene, gameObject);
    }

    // Example to display current health
    void Update()
    {
        // Access current health from the EnemyVariables object in enemyData
        foreach (var enemy in enemyData.Enemies[scene])
        {
           // Debug.Log("Current health: " + enemy.currentHealth);
        }
    }
}