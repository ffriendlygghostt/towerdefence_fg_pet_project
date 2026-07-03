using UnityEngine;

[System.Serializable]
public class TowerLevelData 
{
    public int level;

    public float radiusScale;
    public float attackRadiusText => radiusScale * 100;

    public int unitCapacity;

    public int cost;

    public RuntimeAnimatorController idlePrefab;

    public Sprite towerVisual;
}
