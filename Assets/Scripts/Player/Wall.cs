using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Wall : MonoBehaviour
{

    [Header("UI Visuals")]
    [SerializeField] private Slider hpSlider;
    [SerializeField] private TextMeshProUGUI hpText;

    private int defense = 0;
    private float _lastKnowHealth; // The health value before the buff
    public float currentHp { get; private set; }

    void Start()
    {
        currentHp = PlayerStats.Instance.Health;
        defense = PlayerStats.Instance.Defense;
        _lastKnowHealth = PlayerStats.Instance.Health;

        PlayerStats.Instance.OnStatsChanged += OnStatsBuff;
        
        UpdateUI();
    }

    private void OnDestroy()
    {
        PlayerStats.Instance.OnStatsChanged -= OnStatsBuff;
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
        if (hpSlider)
        {
            hpSlider.maxValue = PlayerStats.Instance.MaxHealth; 
            hpSlider.value = currentHp;
        }

        if (hpText)
        {
            hpText.text = $"{currentHp}/{PlayerStats.Instance.MaxHealth}";
        }
    }

    private void OnStatsBuff()
    {
        float newHealth = PlayerStats.Instance.Health;
        currentHp = Mathf.Min(currentHp + (newHealth - _lastKnowHealth), PlayerStats.Instance.MaxHealth);
        UpdateUI();
    }

    public float GetHpPercent() => (float)currentHp / PlayerStats.Instance.MaxHealth;
}