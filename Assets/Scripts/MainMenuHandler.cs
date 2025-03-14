using UnityEditor;
using UnityEngine;

public class MainMenuHandler : MonoBehaviour
{
    public void OnPlayButtonClick()
        => SceneHandler.LoadScene("LevelSelectMenu");

    public void OnOptionsButtonClick()
        => SceneHandler.LoadScene("OptionsMenu");

    public void OnQuitButtonClick()
    {
        //Stops the editor from playing the game too when quit is pressed.
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}