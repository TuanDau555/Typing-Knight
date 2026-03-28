using System;
using UnityEngine;

public class PlayerStats : Singleton<PlayerStats>
{
    #region Parameters

    // TODO: Need a player Stats SO 
    // then pass the data to local stats down here
    public float Health { get; private set; }
    public float Damage { get; private set; }
    public float Speed { get; private set; }

    //TODO: UI of stats

    // Events
    public event Action OnStatsChanged;
    
    #endregion

    #region Excute

    private void Start()
    {
        InitStat();
    }

    #endregion

    #region Init

    // TODO: Pass the stat in this func
    private void InitStat()
    {
        
    }
    
    #endregion
    
    #region Apply Item

    public void ApplyItem(ItemSO itemSO)
    {
        if(itemSO.health > 0)
            UpdateHealth(itemSO.health);

        if(itemSO.damage > 0)
            Damage += itemSO.damage;

        if(itemSO.speed > 0)
            Speed += itemSO.speed;

        if(itemSO.duration > 0)
        {
            // TODO: Temporary apply
        }
    }

    #endregion

    #region Update UI

    private void UpdateHealth(float amount)
    {
        Health += amount;
        // TODO: UI here
    }

    private void UpdateDamage(float amout)
    {
        
    }
    
    #endregion
}