using UnityEngine;

public class MainMenuHandler : MonoBehaviour
{
    public void OnPlayButtonClick()
        => SceneHandler.LoadScene("LevelSelectMenu");

    public void OnOptionsButtonClick()
        => SceneHandler.LoadScene("OptionsMenu");

    public void OnQuitButtonClick()
        => Application.Quit();
}