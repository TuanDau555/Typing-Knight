using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName ="DataConfigOS", menuName ="Typing-Knight, order= 1")]
public class DataConFigOS : ScriptableObject 
{
    [Header("All Topics (For All Mode)")]
    public List<GameTopic> allTopics = new List<GameTopic>();

    [Header("Setting for all mode")]
    public List<ModeLevelConfig> modeLevels = new List<ModeLevelConfig>() ;
}

[System.Serializable]
public class ModeLevelConfig
{
    public string levelName = "level 1";
    public float targetTime = 180f;
    public bool isWordMode = true;
    [Header("cot moc tang do kho trong man")]
    public List<Gamephase> phases = new List<Gamephase>();
}

[System.Serializable]
public class Gamephase
{
    [Tooltip("Thời điểm bắt đầu giai đoạn này tính bằng giây")]
    public float startTime;
    public int maxEnemies;
    public float spawnInterval;

    [Header("Enemy Stats")]
    [Tooltip("Tốc độ di chuyển quái")]
    public float enemyMoveSpeed = 2f;
    [Tooltip("Sát thương mỗi lần tấn công")]
    public int enemyAttackDamage = 5;
    [Tooltip("Khoảng thời gian giữa các lần tấn công (giây)")]
    public float enemyAttcakInterval = 1.0f;

    [Header("Topic cho phase này")]
    [Tooltip("Index của các topic muốn dùng cho phase này (0-based). Nếu để trống thì dùng tất cả topic")]
    public List<TopicDataOS> selectedTopic = new List<TopicDataOS>();
}