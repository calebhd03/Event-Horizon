using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData 
{

    
    public float[] position;

    public PlayerData(SaveSystemTest playerP)
    {
        position = new float[3];
        position[0] = playerP.transform.position.x;
        position[1] = playerP.transform.position.y;
        position[2] = playerP.transform.position.z;
    }


}