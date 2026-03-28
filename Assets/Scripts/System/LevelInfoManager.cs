using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

[System.Serializable]
public class LevelData
{
    [Header("Tên hiển thị trên panel")]
    public string levelDisplayName;

    [Header("Điều kiện 3 sao (có thể xuống dòng bằng \\n)")]
    [TextArea(4, 8)]
    public string threeStarConditions;

    [Header("Tên scene thật (phải giống trong Build Settings)")]
    public string sceneName;
}

public class LevelInfoManager : MonoBehaviour
{
    [Header("=== Panel ===")]
    public GameObject levelInfoPanel;

    [Header("=== Text trong Panel ===")]
    public TextMeshProUGUI txtLevelName;
    public TextMeshProUGUI txt3StarConditions;

    [Header("=== DỮ LIỆU TẤT CẢ LEVEL (hiện ngoài Inspector) ===")]
    public LevelData[] allLevels;

    private string sceneToLoad;

    // Gọi từ nút level trên map
    public void ShowLevelInfo(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= allLevels.Length)
        {
            Debug.LogError("Level index không tồn tại! Kiểm tra lại số level.");
            return;
        }

        LevelData data = allLevels[levelIndex];

        txtLevelName.text = data.levelDisplayName;
        txt3StarConditions.text = data.threeStarConditions;
        sceneToLoad = data.sceneName;

        levelInfoPanel.SetActive(true);
    }

    public void OnPlayButtonClicked()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
            SceneManager.LoadScene(sceneToLoad);
    }

    public void OnCloseButtonClicked()
    {
        levelInfoPanel.SetActive(false);
    }
}