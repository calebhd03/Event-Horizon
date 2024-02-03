using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EnemyDataScriptableObject", order = 1)]
public class EnemyData : ScriptableObject
{
    public List<GameObject> Enemies;
    public Vector3[] enemyPositions;
    public Vector3[] enemyHealthValues;
    //public List<GameObject>[] Enemies = new List<GameObject>[SceneManager.sceneCount];
    
    /*private void OnEnable()
    {
        Enemies = new List<GameObject>[SceneManager.sceneCount];
    }*/

    public void Add(int scene, GameObject enemy)
    {
        Enemies.Add(enemy);
    }

    public void Remove(int scene, GameObject enemy)
    {
        Enemies.Remove(enemy);
    }
}
