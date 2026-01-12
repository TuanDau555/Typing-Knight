using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float distance = 5f;

    private Vector3 startPos;
    private float targetX; // Điểm đích muốn đến
    private bool isMoving = true; // Biến "công tắc" để kiểm soát di chuyển

    void Start()
    {
        startPos = transform.position;
        // Tính toán trước điểm đích (Đi sang trái thì lấy X ban đầu TRỪ đi distance)
        targetX = startPos.x - distance;
    }

    void Update()
    {
        // 1. Kiểm tra: Nếu biến isMoving là false thì Dừng (không chạy code bên dưới nữa)
        if (isMoving == false) return;

        // 2. Di chuyển sang trái
        transform.Translate(Vector2.left * speed * Time.deltaTime);

        // 3. Kiểm tra xem đã đến đích chưa (Toạ độ X nhỏ hơn hoặc bằng đích)
        if (transform.position.x <= targetX)
        {
            StopMoving();
        }
    }

    void StopMoving()
    {
        // Set cứng vị trí lại ngay tại đích (để tránh bị trôi lố đà một chút)
        transform.position = new Vector3(targetX, transform.position.y, transform.position.z);

        // Tắt công tắc di chuyển -> Update sẽ không chạy lệnh Translate nữa
        isMoving = false;

        // (Tuỳ chọn) Nếu có Animator, hãy chuyển sang trạng thái đứng yên ở đây
        // GetComponent<Animator>().SetBool("IsRun", false);

        Debug.Log("Enemy đã đến đích và đứng im!");
    }
}