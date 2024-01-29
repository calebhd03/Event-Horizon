using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EnemyDataScriptableObject", order = 1)]
public abstract class EnemyData<GameObject> : ScriptableObject
{
    public List<GameObject> Enemies = new List<GameObject>();
    public Vector3[] enemyPositions;
    public Vector3[] enemyHealthValues;

    public void Add(GameObject gameObject)
    {
        if (!Enemies.Contains(gameObject)) Enemies.Add(gameObject);
    }

    public void Remove(GameObject gameObject)
    {
        if (Enemies.Contains(gameObject)) Enemies.Remove(gameObject);
    }
}
