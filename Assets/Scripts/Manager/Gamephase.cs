using UnityEngine;
using System.Collections.Generic;

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
    public List<int> selectedTopicIndices = new List<int>();
}