using MessagePack;
using System.Collections.Generic;

[MessagePackObject]
public class LevelData
{
    //Store levels in List to be serialized.
    [Key(0)] public List<LevelInfo> Levels { get; set; } = new List<LevelInfo>();

    //LINQ expression to match the pattern of a level being unlocked.
    public bool IsLevelUnlocked(int level)
    {
        LevelInfo levelInfo = Levels.Find(l => l.LevelNumber == level);
        return levelInfo != null && levelInfo.IsUnlocked;
    }

    //Change level state to unlocked.
    public void UnlockLevel(int level)
    {
        LevelInfo levelInfo = Levels.Find(l => l.LevelNumber == level);
        if (levelInfo != null) levelInfo.IsUnlocked = true;
    }

    //Get info for a level based on its scene name.
    public LevelInfo GetLevelInfo(string sceneName)
    {
        return Levels.Find(level => level.SceneName == sceneName);
    }
}

//Serializeable attribute for MessagePack.
[MessagePackObject]
public class LevelInfo
{
    [Key(0)] public int LevelNumber { get; set; }
    [Key(1)] public string SceneName { get; set; }
    [Key(2)] public bool IsUnlocked { get; set; }
    [Key(3)] public float BronzeTime { get; set; }
    [Key(4)] public float SilverTime { get; set; }
    [Key(5)] public float GoldTime { get; set; }

    public LevelInfo() { }

    public LevelInfo(int levelNumber, string sceneName, float bronze, float silver, float gold, bool unlocked = false)
    {
        LevelNumber = levelNumber;
        SceneName = sceneName;
        BronzeTime = bronze;
        SilverTime = silver;
        GoldTime = gold;
        IsUnlocked = unlocked;
    }

    //Get the medal the player won from data.
    public string GetMedal(float playerTime)
    {
        if (playerTime <= GoldTime) return "Gold";
        if (playerTime <= SilverTime) return "Silver";
        if (playerTime < BronzeTime) return "Bronze";

        return "None";
    }
}