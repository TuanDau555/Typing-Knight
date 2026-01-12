
/// <summary>
/// Implement this interface to the object that need to save
/// </summary>
public interface ISaveable
{
    void Save(SaveData data);
    void Load(SaveData data);
}