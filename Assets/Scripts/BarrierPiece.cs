using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierPiece : MonoBehaviour
{    public float destroyDelay = 3f;
    public float scaleDownTime = 2f;
    public Vector3 targetScale = new Vector3(0.1f, 0.1f, 0.1f);

    void Start()
    {
        // Start the destruction countdown
        StartCoroutine(DestroyAfterDelay());
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

        // Wait for the specified delay
        yield return new WaitForSeconds(destroyDelay);

        // Destroy the BarrierPiece GameObject
        Destroy(gameObject);
    }
}