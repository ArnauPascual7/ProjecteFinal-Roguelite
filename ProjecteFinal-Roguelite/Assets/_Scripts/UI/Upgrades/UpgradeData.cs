using UnityEngine;

[CreateAssetMenu(fileName = "NewUpgrade", menuName = "Roguelite/Upgrade Data")]
public class UpgradeData : ScriptableObject
{
    public string upgradeName;
    [TextArea] public string description;
    public Sprite icon;

    [Header("Descriptions per Level")]
    public string[] levelDescriptions;

    [Header("Levels Configuration")]
    public float[] values;
    public int[] costs;   // Cost de cada nivell
}
