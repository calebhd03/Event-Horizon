using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class Portal : MonoBehaviour
{
    public string nextSceneName; // Name of the scene to load


    private void Start()
    {
        AndDestroy();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Check if the colliding object has the "Player" tag
            SetActive();
        }
    }

    private void SetActive()
    {


        // Load the next scene
        SceneManager.LoadScene(nextSceneName);
    }

    private void AndDestroy()
    {

        // Find and destroy all objects tagged as "Enemy"
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }

        // Find and destroy all objects tagged as "Enemy Spawn points"
      //  GameObject[] enemySpawnPoints = GameObject.FindGameObjectsWithTag("Enemy Spawn points");
     //   foreach (GameObject spawnPoint in enemySpawnPoints)
      //  {
      //      Destroy(spawnPoint);
      //  }

    }
}