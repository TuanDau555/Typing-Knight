using UnityEngine;
using TMPro;
public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI wrongText;

    public int Score {  get; private set; }
    public int WrongCount { get; private set; }

    public void AddScore(int amount)
    {
        Score += amount;
        UpdateUI();
    }
    public void AddWrong(int amount)
    {
        WrongCount += amount;
        UpdateUI();
    }
    public void Reset()
    {
        Score = 0;
        WrongCount = 0;
        UpdateUI();
    }
    private void UpdateUI()
    {
        if (scoreText != null) scoreText.text = $"Score: {Score}";
        if (wrongText != null) wrongText.text = $"Wrong: {WrongCount}";
    }
}
