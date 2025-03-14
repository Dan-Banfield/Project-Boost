using UnityEditor;
using UnityEngine;
using System.IO;

public class DeleteSaveFileEditor : EditorWindow
{
    [MenuItem("Tools/Delete Save File")]
    public static void DeleteSaveFile()
    {
        // Get the path of the save file
        string saveFilePath = Path.Combine(Application.persistentDataPath, "save.dat");

        // Check if the file exists
        if (File.Exists(saveFilePath))
        {
            // Delete the file
            File.Delete(saveFilePath);
            Debug.Log("Save file deleted successfully: " + saveFilePath);
        }
        else
        {
            Debug.LogWarning("Save file not found: " + saveFilePath);
        }
    }
}
