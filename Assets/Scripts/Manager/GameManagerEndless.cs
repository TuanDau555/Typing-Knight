using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.Events;
[System.Serializable]

public class GameTopic
{
    public string topicName;
    [Header("Chọn chế độ")]
    [SerializeField] public bool isWordMode;
    [Header("Data")]
    [TextArea(3, 5)]
    [SerializeField] public string charSet;
    [SerializeField] public List<string> wordList;
}
public class GameManagerEndless : MonoBehaviour
{
    public static GameManagerEndless Instance;
    [Header("UI Score & Wrong")]
    [SerializeField] private TextMeshProUGUI wrongKeyText;
    [SerializeField] private TextMeshProUGUI scoreKeyText;
    [Header("UI Over & Win")]
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject gameWinUI;
    [Header("Timer Settings")]
    [SerializeField] private TextMeshProUGUI timerText;
    [Tooltip("Thời gian mục tiêu (giây) - dùng cho đếm ngược hoặc giới hạn đếm lên")]
    [SerializeField] private float targetTime = 180f; // 3 phút mặc định
    [Header("Timer Mode")]
    [Tooltip("true = Đếm ngược (từ targetTime về 0 → Win), false = Đếm lên (từ 0 lên vô hạn hoặc đến targetTime)")]
    [SerializeField] private bool isCountdownMode = true; // Chế độ mặc định: đếm ngược
    [Header("Game setting")]
    [SerializeField] public List<GameTopic> allTopics;
    [SerializeField] public List<int> selectedTopicIndices = new List<int>();
    [SerializeField] private bool fallbackToAllIfNoneSelected = true;

    [Header("Narrative")]
    [SerializeField] private LevelStoryData currentLevelStory;
    public UnityEvent onLevelStart;   // optional
    public UnityEvent onLevelComplete;
    public float CurrentTime { get; private set; }
    public float elapsedTime { get; private set; } = 0f;
    public bool isGameOver = false;
    public bool isGameWin = false;
    public bool isGameEnd = false;
    private int wrong = 0;
    private int score = 0;
    private Gamephase currentPhase;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        isGameOver = false;
        isGameWin = false;
        isGameEnd = false;

        // NGAY TỪ ĐẦU: Pause toàn bộ game để intro chạy
        Time.timeScale = 0f;

        CurrentTime = targetTime;
        elapsedTime = 0f;
        UpdateUI();
        UpdateTimerUI();
        gameOverUI.SetActive(false);
        gameWinUI.SetActive(false);

