using UnityEngine;

public class WallHealth : MonoBehaviour
{
    public int HP = 100;       // Máu tường
    public int defense = 10;   // Phòng thủ (nếu muốn wall mất máu nhanh hơn → đổi thành 0)

    void Start()
    {
        Debug.Log($"Wall khởi tạo! HP: {HP} | Defense: {defense}");
    }

    public void TakeDamage(int dmg)
    {
        int realDamage = Mathf.Max(0, dmg - defense);
        HP -= realDamage;

        Debug.Log($"Wall bị đánh! Damage gốc: {dmg} | Defense: {defense} | Thực tế trừ: {realDamage} | HP còn: {HP}");

        if (HP <= 0)
        {
            Debug.Log("Wall đã chết!");
            Destroy(gameObject);  // Wall biến mất khi hết máu
        }
    }
}