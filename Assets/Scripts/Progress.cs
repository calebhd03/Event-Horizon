using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Progress : MonoBehaviour
{
    public float progressTime = 15.0f; // Time in seconds to reach full progress
    public string nextSceneName; // Name of the scene to load when progress is full
    public string playerTag = "Player"; // Tag for the player object
    public string enemyTag = "Enemy"; // Tag for objects tagged as "Enemy"
    public Material highlightMaterial; // Material for highlighting
    public Color blueHighlightColor = Color.blue;
    public Color redHighlightColor = Color.red;
    public TextMeshProUGUI countdownText; // Assign this in the Inspector

    private float currentProgress = 0f;
    private float currentTimeRemaining;
    private bool isPlayerInside = false;

    private void Start()
    {
        ResetHighlight();
        currentTimeRemaining = progressTime;
    }

    private void Update()
    {
        if (isPlayerInside)
        {
            currentProgress += Time.deltaTime / progressTime;
            UpdateCountdownText();
            if (currentProgress >= 1.0f)
            {
                LoadNextScene();
            }
        }

        
        UpdateHighlight();

        if (!isPlayerInside)
        {
            DecreaseTimeRemaining();
        }
    }

    private void UpdateCountdownText()
    {
        if (currentTimeRemaining > 0)
        {
            currentTimeRemaining -= Time.deltaTime;
        }
        else
        {
            currentTimeRemaining = 0;
        }

        if (countdownText != null)
        {
            countdownText.text = "Time Remaining: " + currentTimeRemaining.ToString("F1");
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

    private void DecreaseTimeRemaining()
    {
        if (currentTimeRemaining < progressTime)
        {
            currentTimeRemaining += Time.deltaTime;
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

    private void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}