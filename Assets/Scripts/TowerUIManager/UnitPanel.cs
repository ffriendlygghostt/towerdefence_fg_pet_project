using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitPanel : MonoBehaviour
{
    [Header("Main Fields")]
    public Image iconUnit;
    public TextMeshProUGUI lvl_txt;
    public TextMeshProUGUI speed_txt;
    public TextMeshProUGUI damage_txt;

    [Header("ButtonBuy")]
    public Button buyUnitButton;
    public TextMeshProUGUI buyUnitButton_txt;
    private Image buyUnitBI;

    [Header("ButtonUpgrade")]
    public Button upgradeUnitButton;
    public TextMeshProUGUI upgradeUnitButton_txt;
    private Image upgradeUnitBI;

    [Header("Colors Button")]
    public Color buttonColorActive;
    public Color buttonTextColorActive;
    public Color maxLevelButtonColor;

    [Header("Colors Text")]
    public Color nextLevelColor;
    public Color currentLevelColor;
    public Color maxLevelColor;
    public Color maxLevelButtonTextColor;
    private string hexNextLevelColor;

    [Header("Other")]
    public GameObject unitsInfo;

    private Color defaultColorButton;
    private Color defaultColorTxtB;

    private Vector3 buyButtonScale;
    private Vector3 upButtonScale;

    private UnitDefinition unitDef;
    private UnitLevelData currentLevelData;
    private UnitLevelData nextLevelData;
    private int unitLevel;

    private bool isHoverUp = false;
    private bool isHoverBuy = false;

    private int panelIndex;
    public event Action<int> UnitPanelOnClickBuy;
    public event Action<int> UnitPanelOnClickUp;

    public event Action UnitPanelOnClickCantBuy;
    public event Action UnitPanelOnClickCantUp;
    public event Action UnitPanelHoverEnter;

    private void Awake()
    {
        buyUnitBI = buyUnitButton.GetComponent<Image>();
        upgradeUnitBI = upgradeUnitButton.GetComponent<Image>();

        defaultColorButton = buyUnitBI.color;
        defaultColorTxtB = buyUnitButton_txt.color;

        buyButtonScale = buyUnitBI.rectTransform.localScale;
        upButtonScale = upgradeUnitBI.rectTransform.localScale;

        hexNextLevelColor = ColorUtility.ToHtmlStringRGB(nextLevelColor);
    }
    private void Start()
    {
        WalletManager.Instance.OnCoinsChanged += UpdateBuyButton;
        WalletManager.Instance.OnCoinsChanged += UpdateUpButton;
    }

    public void SetUnit(UnitDefinition unitDef, int level, int indexPanel)
    {
        ResetHoverState();

        this.unitDef = unitDef;
        unitLevel = level;
        panelIndex = indexPanel;
        UpdateUnit();
    }

    private void UpdateLevelData()
    {
        bool hasUnit = unitLevel > 0;

        currentLevelData = hasUnit
            ? unitDef.unitLevelData[unitLevel - 1]
            : null;

        nextLevelData = unitLevel < unitDef.unitLevelData.Length
            ? unitDef.unitLevelData[unitLevel]
            : null;

        iconUnit.sprite = hasUnit
            ? currentLevelData.icon
            : nextLevelData.icon;
    }

    private void UpdateUnit()
    {
        UpdateLevelData();

        if (unitLevel == 0)
        {

            unitsInfo.SetActive(false);

            UpdateBuyButton();
            buyUnitButton_txt.text = nextLevelData.cost.ToString();
            return;
        }

        buyUnitButton.gameObject.SetActive(false);
        unitsInfo.SetActive(true);

        if (nextLevelData == null)
        {
            SetMaxLevelStats();
            return;
        }

        SetCurrentLevelStats();
        upgradeUnitButton_txt.text = nextLevelData.cost.ToString();

        UpdateUpButton();

    }

    private void UpdateBuyButton(int x)
    {
        UpdateBuyButton();
    }
    private void UpdateBuyButton()
    {
        if (unitLevel > 0 || nextLevelData == null)
            return;

        bool canBuy = CanBuy();

        buyUnitBI.color = canBuy
            ? buttonColorActive 
            : defaultColorButton;

        buyUnitButton_txt.color = canBuy
            ? buttonTextColorActive 
            : defaultColorTxtB;

        if (isHoverBuy)
        {
            buyUnitButton_txt.text = canBuy
                ? "BUY"
                : "NOT ENOUGH MONEY";
        }
        else
        {
            buyUnitButton_txt.text = nextLevelData.cost.ToString();
        }
    }

    private void UpdateUpButton(int x)
    {
        UpdateUpButton();
    }
    private void UpdateUpButton()
    {
        if (unitLevel == 0 || nextLevelData == null)
            return;

        bool canUp = CanUpgrade();

        upgradeUnitBI.color = canUp 
            ? buttonColorActive 
            : defaultColorButton;

        upgradeUnitButton_txt.color = canUp 
            ? buttonTextColorActive 
            : defaultColorTxtB;

        if (isHoverUp)
            SetNextLevelStats();
    }

    private bool CanBuy()
    {
        return unitLevel == 0 &&
            nextLevelData != null &&
            WalletManager.Instance.Coins >= nextLevelData.cost;
    }

    private bool CanUpgrade()
    {
        return nextLevelData != null &&
            WalletManager.Instance.Coins >= nextLevelData.cost;
    }



    public void HoverButtonBuy()
    {
        UnitPanelHoverEnter?.Invoke();

        if (unitLevel > 0)
            return;

        isHoverBuy = true;

        buyUnitBI.rectTransform.DOKill();
        buyUnitBI.rectTransform
            .DOScale(buyButtonScale * 0.95f, 0.1f)
            .SetEase(Ease.OutQuad);

        buyUnitButton_txt.text = CanBuy()
            ? "BUY"
            : "NOT ENOUGH MONEY";

    }
    public void HoverExitButtonBuy()
    {
        if (unitLevel > 0)
            return;

        buyUnitBI.rectTransform.DOKill();
        buyUnitBI.rectTransform
            .DOScale(buyButtonScale, 0.4f)
            .SetEase(Ease.OutBack);

        buyUnitButton_txt.text = nextLevelData.cost.ToString();

        isHoverBuy = false;
    }
    public void ClickButtonBuy()
    {
        if (unitLevel > 0 || !CanBuy())
        {
            UnitPanelOnClickCantBuy?.Invoke();
            return;
        }

        UnitPanelOnClickBuy?.Invoke(panelIndex);
    }


    public void HoverButtonUp()
    {
        UnitPanelHoverEnter?.Invoke();

        if (nextLevelData == null)
            return;

        upgradeUnitBI.rectTransform.DOKill();
        upgradeUnitBI.rectTransform
            .DOScale(upButtonScale * 0.95f, 0.1f)
            .SetEase(Ease.OutBack);

        isHoverUp = true;

        bool canUpgrade = CanUpgrade();

        upgradeUnitButton_txt.text = canUpgrade
            ? "UPGR"
            : "NOT ENOUGH MONEY";

        if (canUpgrade)
            SetNextLevelStats();
    }
    public void HoverExitButtonUp()
    {
        if (nextLevelData == null)
            return;

        ResetUpButtonScaleAnim();

        SetCurrentLevelStats();

        upgradeUnitButton_txt.text = nextLevelData.cost.ToString();

        isHoverUp = false;
    }
    public void ClickButtonUp()
    {
        if (nextLevelData == null || !CanUpgrade())
        {
            UnitPanelOnClickCantUp?.Invoke();
            return;
        }

        UnitPanelOnClickUp?.Invoke(panelIndex);
    }

    private void SetCurrentLevelStats()
    {
        lvl_txt.text = unitLevel.ToString();
        speed_txt.text = $"{currentLevelData.attackCooldown}s";
        damage_txt.text = currentLevelData.damage.ToString();

        lvl_txt.color = currentLevelColor;
        speed_txt.color = currentLevelColor;
        damage_txt.color = currentLevelColor;
    }

    private void SetNextLevelStats()
    {
        if (nextLevelData == null || currentLevelData == null)
            return;

        lvl_txt.text =
            $"{unitLevel} <color=#{hexNextLevelColor}>→ {nextLevelData.level}</color>";
        speed_txt.text =
            $"{currentLevelData.attackCooldown}s <color=#{hexNextLevelColor}>→ {nextLevelData.attackCooldown}s</color>";
        damage_txt.text =
            $"{currentLevelData.damage} <color=#{hexNextLevelColor}>→ {nextLevelData.damage}</color>";
    }

    private void SetMaxLevelStats()
    {
        ResetUpButtonScaleAnim();
        SetCurrentLevelStats();

        lvl_txt.text = "MAX";
        upgradeUnitButton_txt.text = "MAX";

        lvl_txt.color = maxLevelColor;
        speed_txt.color = maxLevelColor;
        damage_txt.color = maxLevelColor;

        upgradeUnitBI.color = maxLevelButtonColor;
        upgradeUnitButton_txt.color = maxLevelButtonTextColor;

    }

    private void ResetUpButtonScaleAnim() 
    {
        if (!isHoverUp)
            return;
        upgradeUnitBI.rectTransform
                .DOScale(upButtonScale, 0.6f)
                .SetEase(Ease.OutBack);
    }

    public void ResetPanel()
    {
        unitsInfo.SetActive(false);
        buyUnitButton.gameObject.SetActive(true);

        iconUnit.sprite = null;

        lvl_txt.text = "";
        speed_txt.text = "";
        damage_txt.text = "";

        unitDef = null;
        unitLevel = 0;

        currentLevelData = null;
        nextLevelData = null;
    }

    private void ResetHoverState()
    {
        isHoverBuy = false;
        isHoverUp = false;

        buyUnitButton_txt.text = "";
        upgradeUnitButton_txt.text = "";
    }
}
