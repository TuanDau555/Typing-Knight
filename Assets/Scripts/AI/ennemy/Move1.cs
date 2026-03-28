using UnityEngine;

public class Move1 : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float baseMoveSpeed = 12f;

    [Header("Lane Settings")]
    [SerializeField] private float myLaneY = 0f;                    // Lane Y được gán khi spawn
    [SerializeField] private float[] laneBounds = { -3.5f, 3.5f };  // giới hạn di chuyển

    [Header("Blocking & Spacing Settings")]
    [SerializeField] private float blockDistance = 2.8f;            // Khoảng cách dừng (tăng để giãn xa hơn, dễ đọc chữ)
    [SerializeField] private LayerMask enemyLayer;                  // Layer "Enemy" – kéo vào Inspector

    private Animator animator;
    private Attack attacking;
    private Wall targetWall;
    private Rigidbody2D rb;
    private bool isInAttackRange = false;

    private SpawnEndless spawnManager;

    void Start()
    {
        animator = GetComponent<Animator>();
        attacking = GetComponent<Attack>();
        targetWall = FindFirstObjectByType<Wall>();
        rb = GetComponent<Rigidbody2D>();
        spawnManager = FindFirstObjectByType<SpawnEndless>();

        if (rb == null) Debug.LogError("Thiếu Rigidbody2D (Dynamic) trên " + gameObject.name);
        if (animator == null) Debug.LogWarning("Thiếu Animator!");

        // Khởi tạo velocity Y = 0
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        myLaneY = transform.position.y;
    }

    #region di chuyển vật lý  và giữ lane
    void FixedUpdate()
    {
        if (targetWall == null || targetWall.currentHp <= 0 || isInAttackRange)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        float currentSpeed = spawnManager != null
            ? spawnManager.GetCurrentEnemyMoveSpeed()
            : baseMoveSpeed;

        // Kiểm tra có enemy phía trước trong cùng lane không
        Collider2D frontEnemy = GetFrontEnemyInLane();

        if (frontEnemy != null)
        {
            // Dừng hoàn toàn khi chạm enemy phía trước
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // Di chuyển bình thường
        Vector2 direction = new Vector2(targetWall.transform.position.x - rb.position.x, 0).normalized;
        rb.linearVelocity = direction * currentSpeed;

        // Soft lock lane Y + giới hạn đường nâu
        Vector2 pos = rb.position;
        pos.y = Mathf.Lerp(pos.y, myLaneY, 0.4f);
        pos.y = Mathf.Clamp(pos.y, laneBounds[0], laneBounds[1]);
        rb.position = pos;
    }
    #endregion
    // Tìm enemy phía trước trong cùng lane
    private Collider2D GetFrontEnemyInLane()
    {
        // Mở rộng detect range để phát hiện sớm
        Collider2D[] hits = Physics2D.OverlapCircleAll(rb.position, blockDistance * 1.5f, enemyLayer);

        Collider2D closestFront = null;
        float closestDist = float.MaxValue;

        foreach (var col in hits)
        { 
            if (col.gameObject == gameObject) continue;

            Move1 other = col.GetComponent<Move1>();
            if (other == null) continue;

            // Chỉ tính enemy phía trước + cùng lane
            if (Mathf.Abs(other.myLaneY - myLaneY) < 0.6f &&
                other.transform.position.x > transform.position.x)
            {
                float dist = Vector2.Distance(rb.position, other.transform.position);
                if (dist < closestDist && dist < blockDistance)
                {
                    closestDist = dist;
                    closestFront = col;
                }
            }
        }

        return closestFront;
    }

    void Update()
    {
        if (targetWall == null || targetWall.currentHp <= 0) return;

        if (isInAttackRange)
        {
            if (attacking != null) attacking.StartAttacking();
            animator.SetBool("isAttacking", true);
        }
        else
        {
            if (attacking != null) attacking.StopAttacking();
            animator.SetBool("isAttacking", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Wall>() != null)
            isInAttackRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Wall>() != null)
            isInAttackRange = false;
    }

    public void SetLane(float laneY) => myLaneY = laneY;
    public void SetMoveSpeed(float newSpeed) => baseMoveSpeed = newSpeed;
}