        // Chạy intro (nếu có)
        if (currentLevelStory != null && currentLevelStory.introLines.Length > 0)
        {
            DialogueManager.Instance.StartDialogue(currentLevelStory.introLines, OnIntroFinished);
        }
        else
        {
            // Không có intro → bắt đầu luôn (hiếm xảy ra)
            OnIntroFinished();
        }
    }
    private void OnIntroFinished()
    {
        // Bây giờ mới chạy game thật
        Time.timeScale = 1f;

        // Reset lại elapsedTime để timer bắt đầu từ đầu (nếu cần)
        elapsedTime = 0f;
        CurrentTime = targetTime;
        UpdateTimerUI();

        // Bật spawn enemy, player input, v.v.
        StartTimerAndGameplay();
    }
    private void StartTimerAndGameplay()
    {
        // Bật player controller, spawn enemy, timer chạy...
        // Ví dụ: EnablePlayerInput(); SpawnManager.StartSpawning();
        UpdateTimerUI();
        onLevelStart?.Invoke();
    }
    void Update()
    {
        if (isGameOver || isGameEnd) return;
        // Thời gian đã trôi qua luôn tăng
        elapsedTime += Time.deltaTime;
        // Cập nhật CurrentTime theo chế độ
        if (isCountdownMode)
        {
            CurrentTime = targetTime - elapsedTime;
            if (CurrentTime <= 0)
            {
                CurrentTime = 0;
                GameWin(); // Đếm ngược hết → thắng
            }
        }
        else
        {
            CurrentTime = elapsedTime; // Đếm lên bình thường
            // Optional: Nếu muốn thắng khi đạt targetTime (ví dụ 180s)
            // if (CurrentTime >= targetTime) GameWin();
        }
        UpdateTimerUI();
    }
    // Hàm để chuyển chế độ runtime (gọi từ button hoặc script khác nếu cần)
    public void SetCountdownMode(bool countdown)
    {
        isCountdownMode = countdown;
        Debug.Log("Chuyển chế độ timer: " + (countdown ? "Đếm ngược" : "Đếm lên"));
        // Reset thời gian khi chuyển chế độ (tùy ý bạn)
        if (countdown)
            CurrentTime = targetTime;
        else
            CurrentTime = 0f;
        UpdateTimerUI();
    }
    public void SetCurrentPhase(Gamephase phase)
    {
        currentPhase = phase;
        Debug.Log("Phase hiện tại đã được cập nhật: startTime = " + (phase != null ? phase.startTime : -1));
    }
    public string GetRandomContent()
    {
        if (allTopics == null || allTopics.Count == 0)
        {
            Debug.LogWarning("Không có topic nào được thiết lập!");
            return "ERROR_NO_TOPICS";
        }
        List<GameTopic> activeTopics = GetActiveTopicsForCurrentPhase();
        if (activeTopics.Count == 0)
        {
            Debug.LogWarning("Không có topic nào được chọn!");
            return "ERROR_NO_ACTIVE_TOPICS";
        }
        // Chọn ngẫu nhiên một topic từ các topic đã chọn
        int randomTopicIdx = Random.Range(0, activeTopics.Count);
        GameTopic selectedTopic = activeTopics[randomTopicIdx];
        if (selectedTopic.isWordMode)
        {
            if (selectedTopic.wordList != null && selectedTopic.wordList.Count > 0)
            {
                int rd = Random.Range(0, selectedTopic.wordList.Count);
                return selectedTopic.wordList[rd];
            }
            Debug.LogWarning("Topic từ vựng không có từ nào: " + selectedTopic.topicName);
            return "NULL_WORD";
        }
        else
        {
            if (!string.IsNullOrEmpty(selectedTopic.charSet))
            {
                int rd = Random.Range(0, selectedTopic.charSet.Length);
                return selectedTopic.charSet[rd].ToString();
            }
            Debug.LogWarning("Topic ký tự không có ký tự nào: " + selectedTopic.topicName);
            return "A";
        }
    }
    private List<GameTopic> GetActiveTopicsForCurrentPhase()
    {
        if (currentPhase != null && currentPhase.selectedTopicIndices.Count > 0)
        {
            List<GameTopic> phaseTopics = new List<GameTopic>();
            foreach (int index in currentPhase.selectedTopicIndices)
            {
                if (index >= 0 && index < allTopics.Count)
                {
                    phaseTopics.Add(allTopics[index]);
                }
            }
            return phaseTopics;
        }
        // Fallback
        if (fallbackToAllIfNoneSelected)
        {
            return new List<GameTopic>(allTopics);
        }
        return new List<GameTopic>();
    }
    public void AddWrong(int amount)
    {
        wrong += amount;
        UpdateUI();
    }
    public void AddScore(int amount)
    {
        score += amount;
        UpdateUI();
    }
    private void UpdateUI()
    {
        if (wrongKeyText != null) wrongKeyText.text = "Wrong: " + wrong.ToString();
        if (scoreKeyText != null) scoreKeyText.text = "Score: " + score.ToString();
    }
    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            float displayTime = Mathf.Max(0, CurrentTime);
            int minutes = Mathf.FloorToInt(displayTime / 60);
            int seconds = Mathf.FloorToInt(displayTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
    public void GameWin()
    {
        isGameWin = true;
        isGameEnd = true;
        Time.timeScale = 0;
        // 1. Chơi outro dialogue
        if (currentLevelStory != null && currentLevelStory.outroLines.Length > 0)
        {
            DialogueManager.Instance.StartDialogue(currentLevelStory.outroLines, () =>
            {
                // Outro xong → chuyển màn tiếp theo hoặc win game
                gameWinUI.SetActive(true); // hoặc LoadNextLevel();
            });
        }
        else
        {
            gameWinUI.SetActive(true);
        }
    }
    public void GameOver()
    {
        isGameOver = true;
        isGameEnd = true;
        Time.timeScale = 0;
        gameOverUI.SetActive(true);
        gameWinUI.SetActive(false);
    }
    public void RestartGame()
    {
        isGameEnd = false;
        isGameOver = false;
        score = 0;
        wrong = 0;
        CurrentTime = 0;
        Time.timeScale = 1;
        UpdateUI();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gameOverUI.SetActive(false);
    }
    public bool IsGameEnd()
    {
        return isGameEnd;
    }
}