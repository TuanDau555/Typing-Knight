using Unity.VisualScripting;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [Header("setup Wall")]
    [SerializeField] private int maxHp = 100;
    [SerializeField] private int defense = 0;
    public int currentHp;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHp = maxHp;
    }
    public void TakeDamage(int damage)
    {
        int finalDamage = Mathf.Max(1, damage - defense);
        currentHp -= finalDamage;
        Debug.Log($"wall is attacking ! lose{finalDamage} hp now{currentHp}");
        if (currentHp <= 0)
        {
            currentHp =0;
            FindObjectOfType<GameManager>().GameOver();
        }
    }
    
}
