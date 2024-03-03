
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    // List of all enemy prefabs, indexed by their type
    public GameObject[] enemyPrefabs;

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

    public GameObject GetEnemyPrefab(int enemyType)
    {
        if (enemyType >= 0 && enemyType < enemyPrefabs.Length)
        {
            return enemyPrefabs[enemyType];
        }
        else
        {
            Debug.LogError("Enemy type out of range: " + enemyType);
            return null;
        }
    }
}