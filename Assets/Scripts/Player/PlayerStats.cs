using System;
using System.Collections;
using UnityEngine;

public class PlayerStats : SingletonPersistent<PlayerStats>
{
    #region Parameters

    [Header("Player Stats SO")]
    [SerializeField] private PlayerStatsSO playerStatsSO;
    
    public float Health { get; private set; }
    public float MaxHealth { get; private set; }
    public int Defense { get; private set; }
    public float Damage { get; private set; }
    public float Speed { get; private set; }

    //TODO: UI of stats

    // Events
    public event Action OnStatsChanged;

    #endregion

    #region Excute

    protected override void Awake()
    {
        base.Awake();
        InitStat();
    }

    #endregion

    #region Init

    // TODO: Pass the stat in this func
    private void InitStat()
    {
        Health = playerStatsSO.health;
        MaxHealth = playerStatsSO.health;
        Defense = playerStatsSO.defense;
    }
    
    #endregion
    
    #region Apply Item

    public void ApplyItem(ItemSO itemSO)
    {
        bool changed = false; 

        if(itemSO.health > 0)
        {
            UpdateHealth(itemSO.health);
            changed = true;
        }    

        // if(itemSO.damage > 0)
        //     Damage += itemSO.damage;

        // if(itemSO.speed > 0)
        //     Speed += itemSO.speed;

        if (changed)
        {
            OnStatsChanged?.Invoke();
        }

        if(itemSO.duration > 0)
        {
            // TODO: Temporary apply
        }
    }

    private IEnumerator TemporayBuff(ItemSO itemSO)
    {
        OnStatsChanged?.Invoke();

        yield return new WaitForSeconds(itemSO.duration);

        OnStatsChanged?.Invoke();
    }
    
    #endregion

    #region Update UI

    private void UpdateHealth(float amount)
    {
        Health += amount;
    }

    private void UpdateDamage(float amout)
    {
        
    }
    
    #endregion
}