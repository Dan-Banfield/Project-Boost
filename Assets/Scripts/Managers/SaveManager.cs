using MessagePack;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveManager
{
    private static string saveFilePath => Path.Combine(Application.persistentDataPath, "save.dat");

    public static void SaveGame(LevelData levelData)
    {
        try
        {
            //Serialize level data using message pack and store it in a file.
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
        //Validation if save file exists.
        if (File.Exists(saveFilePath))
        {
            try
            {
                //Read bytes and use message pack to desrialize to level data.
                byte[] data = File.ReadAllBytes(saveFilePath);
                return MessagePackSerializer.Deserialize<LevelData>(data);
            }
            catch (System.Exception e)
            {
                Debug.LogError("Load failed: " + e.Message);
            }
        }

        LevelData newGame = new LevelData
        {
            //Create default levels if no save file exists.
            Levels = new List<LevelInfo>
            {
                new LevelInfo(1, "Level1Scene", 30f, 20f, 10f, true),
                new LevelInfo(2, "Level2Scene", 30f, 20f, 10f),
                new LevelInfo(3, "Level3Scene", 30f, 20f, 10f),
                new LevelInfo(4, "Level4Scene", 30f, 20f, 10f),
                new LevelInfo(5, "Level5Scene", 30f, 20f, 10f),
                new LevelInfo(6, "Level6Scene", 30f, 20f, 10f),
                new LevelInfo(7, "Level7Scene", 30f, 20f, 10f),
                new LevelInfo(8, "Level8Scene", 30f, 20f, 10f),
                new LevelInfo(9, "Level9Scene", 30f, 20f, 10f),
                new LevelInfo(10, "Level10Scene", 30f, 20f, 10f)
            }
        };

        SaveGame(newGame);
        return newGame;
    }

    //Return details for a level by searching the list based on scene name.
    public static LevelInfo GetLevelInfo(string sceneName)
    {
        LevelData data = LoadGame();
        return data.Levels.Find(level => level.SceneName == sceneName);
    }
}