using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class LowCameraController : MonoBehaviour
{
  public CinemachineVirtualCamera mainCamera;
    public Transform focusTarget;
    public float defaultRevertPriorityDelay = 10f;

    private CinemachineVirtualCamera cinemachineCamera;
    private CinemachineTrackedDolly dollyComponent;
    private CinemachineComposer composerComponent;
    private CinemachineOrbitalTransposer orbitalTransposer;

    void Start()
    {
        cinemachineCamera = GetComponent<CinemachineVirtualCamera>();
        dollyComponent = cinemachineCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
        composerComponent = cinemachineCamera.GetCinemachineComponent<CinemachineComposer>();
        orbitalTransposer = cinemachineCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();

        if (focusTarget != null)
        {
            cinemachineCamera.LookAt = focusTarget;
            cinemachineCamera.Follow = focusTarget;
        }
    }

    public void TriggerCamera(float delay, bool enableDolly, float dollyAmount, bool enablePan, float panAmount, Transform newFocusTarget, float distance, float rotation)
    {
        Debug.Log($"TriggerCamera called with delay: {delay}, enableDolly: {enableDolly}, dollyAmount: {dollyAmount}, enablePan: {enablePan}, panAmount: {panAmount}, distance: {distance}, rotation: {rotation}");

        if (newFocusTarget != null)
        {
            SetFocusTarget(newFocusTarget);
            Debug.Log("Focus target set.");
        }
        
        if (enableDolly && dollyComponent != null)
        {
            MoveDolly(dollyAmount);
            Debug.Log("Dolly moved.");
        }
        else
        {
            Debug.LogError("Dolly component not found or dolly not enabled.");
        }

        if (enablePan && composerComponent != null)
        {
            PanCamera(panAmount);
            Debug.Log("Camera panned.");
        }
        else
        {
            Debug.LogError("Composer component not found or pan not enabled.");
        }

        if (orbitalTransposer != null)
        {
            SetCameraDistance(distance);
            SetCameraRotation(rotation);
            Debug.Log("Camera distance and rotation set.");
        }
        else
        {
            Debug.LogError("OrbitalTransposer component not found.");
        }

        cinemachineCamera.Priority = 11;
        if (mainCamera != null)
            mainCamera.Priority = 8;

        StartCoroutine(RevertPriorityAfterDelay(delay));
    }

    private IEnumerator RevertPriorityAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (mainCamera != null)
        {
            mainCamera.Priority = 10;
            cinemachineCamera.Priority = 0;
        }
        Debug.Log("Priority reverted after delay.");
    }

    public void MoveDolly(float amount)
    {
        if (dollyComponent != null)
            dollyComponent.m_PathPosition += amount;
    }

    public void PanCamera(float amount)
    {
        if (composerComponent != null)
            composerComponent.m_TrackedObjectOffset.y += amount;
    }

    public void SetFocusTarget(Transform newTarget)
    {
        focusTarget = newTarget;
        cinemachineCamera.LookAt = focusTarget;
        cinemachineCamera.Follow = focusTarget;
    }

    public void SetCameraDistance(float distance)
    {
        if (orbitalTransposer != null)
            orbitalTransposer.m_FollowOffset.z = -distance;  // Assuming negative z for distance from target
    }

    public void SetCameraRotation(float rotation)
    {
        if (orbitalTransposer != null)
            orbitalTransposer.m_XAxis.Value = rotation;
    }
}