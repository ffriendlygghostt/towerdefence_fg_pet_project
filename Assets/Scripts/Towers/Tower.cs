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
    [SerializeField] private Transform visualRoot;

    private TowerVisual currentVisual;

    [HideInInspector] public UnitDefinition unitDef;
    [HideInInspector] public List<int> unitLevelsBySlot = new();

    private Vector3 scale;
    private float hoverScale;
    [SerializeField] private float hoverScaleMultiply = 1.08f;
    [SerializeField] private float durationScale = 0.15f;

    private void Start()
    {
        scale = visualRoot.localScale;
        hoverScale = scale.x * hoverScaleMultiply;
    }

    public bool TryUpgradeTower()
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
        RefreshVisual();
        RestoreUnits();

        if (currentLevel != towerDef.levels.Length)
        {
            AudioManager.Instance.PlaySFXType(SoundDefaultEnum.TowerUpgrade);
        }
        else
        {
            AudioManager.Instance.PlaySFXType(SoundDefaultEnum.TowerUpgradeMax);
        }


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

        UpdateCurrentTowerRange();
        RefreshVisual();
        GiveStartUnit();
        RestoreUnits();
        HideRange();
    } 

    private void GiveStartUnit()
    {
        unitLevelsBySlot[0] = 1;
        currentUnitsCount++;
    }

    public bool TryUnitBuy(int index)
    {
        if (unitLevelsBySlot[index] != 0)
            return false;
        if (index >= towerDef.levels[currentLevel-1].unitCapacity)
            return false;

        if (!WalletManager.Instance.TrySpend(unitDef.unitLevelData[0].cost))
            return false;

        unitLevelsBySlot[index] = 1;

        currentUnitsCount++;
        currentVisual.AddUnit(
            index,
            unitDef.unitLevelData[0].unitPrefab,
            unitDef.unitLevelData[0]
            );

        AudioManager.Instance.PlaySFXType(SoundDefaultEnum.UnitBuy);

        return true;
    }

    public bool TryUnitUpgrade(int index)
    {
        if (unitLevelsBySlot[index] == 0)
            return false;
        if (unitLevelsBySlot[index] >= unitDef.unitLevelData.Length)
            return false;

        int nextLevel = unitLevelsBySlot[index];
        if (!WalletManager.Instance.TrySpend(unitDef.unitLevelData[nextLevel].cost))
            return false;

        unitLevelsBySlot[index]++;

        var levelData = unitDef.unitLevelData[unitLevelsBySlot[index] - 1];

        currentVisual.UpgradeUnit(
            index,
            unitDef.unitLevelData[unitLevelsBySlot[index] - 1].unitPrefab,
            levelData
            );

        if (unitLevelsBySlot[index] == unitDef.unitLevelData.Length)
        {
            AudioManager.Instance.PlaySFXType(SoundDefaultEnum.UnitMaxUpgrade);
        }
        else
        {
            AudioManager.Instance.PlaySFXType(SoundDefaultEnum.UnitUpgrade);
        }

        return true;
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
        visualRoot.transform.DOKill();
        visualRoot.transform
            .DOScale(hoverScale, durationScale)
            .SetEase(Ease.OutQuad);
    }
    public void HoverExit()
    {
        visualRoot.transform.DOKill();
        visualRoot.transform
            .DOScale(scale, durationScale)
            .SetEase(Ease.OutBack);
    }

    private void RefreshVisual()
    {
        if (currentVisual != null)
        {
            Destroy(currentVisual.gameObject);
        }

        currentVisual = Instantiate(
            towerDef.levels[currentLevel - 1].towerVisual,
            visualRoot)
            .GetComponent<TowerVisual>();

        currentVisual.Initialize(
            this,
            range);
    }

    public void RestoreUnits()
    {
        for (int i = 0; i < unitLevelsBySlot.Count; i++)
        {
            if (unitLevelsBySlot[i] == 0)
                continue;

            UnitLevelData levelData =
                unitDef.unitLevelData[unitLevelsBySlot[i] - 1];

            currentVisual.AddUnit(
                i,
                unitDef.unitLevelData[unitLevelsBySlot[i] - 1].unitPrefab,
                levelData
                );
        }
    }
}