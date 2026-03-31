using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class WinConditions
{
    [Header("Conditions (For Countdown Mode Only)")]
    public int requiredScore = 270;
    public int maxAllowedWrong = 30;
    [Range(0, 1)] public float minHpPercent = 0.7f;
}

public class UIWinResult : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private WinConditions conditions;
    [Tooltip("số điểm sẽ chia con số này để ra tiền")]
    [SerializeField] private float scoreDivisor = 10f;

    [Header("UI Panels")]
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject overPanel;

    [Header("Win UI References")]
    [SerializeField] private TextMeshProUGUI winTitleText;
    [SerializeField] private TextMeshProUGUI winSummaryText;

    [Header("Over UI References")]
    [SerializeField] private TextMeshProUGUI overTitleText;
    [SerializeField] private TextMeshProUGUI overSummaryText;

    [Header("Stars UI")]
    [SerializeField] private GameObject starContainer; // Chứa 3 ngôi sao
    [SerializeField] private GameObject[] stars;        // Mảng 3 ngôi sao (Image/Object)
    public void Show(int score, int wrong, float elapsedTime, float hpPercent, bool isCountdown, bool isWin)
    {
        if (winPanel != null) winPanel.SetActive(isWin);
        if (overPanel != null) overPanel.SetActive(!isWin);

        // 2. Chọn Text để hiển thị dựa trên trạng thái Win/Loss
        TextMeshProUGUI activeTitle = isWin ? winTitleText : overTitleText;
        TextMeshProUGUI activeSummary = isWin ? winSummaryText : overSummaryText;
        if (activeTitle != null)
        {
            activeTitle.text = isWin ? "GAME WIN" : "GAME OVER";
        }
        // --- LOGIC QUY ĐỔI TIỀN ---
        // Đảm bảo không chia cho 0 để tránh lỗi
        float finalDivisor = Mathf.Max(0.1f, scoreDivisor);
        int earnedMoney = Mathf.FloorToInt(score / finalDivisor);

        // 1. Hiển thị thông tin chung
        string timeStr = $"{(int)elapsedTime / 60:00}:{(int)elapsedTime % 60:00}";
        string summary = $"Score: {score}\n" +
                                 $"Wrong: {wrong}\n" +
                                 $"Time: {timeStr}\n" +
                                 $"<color=#FFD700>Money: +{earnedMoney}$</color>"; // Hiện tiền màu vàng

        if (activeSummary != null)
        {
            activeSummary.text = summary;
        }
        // 2. Xử lý đánh giá Sao
        if (starContainer != null)
        {
            // ĐIỀU KIỆN HIỆN SAO: Phải là Panel Thắng VÀ Phải là mode Countdown
            bool shouldShowStars = isWin && isCountdown;

            starContainer.SetActive(shouldShowStars);

            if (shouldShowStars)
            {
                EvaluateStars(score, wrong, hpPercent);
            }
        }

        // Gọi hệ thống lưu trữ tiền ở đây
        // Wallet.Instance.AddMoney(earnedMoney);
    }

    private void EvaluateStars(int score, int wrong, float hpPercent)
    {
        // Ẩn tất cả sao trước khi tính
        foreach (var s in stars) s.SetActive(false);

        // Điều kiện 1: Điểm số
        if (score >= conditions.requiredScore) stars[0].SetActive(true);

        // Điều kiện 2: Số câu sai
        if (wrong <= conditions.maxAllowedWrong) stars[1].SetActive(true);

        // Điều kiện 3: Máu của tường
        if (hpPercent >= conditions.minHpPercent) stars[2].SetActive(true);
    }
}

