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
        enemyData.Add(scene, gameObject, healthMetrics, enemyType); // Pass enemyType when adding to EnemyData
    }
    
    void OnDestroy()
    {
        enemyData.Remove(scene, gameObject);
    }
}