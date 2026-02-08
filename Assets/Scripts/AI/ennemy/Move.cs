using UnityEngine;

public class Move : MonoBehaviour
{
    [Header("setting move")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float stopDistance = 1.5f;

    private Attack attacking;
    private Wall targetWall;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetWall = FindObjectOfType<Wall>();
        attacking = GetComponent<Attack>();   // ← thêm dòng này

        if (attacking == null)
        {
            Debug.LogError("Không tìm thấy component Attack trên " + gameObject.name);
        }
        if (targetWall == null)
        {
            Debug.LogError("Khong tim thay wall");
        }

    }
    void Update()
    {
        if (targetWall == null || targetWall.currentHp <= 0) return;
        float distanceX = Mathf.Abs(transform.position.x - targetWall.transform.position.x);
        if (distanceX > stopDistance)
        {
            Moving();
            if (attacking != null)
            {
                attacking.StopAttacking();
            }
        }
        else
        {
            if (attacking != null)
            {
                attacking.StartAttacking();
            }
        }
    }

    private void Moving()
    {
        Vector3 targetPosition = new Vector3(targetWall.transform.position.x, transform.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

}
