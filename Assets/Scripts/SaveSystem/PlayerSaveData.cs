using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerSaveData
{
    public PlayerData playerData;
    public PlayerAmmoData ammoData;

    public PlayerSaveData(PlayerData playerData, PlayerAmmoData ammoData)
    {
        this.playerData = playerData;
        this.ammoData = ammoData;
    }
}
