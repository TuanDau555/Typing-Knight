using UnityEngine;
using TMPro;

public class UIPause : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI wrongText;
    [SerializeField] private TextMeshProUGUI timeText;

    public void Show(int score, int wrong, float elapsedTime)
    {
        panel.SetActive(true);
        scoreText.text = $"Score: {score}";
        wrongText.text = $"Wrong: {wrong}";

        string timeStr = $"{(int)elapsedTime / 60:00}:{(int)elapsedTime % 60:00}";
        timeText.text = $"Time: {timeStr}";
    }

    public void Hide()
    {
        panel.SetActive(false);
    }
}