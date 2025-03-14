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
            Levels = new List<LevelInfo>
            {
                new LevelInfo(1, "Level1Scene", 60f, 45f, 30f, true),
                new LevelInfo(2, "Level2Scene", 60f, 45f, 30f),
                new LevelInfo(3, "Level3Scene", 60f, 45f, 30f),
                new LevelInfo(4, "Level4Scene", 60f, 45f, 30f),
                new LevelInfo(5, "Level5Scene", 60f, 45f, 30f),
                new LevelInfo(6, "Level6Scene", 60f, 45f, 30f),
                new LevelInfo(7, "Level7Scene", 60f, 45f, 30f),
                new LevelInfo(8, "Level8Scene", 60f, 45f, 30f),
                new LevelInfo(9, "Level9Scene", 60f, 45f, 30f),
                new LevelInfo(10, "Level10Scene", 60f, 45f, 30f)
            }
        };

        SaveGame(newGame);
        return newGame;
    }

    public static LevelInfo GetLevelInfo(string sceneName)
    {
        LevelData data = LoadGame();
        return data.Levels.Find(level => level.SceneName == sceneName);
    }
}