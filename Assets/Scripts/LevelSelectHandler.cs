using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectUI : MonoBehaviour
{
    [SerializeField] private Transform levelButtonParent; //Parent object for level buttons.
    [SerializeField] private GameObject levelButtonPrefab; //Button prefab to instantiate for each level.
    private LevelData levelData;

    void Start()
    {
        levelData = SaveManager.LoadGame();
        GenerateLevelButtons();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) { SceneHandler.LoadScene("MainMenu"); }
    }

    private void GenerateLevelButtons()
    {
        //Clear existing buttons to prevent duplicates.
        foreach (Transform child in levelButtonParent)
        {
            Destroy(child.gameObject);
        }

        //Instantiate a button for each level.
        foreach (LevelInfo level in levelData.Levels)
        {
            //Create GameObject for prefab.
            GameObject buttonObj = Instantiate(levelButtonPrefab, levelButtonParent);

            //Get button and text components from prefab.
            Button button = buttonObj.GetComponent<Button>();
            TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();

            //Set button text with level details.
            buttonText.text = $"Level {level.LevelNumber}\nGold: {level.GoldTime}s\nSilver: {level.SilverTime}s\nBronze: {level.BronzeTime}s";

            if (level.IsUnlocked)
            {
                int levelNumber = level.LevelNumber;
                string sceneName = level.SceneName;
                button.onClick.AddListener(() => LoadLevel(sceneName));
            }
            else
            {
                button.interactable = false; //Disable button for locked levels.
            }
        }
    }

    private void LoadLevel(string sceneName)
    {
        SceneHandler.LoadScene(sceneName);
    }
}