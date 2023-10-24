using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyRespwaner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject respawnEnemyPrefab;
    public Transform respawnPoint;
    private GameObject newEnemy; 
    void Start()
    {
        respawn();
    }

    // Update is called once per frame
    void Update()
    {
        if(newEnemy == null)
        {
            respawn();
        }
    }
    public void respawn()
    {
        newEnemy = Instantiate(respawnEnemyPrefab, respawnPoint.position, Quaternion.identity);
    }
}
