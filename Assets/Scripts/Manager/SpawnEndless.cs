using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SpawnEndless : MonoBehaviour
{
    public GameManager gameManager;
    public Transform targetObjectA;
    public GameObject enemyPrefab;

    [Header("Danh sách các vị trí Spawn")]
    public Transform[] spawnPoints;

    [SerializeField] private List<Gamephase> gamePhases = new List<Gamephase>();

    private int currentMaxEnemy;
    private float currentSpawnInterval;
    private float currentEnemyMoveSpeed;
    private int currentEnemyAttackDamage;
    private float currentEnemyAttcakInterval;
    private List<GameObject> activeEnemies = new List<GameObject>();
    private Gamephase currentPhase;
    private Coroutine spawnCoroutine;

    private void Awake()
    {
        if (gameManager == null)
        {
            gameManager = FindFirstObjectByType<GameManager>();
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
            if (gameManager != null)
            {
                gameManager.SetCurrentPhase(currentPhase);
            }
        }

        //spawnCoroutine = StartCoroutine(SpawnRoutine());
    }

    void Update()
    {
        if (gameManager == null) return;

        float elapsedTime = gameManager.ElapsedTime;
        UpdateDifficulty(elapsedTime);

        if (gameManager.IsGameEnd || gameManager.IsPaused) return;

        if (!string.IsNullOrEmpty(Input.inputString))
        {
            foreach (char c in Input.inputString)
            {
                if (char.IsControl(c) || c == ' ' || c == '\b' || c == '\n' || c == '\r' || c == '\t') continue;
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

            if (gameManager != null)
            {
                gameManager.SetCurrentPhase(currentPhase);
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

        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
        }
        spawnCoroutine = StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        yield return null;
        while (true)
        {
            activeEnemies.RemoveAll(item => item == null);

            if (activeEnemies.Count < currentMaxEnemy && !gameManager.IsGameEnd)
            {
                SpawnEnemy();
            }

            yield return new WaitForSeconds(currentSpawnInterval);
        }
    }

    void SpawnEnemy()
    {
        if (spawnPoints.Length == 0) return;

        // 1. Chọn random 1 cục Spawn Point
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPos = spawnPoints[randomIndex];

        // 2. Sinh quái vật ra ĐÚNG vị trí (X và Y) của Spawn Point đó
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPos.position, Quaternion.identity);
       
        CheckInputPlus check = newEnemy.GetComponent<CheckInputPlus>();
        if (check != null && gameManager != null)
        {
            string content = gameManager.GetNextContent();
            check.SetContent(content);        // ← dùng hàm mới
        }
        // 3. Cài đặt các thông số cho quái
        Move1 moveScript = newEnemy.GetComponent<Move1>();
        if (moveScript != null)
        {
            // Chỉ còn set Tốc độ, bỏ việc set Tọa độ Y (Lane) đi
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

        for (int i = 0; i < activeEnemies.Count; i++)
        {
            GameObject enemy = activeEnemies[i];

            if (enemy != null)
            {
                CheckInputPlus checkKey = enemy.GetComponent<CheckInputPlus>();

                if (checkKey != null)
                {
                    char targetChar = checkKey.GetCurrentChar();

                    if (char.ToLower(typedChar) == char.ToLower(targetChar))
                    {
                        bool isDead = checkKey.CheckChar(typedChar);
                        isCorrect = true;

                        if (isDead)
                        {
                            DestroyEnemy(enemy, i);
                        }

                        break;
                    }
                }
            }
        }

        if (!isCorrect)
        {
            gameManager.AddWrong(1);
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

        if (distance <= 8f) pointsToAdd = 1;
        else if (distance <= 18f) pointsToAdd = 2;
        else pointsToAdd = 3;

        if (gameManager != null) gameManager.AddScore(pointsToAdd);

        Destroy(enemy);
        if (index < activeEnemies.Count) activeEnemies.RemoveAt(index);
    }
}