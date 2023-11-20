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
    public TextMeshProUGUI countdownText; // Assign this in the Inspector
    public GameObject portal; // Assign the Portal GameObject in the Inspector
    public GameObject enemyPrefab; // Reference to your enemy prefab
    public Transform spawnPoint;

    private float currentProgress = 0f;
    private float countdownTimer;
    private bool isPlayerInside = false;
    private bool countdownStopped = false;
    private bool countdownStarted = false;



    private void Start()
    {


        ResetHighlight();
        countdownTimer = progressTime;
        OriginalProgressTime = progressTime;
        portal.SetActive(false); // Set the Portal GameObject to inactive at the start
        countdownText.text = " ";
    }

    private void Update()
    {
        if (countdownStopped)
            return;

        if (isPlayerInside)
        {
            currentProgress += Time.deltaTime / progressTime;
            if (countdownTimer <= 0.1f)
            {
                portal.SetActive(true);
            }
        }

        if (isPlayerInside == false)
        {
            if (currentProgress == OriginalProgressTime)
            {
                countdownText.text = " ";
            }
        }
        else if (countdownText.text == " ")
        {
            UpdateCountdownText();
            countdownStarted = true;
        }

        if( countdownStarted == true)
        {
             UpdateCountdownText();

        }

        if (!countdownStopped && countdownStarted == true)
        {
            SpawnEnemies();
        }


        UpdateHighlight();
        
    }

    private void UpdateCountdownText()
      {
            if (countdownText != null && !countdownStopped)
            {
                if (countdownTimer > 0f)
                {
                    countdownText.text = "Time Remaining: " + countdownTimer.ToString("F1");
                }
                else
                {
                    countdownText.text = " ";
                    countdownStopped = true;
                    countdownStarted = false;
                }
            }
        }

    private void UpdateHighlight()
    {
        if (isPlayerInside)
        {
            SetHighlightColor(blueHighlightColor);
            countdownTimer -= Time.deltaTime;
        }
        else if (Physics.CheckBox(transform.position, transform.localScale / 2, transform.rotation))
        {
            SetHighlightColor(redHighlightColor);
            countdownTimer += Time.deltaTime;
        }
        else
        {
            ResetHighlight();
            countdownTimer += Time.deltaTime;
        }

        countdownTimer = Mathf.Clamp(countdownTimer, 0f, progressTime); // Ensure countdownTimer stays within [0, progressTime]
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(playerTag))
        {
            isPlayerInside = true;
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
        }
    }
    private void SpawnEnemies()
    {
        // Check if it's time to spawn enemies
        // You can adjust the conditions based on your needs
        if (Time.timeSinceLevelLoad > 2f && Time.timeSinceLevelLoad < 3f)
        {
            InstantiateEnemy(1);
        }
        else if (Time.timeSinceLevelLoad > 3.3f && Time.timeSinceLevelLoad < 4.3f)
        {
            InstantiateEnemy(2);
        }
        // Add more conditions as needed for subsequent waves
    }

    private void InstantiateEnemy(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
            // Optionally, you can do additional setup for the spawned enemy here
        }
    }

}