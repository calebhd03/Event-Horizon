using UnityEngine;
using UnityEngine.SceneManagement;
using Steamworks;

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
    public int normalNexusAmmo;
    public int nexusAmmo;
    public int nexusAmmoLoaded;
    public int nexusAmmoMax;
    //logskills
    [Header ("Skill")]
        public bool SavePlasmaUpgrade = false, SaveBHGToolUpgrade = false;
    [Header ("Skill")]
        public bool SaveDamageOverTimeUpgrade = false, SaveSlowEnemyUpgrade = false, SaveKnockBackUpgrade = false;
    [Header("Skill")]
        public bool SaveOGBHGUpgrade = false, SaveBHGPullEffect = false;
    [Header("Weapon")]
        public bool hasBlaster = false, hasNexus = false;
    [Header("Tutorial")]
        public bool tutorialComplete = false;
    [Header("Compass")]
        public bool hasCompass = false;

    //hardModeToggle
    public bool hardMode = false;

    public bool PickUp1 = false;
    public bool PickUp2 = false;

    public bool[] enemyBools, memoryBools, itemBools, journalBools;
    //LogSystemSave
    //logenemies
     public bool necroshade , necroshadeRanged , Quadravore , Crawley , Frondbeast , AscenededOnes ;
     public bool crystalCrawley , Mori , DilphopithicusFlora , Incendorathe , WitheredVines , RunAwayAnamoly ;
    //memories
     public bool mother , project004 , kaz , memory4, memory5;
    //items
    public bool ammoHole , healthPlant , poisonPlant , spaceSuit1 , spaceSuit2 , spaceSuit3 ;
    public bool spaceSuit4 , spaceSuit5 , spaceSuit6 , wreakedShip1 , wreakedShip2 , memo ;
    public bool memo1 , memo2 , memo3 , memo4 , memo5 , memo6 , tachyonTranslocator ;
    public bool dogTag , quantumStabilizer , nexusGun ;
    //journal
    public bool objective , objective1 , objective2 , objective3 , objective4 , objective5 ;
    public bool objective6 , objective7 , objective8 , objective9 , objective10 , objective11 ;
    public bool objective12 , objective13 , objective14 , objective15 , objective16 , objective17 ;

    public int crabsStuck;



    public void InitializeArrays()
    {
        // Initialize the arrays with the values of the hidden variables
        enemyBools = new bool[] { necroshade, necroshadeRanged, Quadravore, Crawley, Frondbeast, AscenededOnes, crystalCrawley, Mori, DilphopithicusFlora, Incendorathe, WitheredVines, RunAwayAnamoly };
        memoryBools = new bool[] { mother, project004, kaz, memory4, memory5};
        itemBools = new bool[] { ammoHole, healthPlant, poisonPlant, spaceSuit1, spaceSuit2, spaceSuit3, spaceSuit4, spaceSuit5, spaceSuit6, wreakedShip1, wreakedShip2, memo, memo1, memo2, memo3, memo4, memo5, memo6, tachyonTranslocator, dogTag, quantumStabilizer, nexusGun };
        journalBools = new bool[] { objective, objective1, objective2, objective3, objective4, objective5, objective6, objective7, objective8, objective9, objective10, objective11, objective12, objective13, objective14, objective15, objective16, objective17 };
    }

    public void UpdateLogArrays()
    {
        necroshade = enemyBools[0]; necroshadeRanged = enemyBools[1]; Quadravore = enemyBools[2]; Crawley = enemyBools[3]; Frondbeast = enemyBools[4]; AscenededOnes = enemyBools[5]; crystalCrawley = enemyBools[6]; Mori = enemyBools[7]; DilphopithicusFlora = enemyBools[8]; Incendorathe = enemyBools[9]; WitheredVines = enemyBools[10]; RunAwayAnamoly = enemyBools[11];
        mother = memoryBools[0]; project004 = memoryBools[1]; kaz = memoryBools[2]; memory4 = memoryBools[3]; memory5 = memoryBools[4];
        ammoHole = itemBools[0]; healthPlant = itemBools[1]; poisonPlant = itemBools[2]; spaceSuit1 = itemBools[3]; spaceSuit2 = itemBools[4]; spaceSuit3 = itemBools[5]; spaceSuit4 = itemBools[6]; spaceSuit5 = itemBools[7]; spaceSuit6 = itemBools[8]; wreakedShip1 = itemBools[9]; wreakedShip2 = itemBools[10]; memo = itemBools[11]; memo1 = itemBools[12]; memo2 = itemBools[13]; memo3 = itemBools[14]; memo4 = itemBools[15]; memo5 = itemBools[16]; memo6 = itemBools[17]; tachyonTranslocator = itemBools[18]; dogTag = itemBools[19]; quantumStabilizer = itemBools[20]; nexusGun = itemBools[21];
        objective = journalBools[0]; objective1 = journalBools[1]; objective2 = journalBools[2]; objective3 = journalBools[3]; objective4 = journalBools[4]; objective5 = journalBools[5]; objective6 = journalBools[6]; objective7 = journalBools[7]; objective8 = journalBools[8]; objective9 = journalBools[9]; objective10 = journalBools[10]; objective11 = journalBools[11]; objective12 = journalBools[12]; objective13 = journalBools[13]; objective14 = journalBools[14]; objective15 = journalBools[15]; objective16 = journalBools[16]; objective17 = journalBools[16];

        if(SteamManager.Initialized)
        {
            int enemiesUnlocked = 0;
            foreach(var enemy in enemyBools)
            {
                if(enemy == true) enemiesUnlocked++;
            }
            int memoriesUnlocked = 0;
            foreach(var memory in memoryBools)
            {
                if(memory == true) memoriesUnlocked++;
            }
            int itemsUnlocked = 0;
            foreach(var item in itemBools)
            {
                if(item == true) itemsUnlocked++;
            }
            int journalsUnlocked = 0;
            foreach(var journal in journalBools)
            {
                if(journal == true) journalsUnlocked++;
            }

            if(enemiesUnlocked == enemyBools.Length && memoriesUnlocked == memoryBools.Length && itemsUnlocked == itemBools.Length && journalsUnlocked == journalBools.Length)
            {
                if (SteamManager.Initialized) 
                {
                    SteamUserStats.SetAchievement("ACH_LOG_COMPLETE");
                }
            }

            if (SteamManager.Initialized)
            {
                SteamUserStats.StoreStats();
            }
        }
    }   


    public void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        crabsStuck = 0;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if ((scene.name == "TheOuterVer2") && (MenuScript.hardMode == true || hardMode == true))
        {
            standardAmmoDefault = normalAmmo/2;
            nexusAmmoDefault = normalNexusAmmo/2;
            hardMode = true;
            MenuScript.hardMode = true;
        }
        else if ((scene.name == "Inner") && (MenuScript.hardMode == true || hardMode == true))
        {
            standardAmmoDefault = normalAmmo/2;
            nexusAmmoDefault = normalNexusAmmo/2;
            hardMode = true;
            MenuScript.hardMode = true;
        }
        else if ((scene.name == "The Center") && (MenuScript.hardMode == true || hardMode == true))
        {
            standardAmmoDefault = normalAmmo/2;
            nexusAmmoDefault = normalNexusAmmo/2;
            hardMode = true;
            MenuScript.hardMode = true;
        }
        else if ((scene.name == "TimTutorialScene") && (MenuScript.hardMode == true || hardMode == true))
        {
            standardAmmoDefault = normalAmmo/2;
            nexusAmmoDefault = normalNexusAmmo/2;
            hardMode = true;
            MenuScript.hardMode = true;
        }
        else if(MenuScript.hardMode == false)
        {
            standardAmmoDefault = normalAmmo;
            nexusAmmoDefault = normalNexusAmmo;
            hardMode = false;
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
        //SaveMeleeDamageUpgrade = false;
        SaveBHGToolUpgrade = false;
        SaveDamageOverTimeUpgrade = false; 
        SaveSlowEnemyUpgrade = false;
        SaveKnockBackUpgrade = false;
        SaveOGBHGUpgrade = false;
        SaveBHGPullEffect = false;
        hardMode = false;
        hasCompass = false;
        PickUp1 = false;
        PickUp2 = false;
    }
    public void ResetSemiHealth()
    {
        Debug.Log("ResetSemiHealth");
        currentHealth = maxHealth;
        standardAmmo = standardAmmoDefault;
        standardAmmoLoaded = standardAmmoMax;
        shotgunAmmo = shotgunAmmoDefault;
        shotgunAmmoLoaded = shotgunAmmoMax;
        nexusAmmo = nexusAmmoDefault;
        nexusAmmoLoaded = nexusAmmoMax;

    }
    public void ResetLogSystem()
    {
    //enemies
    necroshade = false; necroshadeRanged = false; Quadravore = false; Crawley = false; Frondbeast = false; AscenededOnes = false;
    crystalCrawley = false; Mori = false; DilphopithicusFlora = false; Incendorathe = false; WitheredVines = false; RunAwayAnamoly = false;
    //memories
     mother = false; project004 = false; kaz = false;
    //items
    ammoHole = false; healthPlant = false; poisonPlant = false; spaceSuit1 = false; spaceSuit2 = false; spaceSuit3 = false;
    spaceSuit4 = false; spaceSuit5 = false; spaceSuit6 = false; wreakedShip1 = false; wreakedShip2 = false; memo = false;
    memo1 = false; memo2 = false; memo3 = false; memo4 = false; memo5 = false; memo6 = false; tachyonTranslocator = false;
    dogTag = false; quantumStabilizer = false; nexusGun = false;
    //journal
    objective = false; objective1 = false; objective2 = false; objective3 = false; objective4 = false; objective5 = false;
    objective6 = false; objective7 = false; objective8 = false; objective9 = false; objective10 = false; objective11 = false;
    objective12 = false; objective13 = false; objective14 = false; objective15 = false; objective16 = false; objective17 = false;
    }

}