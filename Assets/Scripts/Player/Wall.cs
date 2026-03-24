using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Wall : MonoBehaviour
{
    [Header("Setup Wall")]
    [SerializeField] private int maxHp = 100;
    [SerializeField] private int defense = 0;
    public int currentHp;

    [Header("UI Visuals")]
    [SerializeField] private Slider hpSlider;
    [SerializeField] private TextMeshProUGUI hpText;

    void Start()
    {
        currentHp = maxHp;
        UpdateUI();
    }

    public void TakeDamage(int damage)
    {
        int finalDamage = Mathf.Max(1, damage - defense);
        currentHp -= finalDamage;
        UpdateUI();

        if (currentHp <= 0)
        {
            currentHp = 0;
            if (GameManager.Instance != null) GameManager.Instance.TriggerGameOver();
            gameObject.SetActive(false);
        }
    }

    private void UpdateUI()
    {
        if (hpSlider) { hpSlider.maxValue = maxHp; hpSlider.value = currentHp; }
        if (hpText) hpText.text = $"{currentHp}/{maxHp}";
    }

    public float GetHpPercent() => (float)currentHp / maxHp;
}