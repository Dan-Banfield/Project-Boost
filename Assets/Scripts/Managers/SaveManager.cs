using System.IO;
using UnityEngine;
using MessagePack;
using System.Collections.Generic;

public static class SaveManager
{
    //Where to save data. Save in a file called "save.dat"
    private static string saveFilePath => Path.Combine(Application.persistentDataPath, "save.dat");

    public static void SaveGame(LevelData levelData)
    {
        try
        {
            //Use MessagePackC# to serialize class to byte[].
            byte[] data = MessagePackSerializer.Serialize(levelData);
            File.WriteAllBytes(saveFilePath, data);
            Debug.Log("Game saved.");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Save failed: " + e.Message);
        }
    }

    public static LevelData LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            try
            {
                //Read data from save file to byte[].
                byte[] data = File.ReadAllBytes(saveFilePath);
                //Deserialize byte[] to LevelData class instance.
                return MessagePackSerializer.Deserialize<LevelData>(data);
            }
            catch (System.Exception e)
            {
                Debug.LogError("Load failed: " + e.Message);
            }
        }

        //Default levels with their scene names and time goals.
        LevelData newGame = new LevelData
        {
            Levels = new List<LevelInfo>
            {
                new LevelInfo(1, "Level1Scene", 60f, 45f, 30f, true),  //Level 1 unlocked by default.
                new LevelInfo(2, "Level2Scene", 60f, 45f, 30f, false),  //Every other level locked by default.
                new LevelInfo(3, "Level3Scene", 60f, 45f, 30f, false),  //Every other level locked by default.
                new LevelInfo(4, "Level4Scene", 60f, 45f, 30f, false),  //Every other level locked by default.
                new LevelInfo(5, "Level5Scene", 60f, 45f, 30f, false),  //Every other level locked by default.
                new LevelInfo(6, "Level6Scene", 60f, 45f, 30f, false),  //Every other level locked by default.
                new LevelInfo(7, "Level7Scene", 60f, 45f, 30f, false),  //Every other level locked by default.
                new LevelInfo(8, "Level8Scene", 60f, 45f, 30f, false),  //Every other level locked by default.
                new LevelInfo(9, "Level9Scene", 60f, 45f, 30f, false),  //Every other level locked by default.
                new LevelInfo(10, "Level10Scene", 60f, 45f, 30f, false),  //Every other level locked by default.
            }
        };

        SaveGame(newGame);
        return newGame;
    }
}