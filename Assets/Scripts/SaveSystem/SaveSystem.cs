using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

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
}