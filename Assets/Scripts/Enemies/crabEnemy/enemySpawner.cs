using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform spawnLocation;
    AudioSource audioSource;
    public AudioClip spawnAudio;
    private bool trigger = false;
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && trigger == false)
        {
            trigger = true;
            Debug.Log("Spawn Enemy");
            GameObject enemy = Instantiate(enemyPrefab, spawnLocation.position, Quaternion.identity);
            audioSource.PlayOneShot(spawnAudio);
        }
    }
}
