using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SpawnEndless : MonoBehaviour
{
    public GameManagerEndless gameManagerEndless;
    public Transform targetObjectA;
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;

    public List<Gamephase> gamePhases = new List<Gamephase>();

    [Header("5 Lane Y - Ngẫu nhiên")]
    public float[] laneYPositions = new float[] { -3f, -1.5f, 0f, 1.5f, 3f };

    private int nextLaneIndex = 0;
    private int currentMaxEnemy;
    private float currentSpawnInterval;
    private float currentEnemyMoveSpeed;
    private int currentEnemyAttackDamage;
    private float currentEnemyAttcakInterval;
    private List<GameObject> activeEnemies = new List<GameObject>();
    private Gamephase currentPhase;

    private void Awake()
    {
        if (gameManagerEndless == null)
        {
            gameManagerEndless = FindFirstObjectByType<GameManagerEndless>();
        }
    }

    void Start()
    {
        if (targetObjectA == null) targetObjectA = transform;

        gamePhases.Sort((p1, p2) => p1.startTime.CompareTo(p2.startTime));

        // Áp dụng phase đầu tiên ngay từ đầu
        if (gamePhases.Count > 0)
        {
            currentPhase = gamePhases[0];
            ApplyPhase(currentPhase);
            if (gameManagerEndless != null)
            {
                gameManagerEndless.SetCurrentPhase(currentPhase);
            }
        }

        StartCoroutine(SpawnRoutine());
    }

    void Update()
    {
        if (gameManagerEndless == null) return;

        float elapsedTime = gameManagerEndless.elapsedTime;
        UpdateDifficulty(elapsedTime);

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

    void UpdateDifficulty(float elapsedTime)
    {
        Gamephase newPhase = null;
        for (int i = gamePhases.Count - 1; i >= 0; i--)
        {
            if (elapsedTime >= gamePhases[i].startTime)
            {
                newPhase = gamePhases[i];
                break;
            }
        }

        if (newPhase != null && newPhase != currentPhase)
        {
            currentPhase = newPhase;
            ApplyPhase(currentPhase);

            if (gameManagerEndless != null)
            {
                gameManagerEndless.SetCurrentPhase(currentPhase);
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
            activeEnemies.RemoveAll(item => item == null);

            if (activeEnemies.Count < currentMaxEnemy && !gameManagerEndless.isGameEnd)
            {
                SpawnEnemy();
            }

            yield return new WaitForSeconds(currentSpawnInterval);
        }
    }

    void SpawnEnemy()
    {
        if (spawnPoints.Length == 0) return;

        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPos = spawnPoints[randomIndex];

        GameObject newEnemy = Instantiate(enemyPrefab, spawnPos.position, Quaternion.identity);

        Move1 moveScript = newEnemy.GetComponent<Move1>();
        if (moveScript != null)
        {
            int randomLaneIndex = Random.Range(0, laneYPositions.Length);  
            float chosenLaneY = laneYPositions[randomIndex];
            moveScript.SetLane(chosenLaneY);
            moveScript.SetMoveSpeed(currentEnemyMoveSpeed);
            Vector3 newPos = newEnemy.transform.position;
            newPos.y = chosenLaneY;
            newEnemy.transform.position = newPos;
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

        for (int i = 0; i < activeEnemies.Count; i++)
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
        if (!isCorrect)
        {
            gameManagerEndless.AddWrong(1);
        }
    }

    public float GetCurrentEnemyMoveSpeed()
    {
        return currentEnemyMoveSpeed;
    }

    void DestroyEnemy(GameObject enemy, int index)
    {
        float distance = Vector3.Distance(enemy.transform.position, targetObjectA.position);
        int pointsToAdd = 0;

        if (distance <= 10f) pointsToAdd = 1;
        else if (distance <= 20f) pointsToAdd = 2;
        else pointsToAdd = 3;

        if (gameManagerEndless != null) gameManagerEndless.AddScore(pointsToAdd);

        Destroy(enemy);
        if (index < activeEnemies.Count) activeEnemies.RemoveAt(index);
    }
}