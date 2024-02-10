using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform spawnLocation;
    public AudioClip spawnAudio;
    [Range(0, 10)] public float spawnAudioVolume;
    private bool trigger = false;
    // Start is called before the first frame update
    void Start()
    {

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
            AudioSource.PlayClipAtPoint(spawnAudio, spawnLocation.position, spawnAudioVolume);
        }
    }
}
