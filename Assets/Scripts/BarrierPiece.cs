using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierPiece : MonoBehaviour
{ 
    public float destroyDelay = 3f;
    public float scaleDownTime = 2f;
    public Vector3 targetScale = new Vector3(0.1f, 0.1f, 0.1f);

    void Awake()
    {
        // Randomize initial scale and rotation
        RandomizeScaleAndRotation();

        // Start the destruction countdown
        StartCoroutine(DestroyAfterDelay());
    }

    void RandomizeScaleAndRotation()
    {
        // Randomize scale in the range of 0.4 to 1.5 for each axis (x, y, z)
        float randomScaleX = Random.Range(0.4f, 1.5f);
        float randomScaleY = Random.Range(0.4f, 1.5f);
        float randomScaleZ = Random.Range(0.4f, 1.5f);

        // Set the random scale
        transform.localScale = new Vector3(randomScaleX, randomScaleY, randomScaleZ);

        // Randomize rotation
        transform.rotation = Random.rotation;
    }

    IEnumerator DestroyAfterDelay()
    {
        float currentTime = 0f;
        Vector3 originalScale = transform.localScale;

        // Scale down over time
        while (currentTime < scaleDownTime)
        {
            transform.localScale = Vector3.Lerp(originalScale, targetScale, currentTime / scaleDownTime);
            currentTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final scale is set explicitly to avoid any interpolation errors
        transform.localScale = targetScale;

        // Wait for the specified delay
        yield return new WaitForSeconds(destroyDelay);

        // Destroy the BarrierPiece GameObject
        Destroy(gameObject);
    }
}