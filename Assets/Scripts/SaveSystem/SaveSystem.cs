using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

public static class SaveSystem 
{
    public static void SavePlayerData(PlayerDataOld playerDataOld)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/playerData.data";
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, playerDataOld);
        stream.Close();
    }

    public static PlayerDataOld LoadPlayerData()
    {
        string path = Application.persistentDataPath + "/playerData.data";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            PlayerDataOld data = formatter.Deserialize(stream) as PlayerDataOld;
            stream.Close();
            return data;
        }
        return null;
    }

    public static void SavePlayerAmmoData(PlayerAmmoData ammoData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/playerAmmoData.data";
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, ammoData);
        stream.Close();
    }

    public static PlayerAmmoData LoadPlayerAmmoData()
    {
        string path = Application.persistentDataPath + "/playerAmmoData.data";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            PlayerAmmoData data = formatter.Deserialize(stream) as PlayerAmmoData;
            stream.Close();
            return data;
        }
        return null;
    }

public static void SavePlayer(PlayerSaveData saveData)
    {
        SavePlayerData(saveData.playerData);
        SavePlayerAmmoData(saveData.ammoData);
        SavePlayerHealth(saveData.healthData);
    }

    public static PlayerSaveData LoadPlayer()
    {
        PlayerDataOld playerDataOld = LoadPlayerData();
        PlayerAmmoData ammoData = LoadPlayerAmmoData();
        PlayerHealth healthData = LoadPlayerHealth();

        if (playerDataOld != null && ammoData != null && healthData != null)
        {
            return new PlayerSaveData(playerDataOld, ammoData, healthData);
        }
        return null;
    }

    private static void SavePlayerHealth(PlayerHealth health)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/playerHealth.data";
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, health);
        stream.Close();
    }

    private static PlayerHealth LoadPlayerHealth()
    {
        string path = Application.persistentDataPath + "/playerHealth.data";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            PlayerHealth health = formatter.Deserialize(stream) as PlayerHealth;
            stream.Close();
            return health;
        }
        return null;
    }

  public static void SaveEnemyData(EnemyData enemyData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/enemyData.data";
        FileStream stream = new FileStream(path, FileMode.Create);

        List<EnemyVariables> allEnemies = enemyData.GetAllEnemyData(); // Retrieve all enemy data
        formatter.Serialize(stream, allEnemies); // Serialize and save enemy data

        stream.Close();
    }

    public static EnemyData LoadEnemyData()
    {
        string path = Application.persistentDataPath + "/enemyData.data";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            List<EnemyVariables> savedEnemies = formatter.Deserialize(stream) as List<EnemyVariables>;
            stream.Close();

            // Check if there are any saved enemies
            if (savedEnemies != null && savedEnemies.Count > 0)
            {
                EnemyData enemyData = ScriptableObject.CreateInstance<EnemyData>();

                foreach (var savedEnemy in savedEnemies)
                {
                    // Check if the saved enemy has died since the last save
                    if (savedEnemy.ifHasDied)
                    {
               
                        if (!WasEnemyAliveInLastSave(savedEnemy))
                        {
                            GameObject enemyPrefab = EnemyManager.instance.GetEnemyPrefab(savedEnemy.enemyType);
                            if (enemyPrefab != null)
                            {
                                GameObject newEnemy = GameObject.Instantiate(enemyPrefab, savedEnemy.enemyObject.transform.position, Quaternion.identity);
                           
                            }
                        }
                    }
                }

                return enemyData;
            }
            else
            {
                Debug.LogWarning("No saved enemy data found!");
                return null;
            }
        }
        else
        {
            Debug.LogWarning("No enemy data file found!");
            return null;
        }
    }

    private static bool WasEnemyAliveInLastSave(EnemyVariables enemy)
    {
 
        return false;
    }
}


