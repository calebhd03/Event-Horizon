using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable] // Add this attribute to mark the class as serializable
public class EnemyVariables
{
    // Your class implementation remains unchanged
    public GameObject enemyObject;
    public HealthMetrics healthMetrics;
    public int enemyType;
    public bool ifHasDied;
    public Vector3 lastSavedPosition; // Add lastSavedPosition property
    public float currentHealth; // Add currentHealth property
    // Adjust the constructor to accept 4 arguments
    public EnemyVariables(GameObject enemyObject, HealthMetrics healthMetrics, int enemyType, Vector3 lastSavedPosition)
    {
        this.enemyObject = enemyObject;
        this.healthMetrics = healthMetrics;
        this.enemyType = enemyType;
        this.ifHasDied = false;
        this.lastSavedPosition = lastSavedPosition;
    }

    // Add a constructor that accepts 5 arguments
    public EnemyVariables(GameObject enemyObject, HealthMetrics healthMetrics, int enemyType, Vector3 lastSavedPosition, float currentHealth)
    {
        this.enemyObject = enemyObject;
        this.healthMetrics = healthMetrics;
        this.enemyType = enemyType;
        this.ifHasDied = false;
        this.lastSavedPosition = lastSavedPosition;
        this.currentHealth = currentHealth;
    }

    // Update the health method to set the current health
    public void UpdateHealth()
    {
        if (healthMetrics != null)
        {
            currentHealth = healthMetrics.currentHealth;
        }
        else
        {
            Debug.LogWarning("HealthMetrics is null in EnemyVariables.");
        }
    }
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EnemyDataScriptableObject", order = 1)]
public class EnemyData : ScriptableObject
{
    public Dictionary<int, List<EnemyVariables>> Enemies = new Dictionary<int, List<EnemyVariables>>();

    public void Add(int scene, GameObject enemyObject, HealthMetrics healthMetrics, int enemyType, Vector3 lastSavedPosition)
    {
        if (!Enemies.ContainsKey(scene))
        {
            Enemies[scene] = new List<EnemyVariables>();
        }
        Enemies[scene].Add(new EnemyVariables(enemyObject, healthMetrics, enemyType, lastSavedPosition, scene));
        var addedEnemy = Enemies[scene][Enemies[scene].Count - 1];
        Debug.Log("Scene: " + scene + ", Enemy: " + addedEnemy.enemyObject + ", Position: " + addedEnemy.enemyObject.transform.position + ", Health: " + addedEnemy.currentHealth + ", EnemyType: " + addedEnemy.enemyType + ", IfHasDied: " + addedEnemy.ifHasDied);
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