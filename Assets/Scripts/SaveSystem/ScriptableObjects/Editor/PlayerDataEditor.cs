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
        PlayerData playerData = (PlayerData)target;

        EditorGUILayout.Space();

        if (GUILayout.Button("Reset All Data to Default", GUILayout.Height(40)))
        {
            Undo.RecordObject(playerData, "Reset Player Data");



            // Reset boolean fields
            playerData.SavePlasmaUpgrade = false;
            playerData.SaveMeleeDamageUpgrade = false;
            playerData.SaveBHGToolUpgrade = false;
            playerData.SaveDamageOverTimeUpgrade = false;
            playerData.SaveSlowEnemyUpgrade = false;
            playerData.SaveKnockBackUpgrade = false;
            playerData.SaveOGBHGUpgrade = false;
            playerData.SaveBHGPullEffect = false;
            playerData.hasBlaster = false;
            playerData.hasNexus = false;
            playerData.tutorialComplete = false;
            playerData.hardMode = false;

            // Reset health
            playerData.currentHealth = playerData.maxHealth;

            EditorUtility.SetDirty(playerData);
        }
    }
}