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

    private float currentProgress = 0f;
    private bool isPlayerInside = false;
    private bool countdownStopped = false;
    private bool countdownStarted = false;

    private void Start()
    {
        ResetHighlight();
        OriginalProgressTime = progressTime;
        portal.SetActive(false); // Set the Portal GameObject to inactive at the start
        if (progressBar != null)
        {
            SetProgressBarScale(0f); // Set the initial scale of the progress bar to 0
        }
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
}