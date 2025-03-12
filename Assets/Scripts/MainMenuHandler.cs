using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    public void OnPlayButtonClick()
        => LoadScene("LevelSelectScene");

    public void OnOptionsButtonClick()
        => LoadScene("OptionsScene");

    public void OnQuitButtonClick()
        => Application.Quit();

    private void LoadScene(string sceneName)
    {
        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            SceneManager.LoadScene(sceneName);
            return;
        }
        Debug.LogError("Scene doesn't exist!");
    }
}