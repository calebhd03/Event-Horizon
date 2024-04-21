using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCameraController : MonoBehaviour
{
    public LowCameraController lowCameraController;
    public GameObject goalObject;
    public float triggerDelay = 10f;
    public bool enableDolly = true;
    public float dollyAmount = 0f;
    public bool enablePan = true;
    public float panAmount = 0f;
    public Transform newFocusTarget;
    public float distance = 10f;
    public float rotation = 0f;

    private ObjectMover objectMover; // Reference to ObjectMover component on newFocusTarget

    void Start()
    {
        if (newFocusTarget != null)
        {
            objectMover = newFocusTarget.GetComponent<ObjectMover>(); // Get the ObjectMover component
        }
    }

    void Update()
    {
        if (goalObject == null || !goalObject.activeInHierarchy) // Checks if the goal object is destroyed or inactive
        {
            TriggerCamera();
            enabled = false; // Disable the update to prevent retriggering
        }
    }

    void TriggerCamera()
    {
        if (lowCameraController != null)
        {
            lowCameraController.TriggerCamera(triggerDelay, enableDolly, dollyAmount, enablePan, panAmount, newFocusTarget, distance, rotation);
            
            // Check if objectMover is not null and call MoveUp or MoveDown accordingly
            if (objectMover != null)
            {
                if (dollyAmount > 0f)
                {
                    objectMover.MoveUp(); // MoveUp if dollyAmount is positive
                }
                else if (dollyAmount < 0f)
                {
                    objectMover.MoveDown(); // MoveDown if dollyAmount is negative
                }
            }
        }
        else
        {
            Debug.LogError("LowCameraController is not assigned.");
        }
    }
}