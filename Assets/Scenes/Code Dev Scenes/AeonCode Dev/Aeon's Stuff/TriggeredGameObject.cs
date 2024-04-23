using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggeredGameObject : MonoBehaviour
{
    public float heightIncrease = 10.0f; // Amount of units to increase height
    public float spinSpeedSlow = 3.0f; // Initial speed at which the object spins around the Y-axis
    public float spinSpeedFast = 30.0f; // Speed after 8 seconds
    public GameObject explosionPrefab; // Particle effect prefab
    public AudioClip explosionSound; // Sound to play on explosion

    private bool triggered = false;
    private Rigidbody rb;

    private void OnTriggerEnter(Collider other)
    {
        if (!triggered && other.CompareTag("Player"))
        {
            triggered = true;

            // Increase height
            Vector3 newPosition = transform.position;
            newPosition.y += heightIncrease;
            transform.position = newPosition;

            // Start spinning
            StartSpinning(spinSpeedSlow);

            // Invoke method to speed up spinning after 8 seconds
            Invoke("SpeedUpSpinning", 8.0f);

            // Instantiate particle effect after a delay
            Invoke("Explode", 8.0f);

            // Play sound effect
            AudioSource.PlayClipAtPoint(explosionSound, transform.position);

            // Destroy this object after a delay
            Destroy(gameObject, 8.0f);
        }
    }

    private void StartSpinning(float speed)
    {
        // Start spinning around the Y-axis
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = true;
        }
        rb.angularVelocity = Vector3.up * speed;
    }

    private void SpeedUpSpinning()
    {
        if (rb != null)
        {
            rb.angularVelocity = Vector3.up * spinSpeedFast;
        }
    }

    private void Explode()
    {
        // Instantiate particle effect
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
    }
}