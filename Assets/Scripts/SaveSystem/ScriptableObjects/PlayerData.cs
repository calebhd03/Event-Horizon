using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PlayerData", order = 1)]
[System.Serializable]
public class PlayerData : ScriptableObject
{
    [Header("Location")]
    public int currentSceneIndex;
    public Vector3 playerPosition;

    [Header("Health")]
    public float maxHealth;
    public float currentHealth;

    [Header ("Ammo")]
    [SerializeField] private int standardAmmoDefault;
    public int standardAmmo;
    public int standardAmmoLoaded;
    public int standardAmmoMax;
    [Space(10)]
    [SerializeField] private int shotgunAmmoDefault;
    public int shotgunAmmo;
    public int shotgunAmmoLoaded;
    public int shotgunAmmoMax;
    [Space(10)]
    [SerializeField] private int nexusAmmoDefault;
    public int nexusAmmo;
    public int nexusAmmoLoaded;
    public int nexusAmmoMax;
    [Header ("Skill")]
        public bool SaveDamageUpgrade = false, SaveHealthUpgrade = false, SaveMeleeDamageUpgrade = false, SaveSpeedUpgrade = false;
    [Header ("Skill")]
        public bool SaveDamageOverTimeUpgrade = false, SaveAmmoCapacityUpgrade = false, SaveSlowEnemyUpgrade = false, SaveKnockBackUpgrade = false;
    [Header("Skill")]
        public bool SavePlasmaEnergyUpgrade = false;

    public void ResetHealthAmmo()
    {
        currentHealth = maxHealth;
        standardAmmo = standardAmmoDefault;
        standardAmmoLoaded = standardAmmoMax;
        shotgunAmmo = shotgunAmmoDefault;
        shotgunAmmoLoaded = shotgunAmmoMax;
        nexusAmmo = nexusAmmoDefault;
        nexusAmmoLoaded = nexusAmmoMax;
    }

}