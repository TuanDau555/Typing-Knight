using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [Header("Managers")]
    [SerializeField] private TimerManager timer;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private ContentProvider contentProvider;

    [Header("UI Panels")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject gameWinPanel;
    [SerializeField] private UIPause pausePanel;

    [Header("New UI Manager")]
    [SerializeField] private UIWinResult winResult; // Kéo script WinResultUI vào đây
    [SerializeField] private Wall wall;               // Kéo Wall vào đây

    [Header("Story")]
    [SerializeField] private LevelStoryData levelStory;
    
    private bool isGameRunning = false;
    public bool IsGameEnd => !isGameRunning;
    private bool isPaused = false;
    public bool IsPaused => isPaused;
    public float ElapsedTime => timer != null ? timer.ElapsedTime : 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void Start()
    {
        HideAllPanels();
        Time.timeScale = 0f;

        // Khởi tạo các manager
        timer?.Reset();
        scoreManager?.Reset();
 
        // Intro
        if (levelStory != null && levelStory.introLines.Length > 0)
        {
            DialogueManager.Instance.StartDialogue(levelStory.introLines, OnIntroComplete);
        }
        else
        {
            OnIntroComplete();
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            if (isGameRunning) 
            {
                TogglePause();
            }
        }
    }
    private void OnIntroComplete()
    {
        Time.timeScale = 1f;
        timer?.Reset();
        timer?.StartTimer();
        isGameRunning = true;
    }
    public void SetCurrentPhase (Gamephase currentPhase)
    {
        List<Gamephase> phaselist = new List<Gamephase>() { currentPhase };
        contentProvider?.SetActiveTopics(phaselist);
    }
    public void TriggerGameWin()
    {
        if (!isGameRunning) return;
        isGameRunning = false;
        timer?.StopTimer();
        Time.timeScale = 0f;

        if (levelStory != null && levelStory.outroLines.Length > 0)
        {
            DialogueManager.Instance.StartDialogue(levelStory.outroLines, ShowWinPanel);
        }
        else
        {
            ShowWinPanel();
        }
    }
    public void TriggerGameOver()
    {
        if (!isGameRunning) return;
        isGameRunning = false;
        timer?.StopTimer();
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
    }
    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;
            int score = scoreManager != null ? scoreManager.Score : 0;
            int wrong = scoreManager != null ? scoreManager.WrongCount : 0;
            float time = timer != null ? timer.ElapsedTime : 0;

            pausePanel.Show(score, wrong, time);
        }
        else
        {
            Time.timeScale = 1f;
            pausePanel.Hide();
        }
    }

    // Hàm dùng cho nút "Resume" trên UI
    public void ResumeGame()
    {
        if (isPaused) TogglePause();
    }

    // Hàm dùng cho nút "Quit" hoặc về Menu
    public void LoadMainMenu(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }

    private void ShowWinPanel()
    {
        int score = scoreManager != null ? scoreManager.Score : 0;
        int wrong = scoreManager != null ? scoreManager.WrongCount : 0;
        float time = timer != null ? timer.ElapsedTime : 0;
        bool isCountdown = timer != null && timer.IsCountdownMode;
        float hpPercent = wall != null ? wall.GetHpPercent() : 0;

        winResult.Show(score, wrong, time, hpPercent, isCountdown);
    }

    private void HideAllPanels()
    {
        if (gameOverPanel) gameOverPanel.SetActive(false);
        if (gameWinPanel) gameWinPanel.SetActive(false);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void AddScore(int amount) => scoreManager?.AddScore(amount);
    public void AddWrong(int amount = 1) => scoreManager?.AddWrong(amount);
    public string GetNextContent() => contentProvider?.GetRandomContent() ?? "?";
}