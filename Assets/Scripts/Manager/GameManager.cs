using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("UI Score & Wrong")]
    [SerializeField] private TextMeshProUGUI wrongKeyText;
    [SerializeField] private TextMeshProUGUI scoreKeyText;
    [Header("UI win & Over")]
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject gameWinUI;
    [Header("setting time")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float timeDuration = 180f;

    private float currentTime;
    public bool isGameWin = false;
    public bool isGameOver = false;
    public bool isGameEnd = false;

    private int wrong = 0;
    private int score = 0;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    void Start()
    {
        isGameEnd = false;
        Time.timeScale = 1;
        currentTime = timeDuration;
        UpdateUI();
        gameOverUI.SetActive(false);
        gameWinUI.SetActive(false);

    }

    private void Update()
    {
        if (isGameEnd) return;
        if (isGameOver || isGameWin) return;
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            UpdateTimerUI();
        }
        else
        {
            currentTime = 0;
            UpdateTimerUI();
            Debug.Log("Time end");
            GameWin();
        }

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
        Debug.Log($"Cộng {amount} điểm. Tổng: {score}");
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
            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
    public void GameWin()
    {
        isGameWin = true;
        isGameEnd = true;
        Time.timeScale = 0;
        gameWinUI.SetActive(true);
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
        isGameOver = false;
        isGameEnd = false;
        isGameWin = false;
        score = 0;
        Time.timeScale = 1;
        UpdateUI();
        SceneManager.LoadScene("Game");
        gameOverUI.SetActive(false);
    }
    public bool IsGameEnd()
    {
        return !isGameEnd;
    }
}
