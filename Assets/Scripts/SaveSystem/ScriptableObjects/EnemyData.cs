using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVariables
{
    public GameObject enemyObject { get; set; }
    public HealthMetrics healthMetrics; // Reference to HealthMetrics component
    public float currentHealth; // Store current health here

    public EnemyVariables(GameObject enemyObject, HealthMetrics healthMetrics)
    {
        this.enemyObject = enemyObject;
        this.healthMetrics = healthMetrics;
        this.currentHealth = healthMetrics.currentHealth; // Initialize current health
    }

    public void UpdateHealth()
    {
        currentHealth = healthMetrics.currentHealth; // Update current health
    }
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EnemyDataScriptableObject", order = 1)]
public class EnemyData : ScriptableObject
{
    public Dictionary<int, List<EnemyVariables>> Enemies = new Dictionary<int, List<EnemyVariables>>();

    public void Add(int scene, GameObject enemyObject, HealthMetrics healthMetrics)
    {
        if (!Enemies.ContainsKey(scene))
        {
            Enemies[scene] = new List<EnemyVariables>();
        }
        Enemies[scene].Add(new EnemyVariables(enemyObject, healthMetrics));
        var addedEnemy = Enemies[scene][Enemies[scene].Count - 1];
        Debug.Log("Scene: " + scene + ", Enemy: " + addedEnemy.enemyObject + ", Position: " + addedEnemy.enemyObject.transform.position + ", Health: " + addedEnemy.currentHealth);
    }

    public void Remove(int scene, GameObject destroyedObject)
    {
        if (Enemies.ContainsKey(scene))
        {
            Enemies[scene].RemoveAll(enemy => enemy.enemyObject == destroyedObject);
        }
    }

    public void UpdateEnemyHealth(int scene)
    {
        if (Enemies.ContainsKey(scene))
        {
            foreach (var enemy in Enemies[scene])
            {
                enemy.UpdateHealth(); // Update current health for each enemy
            }
        }
    }

    public void GetData(int scene)
    {
        UpdateEnemyHealth(scene); // Ensure current health is up to date before getting data

        if (Enemies.ContainsKey(scene))
        {
            foreach (var enemy in Enemies[scene])
            {
                Debug.Log("Scene: " + scene + ", Enemy: " + enemy.enemyObject + ", Position: " + enemy.enemyObject.transform.position + ", Health: " + enemy.currentHealth);
            }
        }
    }
}
