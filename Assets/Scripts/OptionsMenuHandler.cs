using UnityEngine;

public class OptionsMenuHandler : MonoBehaviour
{
    public void OnControlsButtonClick()
        => SceneHandler.LoadScene("ControlsMenu");
}
