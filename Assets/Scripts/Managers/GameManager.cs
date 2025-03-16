using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region GameManager Components

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject loseMenu;
    [SerializeField] private GameObject finishMenu;
    [SerializeField] private Button nextLevelButton;

    [SerializeField] private TextMeshProUGUI timerText;

    [SerializeField] private Image medalImage;

    [SerializeField] private Sprite goldMedal;
    [SerializeField] private Sprite silverMedal;
    [SerializeField] private Sprite bronzeMedal;

    #endregion

    private bool paused = false;

    private void Awake()
    {
        //Assign the timer text element to the variable in TimerManager.
        TimerManager.Instance.SetTimerTextComponent(timerText);
    }

    private void Update()
    {
        //Check for the escape key to open pause menu.
        if (Input.GetKeyDown(KeyCode.Escape)) { TogglePauseMenu(); }
    }

    public void TogglePauseMenu()
    {
        paused = !paused;

        switch (paused)
        {
            case true:
                //Freeze in-game time when pause menu active.
                Time.timeScale = 0f;
                pauseMenu.SetActive(true);
                break;
            case false:
                Time.timeScale = 1f;
                pauseMenu.SetActive(false);
                break;
        }
    }

    public void ReturnToMenu()
    {
        //Re-enable time and reset timer for next levels.
        Time.timeScale = 1f;

        TimerManager.Instance.ResetTimer();
        SceneHandler.LoadScene("MainMenu");
    }

    public void RetryLevel()
    {
        Time.timeScale = 1f;

        TimerManager.Instance.ResetTimer();
        SceneHandler.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadNextLevel()
    {
        // Get the current scene name or index
        string currentScene = SceneManager.GetActiveScene().name;

        // Fetch level data from SaveManager
        LevelInfo currentLevelInfo = SaveManager.GetLevelInfo(currentScene);

        if (currentLevelInfo != null)
        {
            //Get the next level's number.
            int nextLevelNumber = currentLevelInfo.LevelNumber + 1;
            //Fetch the level data.
            LevelData data = SaveManager.LoadGame();
            //Find the next level in the list.
            LevelInfo nextLevelInfo = data.Levels.Find(l => l.LevelNumber == nextLevelNumber);

            if (nextLevelInfo != null && nextLevelInfo.IsUnlocked)
            {
                // Load the next scene based on the level's scene name
                SceneHandler.LoadScene(nextLevelInfo.SceneName);
                TimerManager.Instance.ResetTimer();
            }
            else
            {
                //Validation.
                Debug.LogWarning("Next level is locked or does not exist.");
            }
        }
        else
        {
            Debug.LogWarning("Current level not found in the level data.");
        }

        Time.timeScale = 1f;
    }

    public void LoseLevel()
    {
        Time.timeScale = 0f;
        loseMenu.SetActive(true);
    }

    public void FinishLevel(float elapsedTime)
    {
        Time.timeScale = 0f;
        TimerManager.Instance.StopTimer();
        finishMenu.SetActive(true);

        // Get the active level's medal goals.
        string currentScene = SceneManager.GetActiveScene().name;
        LevelInfo levelInfo = SaveManager.GetLevelInfo(currentScene); // Fetch stored level data.

        if (levelInfo != null)
        {
            string medal = levelInfo.GetMedal(elapsedTime);

            if (medal == "Gold" || medal == "Silver" || medal == "Bronze")
                UnlockNextLevel(levelInfo.LevelNumber);

            //Re-enable in case disable by previous level.
            medalImage.enabled = true;

            switch (medal)
            {
                case "Gold":
                    medalImage.sprite = goldMedal;
                    break;
                case "Silver":
                    medalImage.sprite = silverMedal;
                    break;
                case "Bronze":
                    medalImage.sprite = bronzeMedal;
                    break;
                default:
                    medalImage.enabled = false;
                    break;
            }
        }

        //Check if next level is unlocked and enable/disable the button accordingly.
        int nextLevelNumber = levelInfo.LevelNumber + 1;
        LevelData data = SaveManager.LoadGame();

        if (data.IsLevelUnlocked(nextLevelNumber))
        {
            nextLevelButton.interactable = true;
        }
        else
        {
            nextLevelButton.interactable = false;
        }
    }

    private void UnlockNextLevel(int currentLevelNumber)
    {
        int nextLevelNumber = currentLevelNumber + 1;

        // Load level data
        LevelData data = SaveManager.LoadGame();

        // Ensure the next level exists in the saved data
        LevelInfo nextLevelInfo = data.Levels.Find(level => level.LevelNumber == nextLevelNumber);

        // Check if the next level is valid and exists in the build
        if (nextLevelInfo != null && Application.CanStreamedLevelBeLoaded(nextLevelInfo.SceneName))
        {
            if (!data.IsLevelUnlocked(nextLevelNumber))
            {
                data.UnlockLevel(nextLevelNumber);
                SaveManager.SaveGame(data);
            }
        }
    }
}