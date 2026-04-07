using System.Collections;
using UnityEngine;

public class Attack : MonoBehaviour

{
    [SerializeField] private int attackDamage = 5;
    [SerializeField] private float attackInterval = 1.0f;

    public bool isattack = false;
    private Wall targetWall;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //kiểm tra tường có tồn tại hay không
    void Start()
    {
        targetWall = FindObjectOfType<Wall>();
        if (targetWall == null)
        {
            Debug.LogError("coun't find wall in scene");
        }
    }
    public void SetAttackDamage(int newDamage)
    {
        attackDamage = newDamage;
    }

    public void SetAttackInterval(float newInterval)
    {
        attackInterval = newInterval;
    }
    //hàm dùng vòng lặp
    public void StartAttacking()
    {
        if (!isattack && targetWall != null && targetWall.currentHp > 0)
        {
            StartCoroutine(AttackRoutine());
        }
    }

    // hàm dừng vòng lặp tấn công
    public void StopAttacking()
    {
        isattack = false;
        StopAllCoroutines();
    }

    //vòng lặp tấn công
    public IEnumerator AttackRoutine()
    {
        isattack = true;
        while (targetWall != null && targetWall.currentHp > 0)
        {
            targetWall.TakeDamage(attackDamage);
            yield return new WaitForSeconds(attackInterval);
        }
        isattack = false;
    }
}
