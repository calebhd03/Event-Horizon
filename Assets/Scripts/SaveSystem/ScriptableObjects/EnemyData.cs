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
    public List<EnemyVariables> Enemies = new List<EnemyVariables>();
    //public List<GameObject>[] Enemies = new List<GameObject>[SceneManager.sceneCount];
    
    /*private void OnEnable()
    {
        Enemies = new List<GameObject>[SceneManager.sceneCount];
    }*/

    public void Add(GameObject enemyObject, float enemyHealth)
    {
        var enemy = new EnemyVariables(enemyObject, enemyHealth);
        Enemies.Add(enemy);
        Debug.Log("Enemy: " + enemy.enemyObject + ", Position: " + enemy.enemyObject.transform.position + ", Health: " + enemy.enemyHealth);
    }

    public void Remove(GameObject destroyedObject)
    {
        Enemies.RemoveAll(enemy => enemy.enemyObject == destroyedObject);
    }

    public void GetData()
    {
        foreach (var enemy in Enemies)
        {
            Debug.Log("Enemy: " + enemy.enemyObject + ", Position: " + enemy.enemyObject.transform.position + ", Health: " + enemy.enemyHealth);
        }
    }
}
