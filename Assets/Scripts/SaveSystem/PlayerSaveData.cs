using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerSaveData
{
    public PlayerData playerData;
    public PlayerAmmoData ammoData;
    public PlayerHealth healthData;

    public PlayerSaveData(PlayerData player, PlayerAmmoData ammo, PlayerHealth health)
    {
        playerData = player;
        ammoData = ammo;
        healthData = health;
    }
}
