using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Progress : MonoBehaviour
{
    public float progressTime = 15.0f; // Time in seconds to reach full progress
    private float OriginalProgressTime;
    public string playerTag = "Player"; // Tag for the player object
    public string enemyTag = "Enemy"; // Tag for objects tagged as "Enemy"
    public Material highlightMaterial; // Material for highlighting
    public Color blueHighlightColor = Color.blue;
    public Color redHighlightColor = Color.red;
    public GameObject progressBar; // Assign the ProgressBar GameObject in the Inspector
    public GameObject portal; // Assign the Portal GameObject in the Inspector

    private bool enemy1Activated = false;
    private bool enemy2Activated = false;
    private bool enemy3Activated = false;

    public GameObject enemyPrefab1; // Assign the enemy prefabs in the Inspector
    public GameObject enemyPrefab2;
    public GameObject enemyPrefab3;

    private GameObject enemyInstance1;
    private GameObject enemyInstance2;
    private GameObject enemyInstance3;

    private float currentProgress = 0f;
    private bool isPlayerInside = false;
    private bool PlayerStillInside = false;
    private bool countdownStopped = false;
    private bool countdownStarted = false;

    public Vector3 spawnPosition1; // Replace with your actual spawn positions
    public Vector3 spawnPosition2;
    public Vector3 spawnPosition3;

    private bool resettingProgress = false;

    private void Start()
    {
        ResetHighlight();
        OriginalProgressTime = progressTime;
        portal.SetActive(false); // Set the Portal GameObject to inactive at the start
        if (progressBar != null)
        {
            SetProgressBarScale(0f); // Set the initial scale of the progress bar to 0
        }

        // Instantiate initial enemies
        InstantiateEnemies();
    }

    private void Update()
    {
        if (countdownStopped)
            return;

        if (isPlayerInside)
        {
            currentProgress += Time.deltaTime / progressTime;
            if (progressBar != null)
            {
                SetProgressBarScale(currentProgress);
            }

            if (currentProgress >= 1.0f)
            {
                portal.SetActive(true);

                // Check if progress is not being reset before instantiating new enemies
                if (!resettingProgress)
                {
                    InstantiateEnemies();
                }
            }
        }
        else
        {
            if (progressBar != null)
            {
                SetProgressBarScale(Mathf.Lerp(progressBar.transform.localScale.x, 0f, Time.deltaTime * 2f));
            }
        }

        UpdateHighlight();
        ActivateEnemies();

        if (enemy1Activated && enemyInstance1 == null && !enemy2Activated)
        {
            ActivateEnemy2();
        }

        if (enemy2Activated && enemyInstance2 == null && !enemy3Activated)
        {
            ActivateEnemy3();
        }
    }

    private void UpdateHighlight()
    {
        if (isPlayerInside)
        {
            SetHighlightColor(blueHighlightColor);
        }
        else if (Physics.CheckBox(transform.position, transform.localScale / 2, transform.rotation))
        {
            SetHighlightColor(redHighlightColor);
        }
        else
        {
            ResetHighlight();
        }
    }

    private void SetHighlightColor(Color color)
    {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            Material[] materials = renderer.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = highlightMaterial;
                materials[i].color = color;
            }
            renderer.materials = materials;
        }
    }

    private void ResetHighlight()
    {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            Material[] materials = renderer.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = null; // Reset material to default
            }
            renderer.materials = materials;
        }
    }

    private void SetProgressBarScale(float scale)
    {
        if (progressBar != null)
        {
            scale = Mathf.Clamp01(scale); // Ensure scale stays within [0, 1]
            Vector3 newScale = new Vector3(scale, progressBar.transform.localScale.y, progressBar.transform.localScale.z);
            progressBar.transform.localScale = newScale;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(playerTag))
        {
            isPlayerInside = true;
            PlayerStillInside = true;
            
        }
        else if (other.gameObject.CompareTag(enemyTag))
        {
            isPlayerInside = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(playerTag))
        {
            isPlayerInside = false;
            PlayerStillInside = false;
        }
        else if (other.gameObject.CompareTag(enemyTag) && PlayerStillInside)
        {
            isPlayerInside = true;
        }
    
    }


    private void ActivateEnemies()
    {
         float progressThreshold1 = 1f / 9f;
        if (!enemy1Activated && currentProgress >= progressThreshold1)
        {
            if (enemyInstance1 != null)
            {
                enemyInstance1.SetActive(true);
                enemy1Activated = true;
            }
        }
    }

    private void ActivateEnemy2()
    {
        float progressThreshold2 = 0.4f; // Activate enemy2 at 2/5 progress
        if (!enemy2Activated && currentProgress >= progressThreshold2)
        {
            if (enemyInstance2 != null)
            {
                enemyInstance2.SetActive(true);
                enemy2Activated = true;
            }
        }
    }

    private void ActivateEnemy3()
    {
        float progressThreshold3 = 0.6f; // Activate enemy3 at 3/5 progress
        if (!enemy3Activated && currentProgress >= progressThreshold3)
        {
            if (enemyInstance3 != null)
            {
                enemyInstance3.SetActive(true);
                enemy3Activated = true;
            }
        }
    }

    public void ResetProgress()
    {
        Debug.Log("Progress Reset");

        // Set the flag to indicate that progress is being reset
        resettingProgress = true;

        // Destroy existing enemies
        DestroyExistingEnemies();

        // Reset progress-related variables
        currentProgress = 0f;
        enemy1Activated = false;
        enemy2Activated = false;
        enemy3Activated = false;

        // Reset progress time to the original value
        progressTime = OriginalProgressTime;

        // Deactivate portal
        if (portal != null)
        {
            portal.SetActive(false);
        }

        // Reset countdown if needed
        countdownStopped = false;
        countdownStarted = false;

        // Reset the flag after everything is done
        resettingProgress = false;

        // Instantiate new enemies
        InstantiateEnemies();
    }

    private void DestroyExistingEnemies()
    {
        // Destroy existing enemy instances if they exist
        if (enemyInstance1 != null)
        {
            Destroy(enemyInstance1);
        }

        if (enemyInstance2 != null)
        {
            Destroy(enemyInstance2);
        }

        if (enemyInstance3 != null)
        {
            Destroy(enemyInstance3);
        }
    }

    private void InstantiateEnemies()
    {
        // Instantiate new enemies
        if (enemyPrefab1 != null)
        {
            enemyInstance1 = Instantiate(enemyPrefab1, spawnPosition1, Quaternion.identity);
            enemyInstance1.SetActive(false);
        }

        if (enemyPrefab2 != null)
        {
            enemyInstance2 = Instantiate(enemyPrefab2, spawnPosition2, Quaternion.identity);
            enemyInstance2.SetActive(false);
        }

        if (enemyPrefab3 != null)
        {
            enemyInstance3 = Instantiate(enemyPrefab3, spawnPosition3, Quaternion.identity);
            enemyInstance3.SetActive(false);
        }
    }
}