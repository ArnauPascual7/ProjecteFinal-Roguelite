using System.Data;
using UnityEngine;

public enum StatType 
{
    MaxHealth, 
    MoveSpeed, 
    MaxEnergy, 
    EnergyRegen, 
    DashCooldown, 
    AttackDamage, 
    // 
}

[CreateAssetMenu(fileName = "NewUpgrade", menuName = "Roguelite/Upgrade Data")]
public class UpgradeData : ScriptableObject
{
    public string upgradeName;
    public StatType statType;
    public Sprite icon;

    [Header("Descriptions per Level")]
    public string[] levelDescriptions;

    [Header("Levels Configuration")]
    public float[] values;
    public int[] costs;

    [Header("Player Level Requirements")]
    public int[] playerLevelRequired;
}
