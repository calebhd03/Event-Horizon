using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyMusicController : MonoBehaviour
{
    public float range = 2f;
    [SerializeField] private Collider[] colliderArray;
    private bool enemymusicplaying = false; // Flag to track music state
    string sceneName;
    public LayerMask layerMask;

    private void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;
        colliderArray = new Collider[0]; // Initialize an empty array
    }

    private void Update()
    {
        UpdateColliderArray();

        // If music is not playing and there are enemy colliders in range, play music
        if (!enemymusicplaying && colliderArray.Length > 0)
        {
            if (Background_Music.instance != null) Background_Music.instance.EnemyMusic();
            enemymusicplaying = true;
        }
        // If music is playing and there are no enemy colliders in range, stop music
        else if (enemymusicplaying && colliderArray.Length == 0)
        {
            if (Background_Music.instance != null) Background_Music.instance.PlayLevelMusic(sceneName);
            enemymusicplaying = false;
        }
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // If no music is playing, play music
            if (!enemymusicplaying)
            {
                Background_Music.instance.EnemyMusic();
                enemymusicplaying = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // If music is playing and no enemy colliders are present, stop music
            if (enemymusicplaying && colliderArray.Length == 0)
            {
                Background_Music.instance.PlayLevelMusic(sceneName);
                enemymusicplaying = false;
            }
        }
    }*/

    // Update colliderArray with colliders in range
    private void UpdateColliderArray()
    {
        colliderArray = Physics.OverlapSphere(transform.position, range, layerMask);
    }
}
