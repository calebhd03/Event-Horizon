using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggeredGameObject : MonoBehaviour
{
 public float heightIncrease = 10.0f;
    public float spinSpeedSlow = 3.0f;
    public float spinSpeedFast = 30.0f;
    public GameObject explosionPrefab;
    public AudioClip explosionSound;
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;
    public float attackCooldown = 3.0f;
    public float moveSpeed = 5.0f;
    public float rotationSpeed = 5.0f;

    private bool triggered = false;
    private Rigidbody rb;
    private Transform playerTransform;
    private bool isAttacking = false;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private bool isMoving = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!triggered && other.CompareTag("Player"))
        {
            triggered = true;
            playerTransform = other.transform;
            initialPosition = transform.position;
            initialRotation = transform.rotation;
            StartCoroutine(AttackSequence());
        }
    }

    private IEnumerator AttackSequence()
    {
        while (true)
        {
            // Get player's position at the beginning of attack phase
            Vector3 targetPosition = playerTransform.position + (playerTransform.forward * 20.0f);

            // Move to 20 units away from player's position
            isMoving = true;
            yield return StartCoroutine(MoveToPosition(targetPosition));

            // Attack for 8 seconds
            isAttacking = true;
            yield return new WaitForSeconds(8.0f);
            isAttacking = false;

            // Cool down
            yield return new WaitForSeconds(attackCooldown);

            // Reset position and rotation
            transform.position = initialPosition;
            transform.rotation = initialRotation;
        }
    }

    private IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            // Rotate to face target position
            Vector3 targetDirection = targetPosition - transform.position;
            targetDirection.y = 0; // Ensure no tilting up or down
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Move towards target position
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            yield return null;
        }
        isMoving = false;
    }

    private void Attack()
    {
        // Rotate to face the player
        Vector3 targetDirection = playerTransform.position - transform.position;
        targetDirection.y = 0; // Ensure no tilting up or down
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Fire a bullet
        Instantiate(bulletPrefab, bulletSpawnPoint.position, transform.rotation);
    }

    private void Update()
    {
        if (isAttacking)
        {
            Attack();
        }
    }

    private void StartSpinning(float speed)
    {
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
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
    }

    private void DeathSpin()
    {
        triggered = true;
        Vector3 newPosition = transform.position;
        newPosition.y += heightIncrease;
        transform.position = newPosition;
        StartSpinning(spinSpeedSlow);
        Invoke("SpeedUpSpinning", 8.0f);
        Invoke("Explode", 8.0f);
        AudioSource.PlayClipAtPoint(explosionSound, transform.position);
        Destroy(gameObject, 8.0f);
    }
}