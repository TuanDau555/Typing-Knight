using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Events;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI speakerText;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private float typingSpeed = 0.04f; // tốc độ gõ chữ

    private bool isTyping = false;
    private bool skip = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        dialoguePanel.SetActive(false);
    }

    public void StartDialogue(DialogueLine[] lines, UnityAction onComplete = null)
    {
        dialoguePanel.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(PlayDialogueSequence(lines, onComplete));
    }

    private IEnumerator PlayDialogueSequence(DialogueLine[] lines, UnityAction onComplete)
    {
        foreach (var line in lines)
        {
            speakerText.text = line.speaker;
            yield return StartCoroutine(TypeSentence(line.text));
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)); // skip bằng space hoặc click
        }

        dialoguePanel.SetActive(false);
        onComplete?.Invoke();
    }

    private IEnumerator TypeSentence(string fullText)
    {
        isTyping = true;
        skip = false;
        dialogueText.text = "";

        float timer = 0f;
        int charIndex = 0;

        while (charIndex < fullText.Length)
        {
            timer += Time.unscaledDeltaTime;  // <--- dùng unscaled để chạy khi timescale=0

            if (timer >= typingSpeed)
            {
                timer -= typingSpeed;
                dialogueText.text += fullText[charIndex];
                charIndex++;
            }

            if (skip)
            {
                dialogueText.text = fullText;
                break;
            }

            yield return null;  // chờ frame tiếp theo
        }

        isTyping = false;
    }
    // Gọi từ Update nếu muốn skip nhanh toàn bộ dòng
    private void Update()
    {
        if (isTyping && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            skip = true;
            Debug.Log("Skip typing requested!");
        }
    }
}

[System.Serializable]
public class DialogueLine
{
    public string speaker;      // "Arin", "Shadow", "Lena", ...
    [TextArea(2, 5)] public string text;
}