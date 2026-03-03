using UnityEngine;

[CreateAssetMenu(fileName = "LevelStoryData", menuName = "Typing-Kinght/LevelStoryData")]
public class LevelStoryData : ScriptableObject
{
    public string levelName; // "Chương 1 - Màn 1", ...

    [Header("Intro (trước khi chơi)")]
    public DialogueLine[] introLines;

    [Header("Outro (sau khi thắng)")]
    public DialogueLine[] outroLines;
}