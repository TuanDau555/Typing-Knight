/* using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements.Experimental;
using UnityEditor.AdaptivePerformance.Editor;
using Unity.VisualScripting;
using System.Collections.Generic;


[System.Serializable]
public class GameTopic
{
    public string topicName;
    [Header("chon che do")]
    [SerializeField] public bool isWordMode;

    [Header("data")]
    [TextArea(3, 5)]
    [SerializeField] public string charSet;
    [SerializeField] public System.Collections.Generic.List<string> wordList;
}
public class GameManagerEndless : MonoBehaviour
{
    public static GameManagerEndless Instance;
    [Header("UI Score & Wrong")]
    [SerializeField] private TextMeshProUGUI wrongKeyText;
    [SerializeField] private TextMeshProUGUI scoreKeyText;

    [Header("Ui Over")]
    [SerializeField] private GameObject gameOverUI;

    [Header("setting time")]
    [SerializeField] private TextMeshProUGUI timerText;
    [Tooltip("time will ending")]
    [SerializeField] private float timeDuration = 180f;

    [Header("game setting")]
    [SerializeField] public List<GameTopic> allTopics;
    [SerializeField] public int currentTopicIndex = 0;

    public float CurrentTime { get; private set; }
    public bool isGameOver = false;
    public bool isGameEnd = false;

    private int wrong = 0;
    private int score = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isGameOver = false ;
        Time.timeScale = 1;
        CurrentTime = 0;
        UpdateUI();
        gameOverUI.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver) return;
        if (isGameEnd) return;

        CurrentTime += Time.deltaTime;
        UpdateTimerUI();
        if (CurrentTime >= timeDuration)
        {
            CurrentTime = timeDuration;
            UpdateTimerUI();
            Debug.Log("time end - you win");
        }
    }


    public string GetRandomContent()
    {
        // Lấy chủ đề hiện tại
        GameTopic currentTopic = allTopics[currentTopicIndex];

        if (currentTopic.isWordMode)
        {
            // CHẾ ĐỘ TỪ VỰNG: Lấy ngẫu nhiên 1 từ trong list
            if (currentTopic.wordList.Count > 0)
            {
                int rd = Random.Range(0, currentTopic.wordList.Count);
                return currentTopic.wordList[rd];
            }
            return "Null";
        }
        else
        {
            // CHẾ ĐỘ KÝ TỰ: Lấy ngẫu nhiên 1 chữ cái trong chuỗi
            if (!string.IsNullOrEmpty(currentTopic.charSet))
            {
                int rd = Random.Range(0, currentTopic.charSet.Length);
                return currentTopic.charSet[rd].ToString();
            }
            return "A";
        }
    }
    public void AddWrong (int amount)
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
        if (wrongKeyText != null) wrongKeyText.text = "Wrong:" + wrong.ToString();
        if (scoreKeyText != null) scoreKeyText.text = "Score:" + score.ToString();
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(CurrentTime / 60);
            int seconds = Mathf.FloorToInt(CurrentTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    public void GameOver()
    {
        isGameOver = true;
        isGameEnd = true;
        Time.timeScale = 0;
        gameOverUI.SetActive(true);
    }

    public void RestarGame()
    {
        isGameEnd = false;
        isGameOver = false;
        score = 0;
        CurrentTime = 0;
        Time.timeScale = 1;
        UpdateUI();
        SceneManager.LoadScene("endless");
        gameOverUI.SetActive(false);
    }
    public bool IsGameEnd()
    {
        return !isGameEnd;
    }
}

*/
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

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

    [Header("UI Over")]
    [SerializeField] private GameObject gameOverUI;

    [Header("Setting time")]
    [SerializeField] private TextMeshProUGUI timerText;
    [Tooltip("Thời gian kết thúc game (giây)")]
    [SerializeField] private float timeDuration = 180f;

    [Header("Game setting")]
    [SerializeField] public List<GameTopic> allTopics;  // Danh sách tất cả các chủ đề

    [Header("Chọn chủ đề để chơi (nhập index, có thể chọn nhiều)")]
    [SerializeField] public List<int> selectedTopicIndices = new List<int>();

    [Tooltip("Nếu không chọn topic nào, tự động dùng hết tất cả topics")]
    [SerializeField] private bool fallbackToAllIfNoneSelected = true;

    public float CurrentTime { get; private set; }
    public bool isGameOver = false;
    public bool isGameEnd = false;

    private int wrong = 0;
    private int score = 0;

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
        isGameEnd = false;
        Time.timeScale = 1;
        CurrentTime = 0;
        UpdateUI();
        gameOverUI.SetActive(false);
    }

    void Update()
    {
        if (isGameOver || isGameEnd) return;

        CurrentTime += Time.deltaTime;
        UpdateTimerUI();

        if (CurrentTime >= timeDuration)
        {
            CurrentTime = timeDuration;
            UpdateTimerUI();
            Debug.Log("Hết thời gian - Bạn thắng!");
            // Nếu muốn kết thúc game khi hết giờ, gọi GameOver() ở đây
            // GameOver();
        }
    }

    public string GetRandomContent()
    {
        if (allTopics == null || allTopics.Count == 0)
        {
            Debug.LogWarning("Không có topic nào được thiết lập!");
            return "ERROR_NO_TOPICS";
        }

        List<GameTopic> activeTopics = GetActiveTopics();

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

    private List<GameTopic> GetActiveTopics()
    {
        List<GameTopic> active = new List<GameTopic>();

        // Nếu không chọn topic nào và bật fallback → dùng hết
        if (selectedTopicIndices.Count == 0 && fallbackToAllIfNoneSelected)
        {
            return new List<GameTopic>(allTopics);
        }

        // Chỉ lấy các topic có index hợp lệ
        foreach (int index in selectedTopicIndices)
        {
            if (index >= 0 && index < allTopics.Count)
            {
                active.Add(allTopics[index]);
            }
            else
            {
                Debug.LogWarning($"Index topic không hợp lệ: {index}");
            }
        }

        return active;
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
            int minutes = Mathf.FloorToInt(CurrentTime / 60);
            int seconds = Mathf.FloorToInt(CurrentTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    public void GameOver()
    {
        isGameOver = true;
        isGameEnd = true;
        Time.timeScale = 0;
        gameOverUI.SetActive(true);
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