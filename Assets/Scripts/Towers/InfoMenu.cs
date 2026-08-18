using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoMenu : MonoBehaviour
{
    [Header("Fields_Stats")]
    public Image iconTower;
    public TextMeshProUGUI level_txt;
    public TextMeshProUGUI unitsCount_txt;
    public TextMeshProUGUI radius_txt;


    [Header("Button")]
    public Button upgradeButton;
    public TextMeshProUGUI cost_txt;
    public Color upgradeButtonColor;
    public Color upgradeButtonTextColor;
    public Color maxButtonColor;
    private Image buttonImage;
    private Color defaultButtonColor;
    private Color defaultButtonTextColor;
    private Vector3 defaultScaleButton;

    [Header("Text Colors")]
    public Color defaultTextColor;
    public Color nextLevelColor;
    public Color maxLevelColorText;
    public Color maxButtonTextColor;

    [Header("Scroll_View")]
    public Transform contentRoot;
    public GameObject prefabPanelUnits;
    private List<UnitPanel> unitPanels = new();

    private Tower selectedTower;
    private TowerLevelData nextLevelData;
    private TowerLevelData currentLevelData;

    bool isHoverButton = false;

    private string hexNextLevelColor;

    public event Action<float> OnHoverUpgradeTower;
    public event Action OnExitUpgradeTower;
    public event Action<TowerLevelData> ClickUpgradeButton;
    public event Action OnClickCantUpgrade;

    public event Action OnCantBuyUnit;
    public event Action OnCantUpUnit;
    public event Action OnHoverUnit;

    private void Awake()
    {
        buttonImage = upgradeButton.GetComponent<Image>();
        defaultButtonColor = buttonImage.color;
        defaultButtonTextColor = cost_txt.color;

        defaultScaleButton = buttonImage.rectTransform.localScale;

        hexNextLevelColor = ColorUtility.ToHtmlStringRGB(nextLevelColor);
    }
    private void Start()
    {
        WalletManager.Instance.OnCoinsChanged += UpdateButtonUI;
        WalletManager.Instance.OnCoinsChanged += UpdateButtonText;

        for (int i = 0; i < 5; i++)
            CreatePanel();
    }

    public void SetTower(Tower tower)
    {
        ResetInfoMenu();
        selectedTower = tower;
        UpdateInfo();
    }

    public void UpdateInfo()
    {
        currentLevelData = selectedTower.towerDef.levels[selectedTower.currentLevel - 1];

        if (selectedTower.currentLevel < selectedTower.towerDef.levels.Length)
        {
            nextLevelData = selectedTower.towerDef.levels[selectedTower.currentLevel];
        }
        else
        {
            nextLevelData = null;
        }
        
        iconTower.sprite = selectedTower.towerDef.icon;


        level_txt.text = selectedTower.currentLevel.ToString();

        radius_txt.text = currentLevelData.attackRadiusText.ToString();
        unitsCount_txt.text = currentLevelData.unitCapacity.ToString();
        
        if (nextLevelData == null)
        {
            SetMaxLevelStats();
        }
        else
        {
            cost_txt.text = nextLevelData.cost.ToString();
        }

        UpdateButtonUI();
        UpdateScrollRect();
    }

    private bool CanUpgrade()
    {
        return nextLevelData != null &&
            WalletManager.Instance.Coins >= nextLevelData.cost;
    }

    private void UpdateButtonUI(int x)
    {
        UpdateButtonUI();
    }

    private void UpdateButtonUI()
    {
        if (nextLevelData == null)
            return;

        bool canUpgrade = CanUpgrade();

        buttonImage.color = canUpgrade
            ? upgradeButtonColor
            : defaultButtonColor;

        cost_txt.color = canUpgrade
            ? upgradeButtonTextColor
            : defaultButtonTextColor;

        if (isHoverButton)
            SetNextLevelStats();
    }

    private void UpdateButtonText(int x)
    {
        UpdateButtonText();
    } 
    private void UpdateButtonText()
    {
        if (!isHoverButton || nextLevelData == null)
            return;

        cost_txt.text = CanUpgrade()  ? "UPGRADE" : "NOT ENOUGH MONEY";
    }


    public void HoverButtonUpgradeTower()
    {
        if (nextLevelData == null)
            return;

        isHoverButton = true;
        UpdateButtonText();

        if (CanUpgrade())
            SetNextLevelStats();

        buttonImage.rectTransform.DOKill();
        buttonImage.rectTransform
            .DOScale(defaultScaleButton * 0.95f, 0.1f)
            .SetEase(Ease.OutQuad);

        OnHoverUpgradeTower?.Invoke(nextLevelData.radiusScale);
    }
    public void HoverExitButtonUpgrade()
    {
        if (selectedTower == null)
            return;

        ResetButtonScaleAnim();

        OnExitUpgradeTower?.Invoke();


        if (nextLevelData == null)
            return;       

        SetCurrentLevelStats();
        cost_txt.text = nextLevelData.cost.ToString();
    }
    public void ClickButton()
    {
        if (!CanUpgrade())
        {
            OnClickCantUpgrade?.Invoke();
            return;
        }
            

        if (selectedTower.UpgradeTower())
        {
            UpdateInfo();
            if (nextLevelData != null)
                ClickUpgradeButton?.Invoke(nextLevelData);
            else
                ClickUpgradeButton?.Invoke(null);
        }

    }

    private void UpdateScrollRect()
    {
        while (unitPanels.Count < selectedTower.unitLevelsBySlot.Count)
        {
            CreatePanel();
        }
        for (int i = 0; i != selectedTower.unitLevelsBySlot.Count; i++)
        {
            unitPanels[i].gameObject.SetActive(true);
            unitPanels[i].SetUnit(selectedTower.unitDef, selectedTower.unitLevelsBySlot[i], i);
        }
    }

    private UnitPanel CreatePanel()
    {
        var panel = Instantiate(prefabPanelUnits, contentRoot)
            .GetComponent<UnitPanel>();

        panel.gameObject.SetActive(false);

        panel.UnitPanelOnClickBuy += BuyUnit;
        panel.UnitPanelOnClickUp += UpUnit;

        panel.UnitPanelOnClickCantBuy += CantBuyUnit;
        panel.UnitPanelOnClickCantUp += CantUpUnit;
        panel.UnitPanelHoverEnter += HoverUnit;

        unitPanels.Add(panel);

        return panel;
    }

    private void BuyUnit(int index)
    {
        if (selectedTower.UnitBuy(index))
            unitPanels[index].SetUnit(
                selectedTower.unitDef,
                selectedTower.unitLevelsBySlot[index],
                index);
    }

    private void UpUnit(int index)
    {
        if (selectedTower.UnitUpgrade(index))
            unitPanels[index].SetUnit(
                selectedTower.unitDef,
                selectedTower.unitLevelsBySlot[index],
                index);
    }

    private void CantBuyUnit()
    {
        OnCantBuyUnit?.Invoke();
    }
    private void CantUpUnit()
    {
        OnCantUpUnit?.Invoke();
    }
    private void HoverUnit()
    {
        OnHoverUnit?.Invoke();
    }





    private void ResetInfoMenu()
    {
        foreach (var unit in unitPanels)
        {
            unit.ResetPanel();
            unit.gameObject.SetActive(false);
        }

        iconTower.sprite = null;
        selectedTower = null;
        level_txt.text = "";

        nextLevelData = null;
        radius_txt.text = "";
        unitsCount_txt.text = "";

        cost_txt.text = "";
    }

    private void SetCurrentLevelStats()
    {
        level_txt.text = selectedTower.currentLevel.ToString();
        radius_txt.text = currentLevelData.attackRadiusText.ToString();
        unitsCount_txt.text = currentLevelData.unitCapacity.ToString();

        level_txt.color = defaultTextColor;
        radius_txt.color = defaultTextColor;
        unitsCount_txt.color = defaultTextColor;
    }

    private void SetNextLevelStats()
    {
        if (nextLevelData == null)
            return;
        level_txt.text =
            $"{selectedTower.currentLevel} <color=#{hexNextLevelColor}>→ {selectedTower.currentLevel + 1}</color>";
        radius_txt.text =
            $"{currentLevelData.attackRadiusText} <color=#{hexNextLevelColor}>→ {nextLevelData.attackRadiusText}</color>";
        unitsCount_txt.text =
            $"{currentLevelData.unitCapacity} <color=#{hexNextLevelColor}>→ {nextLevelData.unitCapacity}</color>";
    }

    private void SetMaxLevelStats()
    {
        ResetButtonScaleAnim();

        SetCurrentLevelStats();

        level_txt.text = "MAX";
        cost_txt.text = "MAX";

        level_txt.color = maxLevelColorText;
        radius_txt.color = maxLevelColorText;
        unitsCount_txt.color = maxLevelColorText;

        buttonImage.color = maxButtonColor;
        cost_txt.color = maxButtonTextColor;
    }

    private void ResetButtonScaleAnim()
    {
        if (isHoverButton)
        {
            isHoverButton = false;

            buttonImage.rectTransform.DOKill();
            buttonImage.rectTransform
                .DOScale(defaultScaleButton, 0.1f)
                .SetEase(Ease.OutBack);
        }
    }
}
