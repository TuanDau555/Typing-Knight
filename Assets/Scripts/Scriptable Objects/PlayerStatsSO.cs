using UnityEngine;

[CreateAssetMenu(menuName = "PlayerStatsSO")]
public class PlayerStatsSO : ScriptableObject 
{
    [Header("General")]
    [Range(1, 100)]
    public float health;
    public int defense;
}