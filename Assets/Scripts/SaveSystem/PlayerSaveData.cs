using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerSaveData
{
    public PlayerDataOld playerData;
    public PlayerAmmoData ammoData;
    public PlayerHealth healthData;

    public PlayerSaveData(PlayerDataOld player, PlayerAmmoData ammo, PlayerHealth health)
    {
        playerData = player;
        ammoData = ammo;
        healthData = health;
    }
}
