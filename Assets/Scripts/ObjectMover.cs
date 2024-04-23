using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    public float moveSpeed = 5f; // Adjustable speed
    public float distance = 1f; // Adjustable distance

    public void MoveUp()
    {
        StartCoroutine(MoveObject(transform.position + Vector3.up * distance));
    }

    public void MoveDown()
    {
        StartCoroutine(MoveObject(transform.position + Vector3.down * distance));
    }

    private IEnumerator MoveObject(Vector3 targetPosition)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPosition;
    }
}