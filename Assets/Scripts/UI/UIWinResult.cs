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

    [Header("UI References")]
    [SerializeField] private GameObject winPanel;
    [SerializeField] private TextMeshProUGUI resultSummaryText;
    //[SerializeField] private TextMeshProUGUI moneyText;

    [Header("Stars UI")]
    [SerializeField] private GameObject starContainer; // Chứa 3 ngôi sao
    [SerializeField] private GameObject[] stars;        // Mảng 3 ngôi sao (Image/Object)
    public void Show(int score, int wrong, float elapsedTime, float hpPercent, bool isCountdown)
    {
        if (winPanel != null) winPanel.SetActive(true);

        // --- LOGIC QUY ĐỔI TIỀN ---
        // Đảm bảo không chia cho 0 để tránh lỗi
        float finalDivisor = Mathf.Max(0.1f, scoreDivisor);
        int earnedMoney = Mathf.FloorToInt(score / finalDivisor);

        // 1. Hiển thị thông tin chung
        string timeStr = $"{(int)elapsedTime / 60:00}:{(int)elapsedTime % 60:00}";
        resultSummaryText.text = $"Score: {score}\n" +
                                 $"Wrong: {wrong}\n" +
                                 $"Time: {timeStr}\n" +
                                 $"<color=#FFD700>Money: +{earnedMoney}$</color>"; // Hiện tiền màu vàng

        // 2. Xử lý đánh giá Sao
        if (isCountdown)
        {
            starContainer.SetActive(true);
            EvaluateStars(score, wrong, hpPercent);
        }
        else
        {
            starContainer.SetActive(false); // Mode thường không hiện sao
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

