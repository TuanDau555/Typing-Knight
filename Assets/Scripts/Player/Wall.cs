using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Wall : MonoBehaviour
{
    [Header("Cấu hình khoảng cách (Bán kính)")]
    public float range1 = 8f;  // Cách tâm 10m (Hình vuông cạnh 20m)
    public float range2 = 17f;  // Cách tâm 20m (Hình vuông cạnh 40m)

    [Header("Màu sắc vùng")]
    public Color ColorZone1 = new Color(0, 1, 0, 0.5f); // Xanh lá
    public Color ColorZone2 = new Color(1, 1, 0, 0.5f); // Vàng
    public Color ColorZone3 = new Color(1, 0, 0, 0.5f); // Đỏ

    private void OnDrawGizmos()
    {
        // --- VẼ HÌNH VUÔNG (CUBE) ---
        // Lưu ý: DrawWireCube tham số là (Vị trí, Kích thước)
        // Kích thước (Size) phải bằng range * 2 (vì range tính từ tâm ra cạnh)

        // 1. Vẽ vùng xa (Tượng trưng vùng 3)
        Gizmos.color = ColorZone3;
        Gizmos.DrawWireCube(transform.position, new Vector3((range2 + 15) * 2, (range2 + 15) * 2, 0));

        // 2. Vẽ vùng 2 (Vàng)
        Gizmos.color = ColorZone2;
        Gizmos.DrawWireCube(transform.position, new Vector3(range2 * 2, range2 * 2, 0));

        // 3. Vẽ vùng 1 (Xanh - Gần nhất)
        Gizmos.color = ColorZone1;
        Gizmos.DrawWireCube(transform.position, new Vector3(range1 * 2, range1 * 2, 0));

#if UNITY_EDITOR
        // --- HIỂN THỊ CHỮ ---
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.white;
        style.fontSize = 15;
        style.fontStyle = FontStyle.Bold;

        // Vẽ chữ nằm ở cạnh trên của hình vuông
        Handles.Label(transform.position + Vector3.up * range1, $"Mốc {range1}m", style);
        Handles.Label(transform.position + Vector3.up * range2, $"Mốc {range2}m", style);
#endif
    }
}