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
            SetDefaultKeybinds();
            LoadKeybinds();
            return;
        }
        Destroy(gameObject);
    }

    public void SetKeybind(string action, KeyCode key)
    {
        //Set the dictionary storing the current keybinds.
        keybinds[action] = key;
        
        PlayerPrefs.SetString(action, key.ToString());
        PlayerPrefs.Save();

        //Call the event.
        OnKeybindsUpdated?.Invoke();
    }

    //Validation to check if the keybind exists, if not return an empty key.
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
            }
            else
            {
                //If no PlayerPref found, return None keybind.
                keybinds[action] = KeyCode.None;
            }
        }
    }

    private void SetDefaultKeybinds()
    {
        bool defaultsSet = false;

        foreach (string action in actions)
        {
            if (!PlayerPrefs.HasKey(action))
            {
                keybinds[action] = GetDefaultKeybind(action);
                PlayerPrefs.SetString(action, keybinds[action].ToString());
                defaultsSet = true;  //Flag that defaults were set.
            }
        }

        if (defaultsSet)
        {
            PlayerPrefs.Save();  //Only save once if we set defaults.
        }
    }

    private KeyCode GetDefaultKeybind(string action)
    {
        return action switch
        {
            "Thrust" => KeyCode.Space,
            "RotateLeft" => KeyCode.A,
            "RotateRight" => KeyCode.D,
            _ => KeyCode.None
        };
    }
}