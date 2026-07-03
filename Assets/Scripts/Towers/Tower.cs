using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [HideInInspector] public TowerDefinition towerDef;
    [HideInInspector] public int currentLevel;
    [HideInInspector] public int currentUnitsCount;

    [SerializeField] private TowerRange range;
    [SerializeField] private Transform graphics;

    [HideInInspector] public UnitDefinition unitDef;
    [HideInInspector] public List<int> unitLevelsBySlot = new();

    private Vector3 scale;
    private float hoverScale;
    [SerializeField] private float hoverScaleMultiply = 1.08f;
    [SerializeField] private float durationScale = 0.15f;

    private void Start()
    {
        scale = graphics.localScale;
        hoverScale = scale.x * hoverScaleMultiply;
    }

    public bool UpgradeTower()
    {
        if (!WalletManager.Instance.TrySpend(
            towerDef.levels[currentLevel].cost))
            return false;

        currentLevel++;

        int capacity = towerDef.levels[currentLevel - 1].unitCapacity;

        while(unitLevelsBySlot.Count < capacity)
        {
            unitLevelsBySlot.Add(0);
        }

        UpdateCurrentTowerRange();

        return true;
    }

    public void SetTowerDefinition(TowerDefinition towerDef)
    {
        this.towerDef = towerDef;
        unitDef = towerDef.unitType;

        currentLevel = 1;                        //* GameModifiers.Instance.StartTowerLvl;
        unitLevelsBySlot.Clear();

        int capacity = towerDef.levels[0].unitCapacity;

        for (int i=0; i < capacity; i++)
        {
            unitLevelsBySlot.Add(0);
        }
        GiveStartUnit();
        UpdateCurrentTowerRange();
        HideRange();
    } 

    private void GiveStartUnit()
    {
        unitLevelsBySlot[0] = 1;
        currentUnitsCount++;
    }

    public bool UnitBuy(int index)
    {
        if (unitLevelsBySlot[index] != 0)
            return false;
        if (index >= towerDef.levels[currentLevel-1].unitCapacity)
            return false;

        if (!WalletManager.Instance.TrySpend(unitDef.unitLevelData[0].cost))
            return false;

        unitLevelsBySlot[index] = 1;

        return true;
    }

    public bool UnitUpgrade(int index)
    {
        if (unitLevelsBySlot[index] == 0)
            return false;
        if (unitLevelsBySlot[index] >= unitDef.unitLevelData.Length)
            return false;

        int nextLevel = unitLevelsBySlot[index];
        if (!WalletManager.Instance.TrySpend(unitDef.unitLevelData[nextLevel].cost))
            return false;

        unitLevelsBySlot[index]++;
        return true;
    }

    private void AddUnit()
    {
        ///
    }

    public void ShowRange()
    {
        range.Show();
    }
    public void HideRange()
    {
        range.Hide();
    }
    public void UpdateCurrentTowerRange()
    {
        range.SetRadius(towerDef.levels[currentLevel - 1].radiusScale);
    }
    public void SetRangeScale(float radius)
    {
        range.SetRadius(radius);
    }


    public void HoverEnter()
    {
        graphics.transform.DOKill();
        graphics.transform
            .DOScale(hoverScale, durationScale)
            .SetEase(Ease.OutQuad);
    }
    public void HoverExit()
    {
        graphics.transform.DOKill();
        graphics.transform
            .DOScale(scale, durationScale)
            .SetEase(Ease.OutBack);
    }


}