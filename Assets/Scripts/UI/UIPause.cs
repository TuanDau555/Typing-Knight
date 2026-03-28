using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // Thêm thư viện này để chuyển cảnh

public class UIPause : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI wrongText;
    [SerializeField] private TextMeshProUGUI timeText;

    [Header("Settings")]
    [Tooltip("Tên chính xác của Scene bạn muốn thoát ra (ví dụ: MainMenu)")]
    [SerializeField] private string sceneToLoad = "MainMenu";

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

    // Hàm để gọi khi bấm nút Quit
    public void QuitToMenu()
    {
        // QUAN TRỌNG: Phải trả lại thời gian về bình thường trước khi chuyển cảnh
        Time.timeScale = 1f;

        // Load scene theo tên đã nhập ở Inspector
        SceneManager.LoadScene(sceneToLoad);
    }

    // Nếu bạn muốn linh hoạt hơn, dùng hàm này để thoát ra bất cứ scene nào truyền vào
    public void QuitToSpecificScene(string customSceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(customSceneName);
    }
}