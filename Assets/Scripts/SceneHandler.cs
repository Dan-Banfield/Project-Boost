using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneHandler
{
    public static void LoadScene(string sceneName)
    {
        //If the scene can be loaded, it exists.
        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            SceneManager.LoadScene(sceneName);
            return;
        }
        Debug.LogError($"Scene '{sceneName}' does not exist!");
    }
}