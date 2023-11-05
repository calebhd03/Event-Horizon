using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerAmmoData
{
    public int testData;
    public int standardAmmoSave;
    public int blackHoleAmmoSave;
    public int shotgunAmmoSave;

    public PlayerAmmoData(SaveSystemTest playerP)
    {
        testData = playerP.testData;
        standardAmmoSave = playerP.standardAmmoSave;
        blackHoleAmmoSave = playerP.blackHoleAmmoSave;
        shotgunAmmoSave = playerP.shotgunAmmoSave;
    }
} 
