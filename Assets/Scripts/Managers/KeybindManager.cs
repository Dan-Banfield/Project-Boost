using System;
using UnityEngine;
using System.Collections.Generic;

public class KeybindManager : MonoBehaviour
{
    //Holds the singleton isntance so that only one class can be instantiated.
    public static KeybindManager Instance;

    //Dictionary used to hold the keybinds, name of action corresponds to keybind.
    private Dictionary<string, KeyCode> keybinds = new Dictionary<string, KeyCode>();
    //Actions available to the user. Can be expanded.
    private string[] actions = { "Thrust", "RotateLeft", "RotateRight" };

    //Update the UI when the event is called.
    public event Action OnKeybindsUpdated;

    private void Awake()
    {
        //If no instance exists, create a singleton and prevent it from being destroyed.
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            //Initialise dictionary with values stored in PlayerPrefs.
            LoadKeybinds();
            return;
        }
        Destroy(gameObject);
    }

    public void SetKeybind(string action, KeyCode key)
    {
        keybinds[action] = key;

        PlayerPrefs.SetString(action, key.ToString());
        PlayerPrefs.Save();

        OnKeybindsUpdated?.Invoke();
    }

    public KeyCode GetKeybind(string action) => keybinds.ContainsKey(action) ? keybinds[action] : KeyCode.None;

    private void LoadKeybinds()
    {
        //Actions available to the player.
        foreach (string action in actions)
        {
            if (PlayerPrefs.HasKey(action))
            {
                //Use enum.Parse to convert the string representation of the key code in PlayerPrefs to an actual enum.
                keybinds[action] = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(action));
                return;
            }
            //If no PlayerPref found, return None keybind.
            keybinds[action] = KeyCode.None;
        }
    }
}
