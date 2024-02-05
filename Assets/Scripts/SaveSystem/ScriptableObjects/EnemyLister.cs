using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyLister : MonoBehaviour 
{
    public EnemyData enemyData;
    public HealthMetrics healthMetrics;
    [SerializeField]private int listIndex;

    void OnEnable()
    {
        healthMetrics = GetComponent<HealthMetrics>();
        int scene = SceneManager.GetActiveScene().buildIndex;
        enemyData.Add(gameObject, healthMetrics.currentHealth);
    }
    
    void OnDestroy()
    {
        int scene = SceneManager.GetActiveScene().buildIndex;
        enemyData.Remove(gameObject);
    }
}
