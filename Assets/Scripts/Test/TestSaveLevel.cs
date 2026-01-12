using UnityEngine;

public class TestSaveLevel : MonoBehaviour, ISaveable
{
    [SerializeField] private int level; 
    [SerializeField] private int money;
    
    #region Save/Load
    public void Load(SaveData data)
    {
        level = data.Get<int>("TestSaveLevel", level);
        money = data.Get<int>("TestSaveMoney", money);
    }

    public void Save(SaveData data)
    {
        data.Set("TestSaveLevel", level);
        data.Set("TestSaveMoney", money);
    }
    #endregion
}