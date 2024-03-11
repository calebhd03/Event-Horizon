using UnityEngine;
using UnityEngine.SceneManagement;

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
    public int normalAmmo;
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
        public bool SavePlasmaUpgrade = false, SaveMeleeDamageUpgrade = false, SaveBHGToolUpgrade = false;
    [Header ("Skill")]
        public bool SaveDamageOverTimeUpgrade = false, SaveSlowEnemyUpgrade = false, SaveKnockBackUpgrade = false;
    [Header("Skill")]
        public bool SaveOGBHGUpgrade = false, SaveBHGPullEffect = false;
    [Header("Weapon")]
        public bool hasBlaster = false, hasNexus = false;
    [Header("Tutorial")]
        public bool tutorialComplete = false;

    public void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
        if (scene.name == "TheOuterVer2" && MenuScript.hardMode == true)
        {
            standardAmmoDefault = standardAmmoDefault/2;
        }
        else
        {
            standardAmmoDefault = normalAmmo;
        }
    }

    public void ResetHealthAmmo()
    {
        currentHealth = maxHealth;
        standardAmmo = standardAmmoDefault;
        standardAmmoLoaded = standardAmmoMax;
        shotgunAmmo = shotgunAmmoDefault;
        shotgunAmmoLoaded = shotgunAmmoMax;
        nexusAmmo = nexusAmmoDefault;
        nexusAmmoLoaded = nexusAmmoMax;
        hasBlaster = false; 
        hasNexus = false;
        tutorialComplete = false;
        SavePlasmaUpgrade = false;
        SaveMeleeDamageUpgrade = false;
        SaveBHGToolUpgrade = false;
        SaveDamageOverTimeUpgrade = false; 
        SaveSlowEnemyUpgrade = false;
        SaveKnockBackUpgrade = false;
        SaveOGBHGUpgrade = false;
        SaveBHGPullEffect = false;
    }

}