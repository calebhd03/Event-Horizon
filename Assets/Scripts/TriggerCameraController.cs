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
    }
    else
    {
        Debug.LogError("LowCameraController is not assigned.");
    }
}
}