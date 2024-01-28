using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierController : MonoBehaviour
{
    public GameObject barrierPiecesPrefab;
    public int numberOfPieces = 10;
    public float fallForce = 5.0f; // Adjust the force based on your preference

    public void DestroyBarrier()
    {
        StartCoroutine(DestroyAndInstantiate());
    }

    IEnumerator DestroyAndInstantiate()
    {
        // Declare a list to hold the instantiated pieces
        List<GameObject> barrierPieces = new List<GameObject>();

        // Instantiate BarrierPieces and add them to the pool
        for (int i = 0; i < numberOfPieces; i++)
        {
            // Calculate the position for the instantiated piece
            Vector3 spawnPosition = transform.position + Vector3.up * 2.0f; // You can adjust the "2.0f" based on your requirements

            // Instantiate the barrier piece at the calculated position
            GameObject barrierPiece = Instantiate(barrierPiecesPrefab, spawnPosition, Quaternion.identity);
            barrierPiece.GetComponent<MeshRenderer>().enabled = false;

            // Add the instantiated piece to the pool
            barrierPieces.Add(barrierPiece);

            yield return null; // Wait for the next frame before instantiating the next piece
        }

        // Enable MeshRenderers and apply forces when needed
        foreach (var piece in barrierPieces)
        {
            piece.GetComponent<MeshRenderer>().enabled = true;

            // Add a Rigidbody component and apply a downward force
            Rigidbody rigidbody = piece.GetComponent<Rigidbody>();
            if (rigidbody == null)
            {
                rigidbody = piece.AddComponent<Rigidbody>();
            }

            rigidbody.AddForce(Vector3.down * fallForce, ForceMode.Impulse);
        }

        // Destroy the current Barrier GameObject
        Destroy(gameObject);
    }
}