using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVariables
{
    public GameObject enemyObject {get; set;}
    public float enemyHealth {get; set;}

    public EnemyVariables(GameObject enemyObject, float enemyHealth)
    {
        this.enemyObject = enemyObject;
        this.enemyHealth = enemyHealth;
    }
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EnemyDataScriptableObject", order = 1)]
public class EnemyData : ScriptableObject
{
    public Dictionary<int, List<EnemyVariables>> Enemies = new Dictionary<int, List<EnemyVariables>>();

    public void Add(int scene, GameObject enemyObject, float enemyHealth)
    {
        if (!Enemies.ContainsKey(scene))
        {
            Enemies[scene] = new List<EnemyVariables>();
        }
        Enemies[scene].Add(new EnemyVariables(enemyObject, enemyHealth));
        var addedEnemy = Enemies[scene][Enemies[scene].Count - 1];
        Debug.Log("Scene: " + scene + ", Enemy: " + addedEnemy.enemyObject + ", Position: " + addedEnemy.enemyObject.transform.position + ", Health: " + addedEnemy.enemyHealth);
    }

    public void Remove(int scene, GameObject destroyedObject)
    {
        if (Enemies.ContainsKey(scene))
        {
            Enemies[scene].RemoveAll(enemy => enemy.enemyObject == destroyedObject);
        }
    }

    public void GetData(int scene)
    {
        if (Enemies.ContainsKey(scene))
        {
            foreach (var enemy in Enemies[scene])
            {
                Debug.Log("Scene: " + scene + ", Enemy: " + enemy.enemyObject + ", Position: " + enemy.enemyObject.transform.position + ", Health: " + enemy.enemyHealth);
            }
        }
    }
}
