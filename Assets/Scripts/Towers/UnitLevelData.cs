using UnityEngine;

[System.Serializable]
public class UnitLevelData
{
    public Sprite icon;
    public int level;
    public int damage;
    public float speedAttackPerSec;
    public int cost;
    public RuntimeAnimatorController animationsByLevel;

}
