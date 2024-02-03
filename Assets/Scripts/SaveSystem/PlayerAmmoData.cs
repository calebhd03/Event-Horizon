using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerAmmoData
{
    public int testData;
    public int standardAmmoSave;
    public int standardAmmoLoadedSave;
    public int nexusAmmoSave;
    public int nexusAmmoLoadedSave;
    public int shotgunAmmoSave;
    public int shotgunAmmoLoadedSave;

    public PlayerAmmoData(SaveSystemTest playerP)
    {
        testData = playerP.testData;
        standardAmmoSave = playerP.standardAmmoSave;
        standardAmmoLoadedSave = playerP.standardAmmoLoadedSave;
        nexusAmmoSave = playerP.nexusAmmoSave;
        nexusAmmoLoadedSave = playerP.nexusAmmoLoadedSave;
        shotgunAmmoSave = playerP.shotgunAmmoSave;
        shotgunAmmoLoadedSave = playerP.shotgunAmmoLoadedSave;
    }
} 
