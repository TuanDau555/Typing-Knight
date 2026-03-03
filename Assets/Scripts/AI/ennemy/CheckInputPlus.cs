using UnityEngine;
using TMPro;
public class CheckInputPlus : MonoBehaviour
{
    [SerializeField] private TextMeshPro textMesh;

    public string currentWord;
    private int currentIndex = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (GameManagerEndless.Instance != null)
        {
            currentWord = GameManagerEndless.Instance.GetRandomContent();
        }
        else
        {
            currentWord = "abc";
        }
        UpdateVisual();

    }

    public bool CheckChar(char inputChar)
    {
        char charNeedToType = currentWord[currentIndex];
        if (char.ToLower(inputChar) == char.ToLower(charNeedToType))
        {
            currentIndex++;
            UpdateVisual();

            if (currentIndex >= currentWord.Length)
            {
                return true;
            }
        }
        return false;
    }

    public char GetCurrentChar()
    {
        if (currentIndex < currentWord.Length)
        {
            return currentWord[currentIndex];
        }
        return ' ';
    }

    void UpdateVisual()
    {
        string typePart = currentWord.Substring(0, currentIndex);
        string remainPart = currentWord.Substring(currentIndex);

        textMesh.text = $"<color=green>{typePart}</color><color=white>{remainPart}</color>";
    }

}
