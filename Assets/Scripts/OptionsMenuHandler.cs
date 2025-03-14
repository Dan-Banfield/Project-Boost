using UnityEngine;

public class OptionsMenuHandler : MonoBehaviour
{
    private void Update()
    {
        //Allows the user to go back to the previous menu by clicking ESC.
        if (Input.GetKeyDown(KeyCode.Escape)) { SceneHandler.LoadScene("MainMenu"); }
    }

    public void OnControlsButtonClick()
        => SceneHandler.LoadScene("ControlsMenu");
}
