using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PickupDataScriptableObject", order = 1)]
public class PickupData : ScriptableObject
{
    public int[] pickupType;
    public Vector3[] pickupPosition;
}
