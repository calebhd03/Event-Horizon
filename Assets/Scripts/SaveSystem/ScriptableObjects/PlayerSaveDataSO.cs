using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PlayerSaveDataScriptableObject", order = 1)]
public class PlayerSaveDataSO : ScriptableObject
{
    public Vector3[] playerPosition;
    public Vector3[] playerHealthValue;
    public int standardAmmo;
    public int shotgunAmmo;
    public int nexusAmmo;
}