using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerAmmoData
{
    public int testData;
    public int standardAmmoSave;
    public int standardAmmoLoadedSave;
    public int blackHoleAmmoSave;
    public int blackHoleAmmoLoadedSave;
    public int shotgunAmmoSave;
    public int shotgunAmmoLoadedSave;

    public PlayerAmmoData(SaveSystemTest playerP)
    {
        testData = playerP.testData;
        standardAmmoSave = playerP.standardAmmoSave;
        standardAmmoLoadedSave = playerP.standardAmmoLoadedSave;
        blackHoleAmmoSave = playerP.blackHoleAmmoSave;
        blackHoleAmmoLoadedSave = playerP.blackHoleAmmoLoadedSave;
        shotgunAmmoSave = playerP.shotgunAmmoSave;
        shotgunAmmoLoadedSave = playerP.shotgunAmmoLoadedSave;
    }
} 
