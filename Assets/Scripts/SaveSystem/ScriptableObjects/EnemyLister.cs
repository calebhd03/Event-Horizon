using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyLister : MonoBehaviour 
{
    public EnemyData enemyData;
    public HealthMetrics healthMetrics;
    [SerializeField]private int listIndex;
    public int scene;

    void OnEnable()
    {
        healthMetrics = GetComponent<HealthMetrics>();
        scene = SceneManager.GetActiveScene().buildIndex;
        enemyData.Add(scene, gameObject, healthMetrics);
    }

    void UpdateData()
    {
        healthMetrics = GetComponent<HealthMetrics>();
        scene = SceneManager.GetActiveScene().buildIndex;
        enemyData.Add(scene, gameObject, healthMetrics);
    }
    
    void OnDestroy()
    {
        enemyData.Remove(scene, gameObject);
    }
}
