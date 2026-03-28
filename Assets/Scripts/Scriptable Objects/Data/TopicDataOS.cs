using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "TypingKngith/Topic Data", fileName = "TopicDataOS", order = 10)]
public class TopicDataOS : ScriptableObject

{
    [Tooltip("Tên hiển thị Debug")]
    public string topicName;
    [Header("Chế độ nhập liệu")]
    public bool isWordMode;
    [Header("Dữ liệu chữ cái từ bàn phím")]
    [TextArea(3, 6)]
    [SerializeField] public string charset;
    [SerializeField] public List<string> wordList = new List<string>();

    public string GetRandomContent()
    {
        if (isWordMode)
        {
            if (wordList == null || wordList.Count == 0)
            {
                Debug.LogWarning($"Topic {topicName} ({name}) rỗng wordList!", this);
                return "???";
            }
            return wordList[Random.Range(0, wordList.Count)];
        }
        else
        {
            if (string.IsNullOrEmpty(charset))
            {
                Debug.LogWarning($"Topic {topicName} ({name}) charset rỗng!", this);
                return "A";
            }
            int idx = Random.Range(0, charset.Length);
            return charset[idx].ToString();
        }
        
    }
    private void OnValidate()
    {
        if (isWordMode && (wordList == null || wordList.Count == 0))
            Debug.LogWarning($"[Validate] {name}: Word mode nhưng wordList rỗng", this);

        if (!isWordMode && string.IsNullOrEmpty(charset))
            Debug.LogWarning($"[Validate] {name}: Char mode nhưng charset rỗng", this);
    }

}
