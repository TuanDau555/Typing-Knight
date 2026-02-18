using UnityEngine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.VirtualTexturing;
using System.Collections;

[System.Serializable]
public class Gamephase
{
    [Tooltip("thoi diem bat dau giai doan nay tinh bang giay")]
    public float startTime;
    public int maxEnemies;
    public float spawnInterval;

    [Header("Enemy Stats")]
    [Tooltip("toc do di chuyen quai")]
    public float enemyMoveSpeed = 2f;
    [Tooltip("sat thuong moi lan tan cong")]
    public int enemyAttackDamage = 5;
    [Tooltip("khoang thoi gian giua cac lan tan cong (giay)")]
    public float enemyAttcakInterval = 1.0f;

}
public class SpawnEndless : MonoBehaviour
{
    public GameManagerEndless gameManagerEndless;
    [Header("vat tinh diem")]
    public Transform targetObjectA;

    [Header("setting spawn")]
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;

    [Header("setting difficulty")]
    [Tooltip("them cac moc thoi gian vao day. script se tu lay moc gan nhat")]
    public List<Gamephase> gamePhases = new List<Gamephase>();

    private int currentMaxEnemy;
    private float currentSpawnInterval;
    private float currentEnemyMoveSpeed;
    private int currentEnemyAttackDamage;
    private float currentEnemyAttcakInterval;

    private List<GameObject> activeEnemies = new List<GameObject>();


    private void Awake()
    {
        if (gameManagerEndless == null)
        {
            gameManagerEndless = FindFirstObjectByType<GameManagerEndless>();
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (targetObjectA == null)
        {
            targetObjectA = transform;
        }
        gamePhases.Sort((p1, p2) => p1.startTime.CompareTo(p2.startTime));
        if (gamePhases.Count > 0)
        {
            ApplyPhase(gamePhases[0]);
        }
        else
        {
            currentMaxEnemy = 5;
            currentSpawnInterval = 2f;
        }
        StartCoroutine(SpawnRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManagerEndless == null) return;
        UpdateDifficulty(gameManagerEndless.CurrentTime);

        if (gameManagerEndless.IsGameEnd()) return;
        if (!string.IsNullOrEmpty(Input.inputString))
        {
            foreach (char c in Input.inputString)
            {
                if (c == '\b' || c == '\n' || c == '\r') continue;
                CheckInputAndKillEnemy(c);
            }
        }
    }
    void UpdateDifficulty(float currentTime)
    {
        for (int i = gamePhases.Count - 1; i >= 0; i--)
        {
            if (currentTime >= gamePhases[i].startTime)
            {
                ApplyPhase(gamePhases[i]);
                break;
            }
        }
    }

    void ApplyPhase(Gamephase phase)
    {
        currentMaxEnemy = phase.maxEnemies;
        currentSpawnInterval = phase.spawnInterval;

        currentEnemyMoveSpeed = phase.enemyMoveSpeed;
        currentEnemyAttackDamage = phase.enemyAttackDamage;
        currentEnemyAttcakInterval = phase.enemyAttcakInterval;
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            // Dọn dẹp danh sách quái chết
            activeEnemies.RemoveAll(item => item == null);

            // Sử dụng biến currentMaxEnemies đã được update
            if (activeEnemies.Count < currentMaxEnemy && !gameManagerEndless.isGameEnd)
            {
                SpawnEnemy();
            }

            // Sử dụng biến currentSpawnInterval đã được update
            yield return new WaitForSeconds(currentSpawnInterval);
        }
    }

    void SpawnEnemy()
    {
        if (spawnPoints.Length == 0) return;

        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPos = spawnPoints[randomIndex];
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPos.position, Quaternion.identity);
        Move moveScript = newEnemy.GetComponent<Move>();
        if (moveScript != null) 
        {
            moveScript.SetMoveSpeed(currentEnemyMoveSpeed);
        }
        Attack attackScript = newEnemy.GetComponent<Attack>();
        if (attackScript != null)
        {
            attackScript.SetAttackDamage(currentEnemyAttackDamage);
            attackScript.SetAttackInterval(currentEnemyAttcakInterval);
        }

        activeEnemies.Add(newEnemy);
    }

    void CheckInputAndKillEnemy(char typedChar)
    {
        bool isCorrect = false;

        // Ưu tiên: Tìm con quái nào đang bị gõ dở dang (Locked target)
        // (Ở mức độ cơ bản này, ta sẽ duyệt qua tất cả quái xem có con nào nhận chữ này không)

        for (int i =0; i < activeEnemies.Count; i++)
        {
            GameObject enemy = activeEnemies[i];


            if (enemy != null)
            {
                CheckInputPlus checkKey = enemy.GetComponent<CheckInputPlus>();

                if (checkKey != null)
                {
                    // Lấy chữ cái hiện tại quái đang chờ
                    char targetChar = checkKey.GetCurrentChar();

                    if (char.ToLower(typedChar) == char.ToLower(targetChar))
                    {
                        // Gửi ký tự vào cho quái xử lý
                        bool isDead = checkKey.CheckChar(typedChar);

                        isCorrect = true; // Gõ đúng

                        if (isDead)
                        {
                            DestroyEnemy(enemy, i); // Gõ xong từ thì giết
                        }

                        // QUAN TRỌNG: Break để mỗi phím chỉ tác động 1 con quái gần nhất/ưu tiên nhất
                        // Nếu không có break, gõ 1 chữ 'a' sẽ trúng tất cả quái có chữ 'a'
                        break;
                    }
                }
            }
        }

        if (isCorrect == false)
        {
            if (gameManagerEndless != null)
            {
                gameManagerEndless.AddWrong(1);
            }
        }
    }

    void DestroyEnemy(GameObject enemy, int index)
    {
        float distance = Vector3.Distance(enemy.transform.position, targetObjectA.position);
        int pointsToAdd = 0;

        if (distance <= 10f) pointsToAdd = 1;
        else if (distance <= 15f) pointsToAdd = 2;
        else pointsToAdd = 3;

        if (gameManagerEndless != null) gameManagerEndless.AddScore(pointsToAdd);

        Destroy(enemy);
        if (index < activeEnemies.Count) activeEnemies.RemoveAt(index);
    }
}
