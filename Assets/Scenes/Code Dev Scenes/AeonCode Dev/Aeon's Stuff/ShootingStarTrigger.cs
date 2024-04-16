using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingStarTrigger : MonoBehaviour
{
    public GameObject Stars;        // Public GameObject to be manipulated
    public Transform EndPosition;   // Target position for the Stars to move towards
    public float MoveSpeed = 5f;    // Speed at which the Stars will move
    public float DurationActive = 10f; // Duration for which Stars remain active before resetting

    private Rigidbody starsRigidbody;   // Rigidbody component of the Stars
    private Vector3 originalPosition;   // Original position of Stars
    private float timer;                // Timer to track activation duration
    private bool isActive;              // Flag to check if the Stars are currently active

    private void Start()
    {
        if (Stars != null)
        {
            starsRigidbody = Stars.GetComponent<Rigidbody>();
            if (starsRigidbody == null)
            {
                Debug.LogError("Rigidbody component not found on the Stars GameObject!");
            }
            originalPosition = Stars.transform.position;
            Stars.SetActive(false);
        }
        else
        {
            Debug.LogError("Stars GameObject is not assigned!");
        }
    }

    public void CallStars()
    {
        if (Stars != null && starsRigidbody != null && !isActive)
        {
            Stars.SetActive(true);
            timer = DurationActive;  // Reset the timer to the duration
            starsRigidbody.position = originalPosition; // Ensure it starts from the original position
            isActive = true;
        }
    }

    private void FixedUpdate()
    {
        if (Stars.activeSelf && timer > 0)
        {
            MoveStarsTowardsTarget();
            timer -= Time.fixedDeltaTime;
        }
        else if (timer <= 0 && isActive)
        {
            ResetStars();
        }
    }

    private void MoveStarsTowardsTarget()
    {
        if (Vector3.Distance(starsRigidbody.position, EndPosition.position) > 0.01f)
        {
            Vector3 newPosition = Vector3.MoveTowards(starsRigidbody.position, EndPosition.position, MoveSpeed * Time.fixedDeltaTime);
            starsRigidbody.MovePosition(newPosition);
        }
        else
        {
            ResetStars(); // Automatically reset if it reaches the end position
        }
    }

    private void ResetStars()
    {
        starsRigidbody.position = originalPosition;
        Stars.SetActive(false);
        isActive = false;
    }
}
