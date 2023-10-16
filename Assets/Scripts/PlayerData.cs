using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData 
{
    public int testData;
    public int standardAmmoSave;
    public int blackHoleAmmoSave;
    public int shotgunAmmoSave;
    
    public float[] position;

    public PlayerData (SaveSystemTest player)
    {
        testData = player.testData;
        standardAmmoSave = player.standardAmmoSave;
        blackHoleAmmoSave = player.blackHoleAmmoSave;
        shotgunAmmoSave = player.shotgunAmmoSave;

        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;
    }


}
