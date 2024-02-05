using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyData))]
public class EnemyDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var script = (EnemyData)target;

            if(GUILayout.Button("Get Data", GUILayout.Height(40)))
            {
                script.GetData();
            }
        
    }   
}
