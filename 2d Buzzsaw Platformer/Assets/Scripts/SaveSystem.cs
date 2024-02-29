using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveSystem
{
    public static string saveFilePath = Application.persistentDataPath + "/playerProfile.save";//creates path pointing to whatever folder the game is in
    public static void SavePlayerData(GameController gameController)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(saveFilePath, FileMode.Create);//creates a file in location 'saveFilePath'

        PlayerData saveData = new PlayerData(gameController);

        formatter.Serialize(stream, saveData);//using 'stream' filestream, creates a file in the path 'saveFilePath' containing the data in 'saveData', encrypred in binary
        stream.Close();
    }

    public static PlayerData LoadPlayerData()
    {
        if (File.Exists(saveFilePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(saveFilePath, FileMode.Open);//opens data from file in location 'saveFilePath'

            PlayerData saveData = formatter.Deserialize(stream) as PlayerData;// using stream 'filestream', returns data from file in location 'saveFilePath' as type 'PlayerData'
            stream.Close();
            return saveData;
        }
        else
        {
            Debug.LogError("Save file not found in " + saveFilePath);
            return null;
        }
    }
}
