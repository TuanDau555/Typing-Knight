using UnityEngine;
using TMPro;

public class CheckInputPlus : MonoBehaviour
{
    [SerializeField] private TextMeshPro textMesh;

    private string currentWord = "";
    private int currentIndex = 0;

    private void Awake()  // Đổi thành Awake để chạy sớm hơn Start
    {
        if (textMesh == null)
            textMesh = GetComponentInChildren<TextMeshPro>();

    }

    public void SetContent(string newContent)   // ← Hàm mới quan trọng
    {
        if (string.IsNullOrEmpty(newContent))
            newContent = "???";

        currentWord = newContent;
        currentIndex = 0;
        UpdateVisual();
    }

    public bool CheckChar(char inputChar)
    {
        if (currentIndex >= currentWord.Length) return false;

        char needed = currentWord[currentIndex];
        if (char.ToLower(inputChar) == char.ToLower(needed))
        {
            currentIndex++;
            UpdateVisual();

            return currentIndex >= currentWord.Length; // true = chết
        }
        return false;
    }

    public char GetCurrentChar()
    {
        return currentIndex < currentWord.Length ? currentWord[currentIndex] : ' ';
    }

    private void UpdateVisual()
    {
        if (textMesh == null) return;

        string typed = currentWord.Substring(0, currentIndex);
        string remain = currentIndex < currentWord.Length
            ? currentWord.Substring(currentIndex)
            : "";

        textMesh.text = $"<color=#00FF00>{typed}</color><color=#FFFFFF>{remain}</color>";
    }

    // Tự động lấy nếu chưa được gán (dành cho test nhanh)
    private void Start()
    {
        if (string.IsNullOrEmpty(currentWord) && GameManager.Instance != null)
        {
            SetContent(GameManager.Instance.GetNextContent());
        }
        else if (string.IsNullOrEmpty(currentWord))
        {
            SetContent("test");
        }
    }
}