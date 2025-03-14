using TMPro;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    //Create a singleton class, so make one public instance to access it.
    public static TimerManager Instance { get; private set; }

    //The text component that shows the timer.
    private TextMeshProUGUI timerText;

    //Keep track of timer state.
    private bool isTiming = false;
    private float elapsedTime = 0f;

    private void Awake()
    {
        //Initialise singleton.
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            return;
        }
        Destroy(gameObject);
    }

    private void Update()
    {
        //Only increment timer if active.
        if (isTiming)
        {
            //Works by adding the time between last frames each frame to avoid framerate dependency.
            elapsedTime += Time.deltaTime;
            UpdateTimerUI();
        }
    }

    public void StartTimer()
    {
        if (!isTiming)
        {
            isTiming = true;
            elapsedTime = 0;
        }
    }

    public void StopTimer() => isTiming = false;

    public void ResetTimer()
    {
        isTiming = false;
        elapsedTime = 0f;
    }

    public float GetElapsedTime() => elapsedTime;

    public bool Running() => isTiming;

    public void SetTimerTextComponent(TextMeshProUGUI timerText)
    {
        this.timerText = timerText;
        UpdateTimerUI();
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            //Maths to calculate minutes, seconds and milliseconds from elapsed total.
            int minutes = Mathf.FloorToInt(elapsedTime / 60);
            int seconds = Mathf.FloorToInt(elapsedTime % 60);
            int milliseconds = Mathf.FloorToInt((elapsedTime * 100) % 100);

            //Formatting ensures there's always two 00s, even when less than 10.
            timerText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);

            return;
        }
        timerText.text = "00:00:00";
    }
}