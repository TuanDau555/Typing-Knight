using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public Transform wallTarget;      // Kéo object Wall vào đây trong Inspector
    public float speed = 3f;          // Tốc độ di chuyển
    public float attackRange = 1.8f;  // Khoảng cách dừng để đánh (tăng lên 2.0 nếu cần)
    public int damage = 20;           // Sát thương mỗi đòn (bạn muốn 20 thì để 20)
    public float attackCooldown = 1f; // Thời gian giữa các đòn đánh

    private float lastAttackTime;

    void Start()
    {
        // Tự động tìm Wall nếu quên kéo
        if (wallTarget == null)
        {
            wallTarget = GameObject.Find("Wall").transform;
            Debug.Log("EnemyMove: Tự tìm thấy Wall target");
        }
         WallHealth wallScript = wallTarget.GetComponent<WallHealth>();

    }

    void Update()
    {
        if (wallTarget == null) return;

        float distance = Vector2.Distance(transform.position, wallTarget.position);

        // Chưa tới gần → di chuyển tới wall
        if (distance > attackRange)
        {
            Vector2 direction = (wallTarget.position - transform.position).normalized;
            transform.position += (Vector3)(direction * speed * Time.deltaTime);

            // Flip sprite theo hướng di chuyển
            transform.localScale = new Vector3(Mathf.Sign(direction.x), 1f, 1f);
        }
        // Đã tới gần → tấn công
        else if (Time.time >= lastAttackTime + attackCooldown)
        {
            if (wallScript != null)
            {
                wallScript.TakeDamage(damage);
                Debug.Log($"Goblin tấn công wall! Damage gửi đi: {damage}");
            }
            else
            {
                Debug.LogError("Không tìm thấy script WallHealth trên Wall!");
            }

            lastAttackTime = Time.time;
        }
    }
}