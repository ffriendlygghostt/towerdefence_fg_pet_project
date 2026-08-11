using UnityEngine;

[CreateAssetMenu(menuName = "Towers/Tower Definition")]
public class TowerDefinition : ScriptableObject
{
    public string id;
    public Sprite icon;

    public TowerLevelData[] levels;

    public UnitDefinition unitType;
}
