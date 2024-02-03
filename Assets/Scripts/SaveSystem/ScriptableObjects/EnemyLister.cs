using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyLister : MonoBehaviour 
{
    public EnemyData enemyData;

    void OnEnable()
    {
        int scene = SceneManager.GetActiveScene().buildIndex;
        enemyData.Add(scene, gameObject);
    }
    
    void OnDestroy()
    {
        int scene = SceneManager.GetActiveScene().buildIndex;
        enemyData.Remove(scene, gameObject);
    }
}
