using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerData))]
public class PlayerDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var script = (PlayerData)target;

            if(GUILayout.Button("Reset Health/Ammo to Default", GUILayout.Height(40)))
            {
                script.ResetHealthAmmo();
            }
        
    }   
}