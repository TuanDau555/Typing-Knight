using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName ="DataConfigOS", menuName ="Typing-Knight", order= 1)]
public class DataConfigOS : ScriptableObject 
{
    [Header("All Topics (For All Mode)")]
    public List<TopicDataOS> allTopics = new List<TopicDataOS>();

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