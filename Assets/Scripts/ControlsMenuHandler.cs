using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class ControlsMenuHandler : MonoBehaviour
{
    #region Components

    [SerializeField] private TextMeshProUGUI thrustBindingButton;
    [SerializeField] private TextMeshProUGUI rotateLeftBindingButton;
    [SerializeField] private TextMeshProUGUI rotateRightBindingButton;

    #endregion

    private string waitingForKey = null;

    private void Start()
    {
        UpdateButtonLabels();
        //Set the event action to update the text for buttons.
        KeybindManager.Instance.OnKeybindsUpdated += UpdateButtonLabels;
    }

    private void UpdateButtonLabels()
    {
        thrustBindingButton.text = KeybindManager.Instance.GetKeybind("Thrust").ToString();
        rotateLeftBindingButton.text = KeybindManager.Instance.GetKeybind("RotateLeft").ToString();
        rotateRightBindingButton.text = KeybindManager.Instance.GetKeybind("RotateRight").ToString();
    }

    public void StartRebind(string action) => StartCoroutine(WaitForKeyPress(action));

    private IEnumerator WaitForKeyPress(string action)
    {
        waitingForKey = action;
        TextMeshProUGUI buttonText = GetButtonForAction(action);
        buttonText.text = "Press any key...";

        while (!Input.anyKeyDown) { yield return null; }

        foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(key))
            {
                KeybindManager.Instance.SetKeybind(action, key);
                waitingForKey = null;
                yield break;
            }
        }
    }

    private TextMeshProUGUI GetButtonForAction(string action)
    {
        return action switch
        {
            "Thrust" => thrustBindingButton,
            "RotateLeft" => rotateLeftBindingButton,
            "RotateRight" => rotateRightBindingButton,
            _ => null
        };
    }
}