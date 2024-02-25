using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVariables
{
    public GameObject enemyObject { get; set; }
    public HealthMetrics healthMetrics;
    public float currentHealth;
    public int enemyType;
    public bool ifHasDied;

    public EnemyVariables(GameObject enemyObject, HealthMetrics healthMetrics, int enemyType)
    {
        this.enemyObject = enemyObject;
        this.healthMetrics = healthMetrics;
        this.currentHealth = healthMetrics.currentHealth;
        this.enemyType = enemyType;
        this.ifHasDied = false;
    }

    public void UpdateHealth()
    {
        currentHealth = healthMetrics.currentHealth;
    }
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EnemyDataScriptableObject", order = 1)]
public class EnemyData : ScriptableObject
{
    public Dictionary<int, List<EnemyVariables>> Enemies = new Dictionary<int, List<EnemyVariables>>();

    public void Add(int scene, GameObject enemyObject, HealthMetrics healthMetrics, int enemyType)
    {
        if (!Enemies.ContainsKey(scene))
        {
            Enemies[scene] = new List<EnemyVariables>();
        }
        Enemies[scene].Add(new EnemyVariables(enemyObject, healthMetrics, enemyType));
        var addedEnemy = Enemies[scene][Enemies[scene].Count - 1];
        Debug.Log("Scene: " + scene + ", Enemy: " + addedEnemy.enemyObject + ", Position: " + addedEnemy.enemyObject.transform.position + ", Health: " + addedEnemy.currentHealth + ", EnemyType: " + addedEnemy.enemyType);
    }

    public void Remove(int scene, GameObject destroyedObject)
    {
        if (Enemies.ContainsKey(scene))
        {
            foreach (var enemy in Enemies[scene])
            {
                if (enemy.enemyObject == destroyedObject)
                {
                    enemy.ifHasDied = true;
                    break;
                }
            }
            Enemies[scene].RemoveAll(enemy => enemy.enemyObject == destroyedObject);
        }
    }

    public void UpdateEnemyHealth(int scene)
    {
        if (Enemies.ContainsKey(scene))
        {
            foreach (var enemy in Enemies[scene])
            {
                enemy.UpdateHealth();
            }
        }
    }

    public void GetData(int scene)
    {
        UpdateEnemyHealth(scene);

        if (Enemies.ContainsKey(scene))
        {
            foreach (var enemy in Enemies[scene])
            {
                Debug.Log("Scene: " + scene + ", Enemy: " + enemy.enemyObject + ", Position: " + enemy.enemyObject.transform.position + ", Health: " + enemy.currentHealth + ", EnemyType: " + enemy.enemyType + ", IfHasDied: " + enemy.ifHasDied);
            }
        }
    }

    // Method to retrieve all enemy data
    public List<EnemyVariables> GetAllEnemyData()
    {
        List<EnemyVariables> allEnemyData = new List<EnemyVariables>();

        foreach (var sceneEnemies in Enemies.Values)
        {
            allEnemyData.AddRange(sceneEnemies);
        }

        return allEnemyData;
    }
}