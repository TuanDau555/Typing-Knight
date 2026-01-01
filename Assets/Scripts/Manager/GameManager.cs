using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("UI Score & Wrong")]
    [SerializeField] private TextMeshProUGUI wrongKeyText;
    [SerializeField] private TextMeshProUGUI scoreText; // Kéo Text điểm số vào đây

    private int wrong = 0;
    private int score = 0;

    void Start()
    {
        UpdateUI();
    }

    // --- XỬ LÝ LỖI (WRONG) ---
    public void AddWrong(int amount)
    {
        wrong += amount;
        UpdateUI();
    }

    // --- XỬ LÝ ĐIỂM (SCORE) - Mới thêm vào ---
    public void AddScore(int amount)
    {
        score += amount;
        UpdateUI();
        Debug.Log($"Cộng {amount} điểm. Tổng: {score}");
    }

    private void UpdateUI()
    {
        if (wrongKeyText != null) wrongKeyText.text = "Wrong: " + wrong.ToString();
        if (scoreText != null) scoreText.text = "Score: " + score.ToString();
    }
}