using UnityEngine;

[CreateAssetMenu(menuName = "Units/Unit Definition")]
public class UnitDefinition : ScriptableObject
{
    public string id;
    public UnitLevelData[] unitLevelData;
}
