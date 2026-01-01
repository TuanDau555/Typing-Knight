using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemySpawner : MonoBehaviour
{
    public GameManager gamemanager;

    [Header("Cấu hình Tính Điểm")]
    [Tooltip("Kéo Vật A (Người chơi hoặc Base) vào đây để làm mốc tính khoảng cách")]
    public Transform targetObjectA;

    [Header("Cài đặt Spawn")]
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;

    [Header("Kiểm soát số lượng")]
    public int maxEnemies = 5;
    public float spawnInterval = 2f;

    private List<GameObject> activeEnemies = new List<GameObject>();

    private void Awake()
    {
        if (gamemanager == null)
        {
            gamemanager = FindFirstObjectByType<GameManager>();
        }
    }

    void Start()
    {
        // Kiểm tra an toàn
        if (targetObjectA == null)
        {
            Debug.LogWarning("Chưa gán Target Object A! Sẽ tính khoảng cách từ chính Spawner này.");
            targetObjectA = transform;
        }

        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            activeEnemies.RemoveAll(item => item == null);

            if (activeEnemies.Count < maxEnemies)
            {
                SpawnEnemy();
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnEnemy()
    {
        if (spawnPoints.Length == 0) return;

        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPos = spawnPoints[randomIndex];
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPos.position, Quaternion.identity);
        activeEnemies.Add(newEnemy);
    }

    void Update()
    {
        if (!string.IsNullOrEmpty(Input.inputString))
        {
            foreach (char c in Input.inputString)
            {
                if (c == '\b' || c == '\n' || c == '\r') continue;
                CheckInputAndKillEnemy(c);
            }
        }
    }

    void CheckInputAndKillEnemy(char typedChar)
    {
        bool hasKilledEnemy = false;

        for (int i = activeEnemies.Count - 1; i >= 0; i--)
        {
            GameObject enemy = activeEnemies[i];

            if (enemy != null)
            {
                KeyBoardCheck label = enemy.GetComponent<KeyBoardCheck>();

                if (label != null)
                {
                    if (char.ToLower(typedChar) == char.ToLower(label.myChar))
                    {
                        // GỌI HÀM TIÊU DIỆT (Sửa đổi để truyền enemy vào tính toán)
                        DestroyEnemy(enemy, i);
                        hasKilledEnemy = true;
                        break;
                    }
                }
            }
        }

        if (hasKilledEnemy == false)
        {
            if (gamemanager != null)
            {
                gamemanager.AddWrong(1);
                Debug.Log("Bấm sai rồi: " + typedChar);
            }
        }
    }

    // --- PHẦN LOGIC TÍNH ĐIỂM KHOẢNG CÁCH ---
    void DestroyEnemy(GameObject enemy, int index)
    {
        // 1. Tính khoảng cách từ Quái đến Vật A
        float distance = Vector3.Distance(enemy.transform.position, targetObjectA.position);

        // 2. Tính điểm dựa trên khoảng cách
        int pointsToAdd = 0;

        if (distance <= 10f)
        {
            pointsToAdd = 1; // 0-8m
        }
        else if (distance <= 15f)
        {
            pointsToAdd = 2; // 8-17m
        }
        else
        {
            pointsToAdd = 3; // > 17m
        }

        // 3. Cộng điểm vào GameManager
        if (gamemanager != null)
        {
            gamemanager.AddScore(pointsToAdd);
        }

        Debug.Log($"Diệt quái cách {distance:F1}m -> Cộng {pointsToAdd} điểm");

        // 4. Xóa quái
        Destroy(enemy);
        activeEnemies.RemoveAt(index);
    }
}