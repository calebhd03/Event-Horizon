using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierController : MonoBehaviour
{
    public GameObject barrierPiecesPrefab;
    public int numberOfPieces = 10;



    public void DestroyBarrier()
    {
        StartCoroutine(DestroyAndInstantiate());
    }

    IEnumerator DestroyAndInstantiate()
    {
        // Instantiate BarrierPieces
        for (int i = 0; i < numberOfPieces; i++)
        {
            Instantiate(barrierPiecesPrefab, transform.position, Quaternion.identity);
            yield return null; // Wait for the next frame before instantiating the next piece
        }

        // Destroy the current Barrier GameObject
        Destroy(gameObject);
    }
}