using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyMenu : MonoBehaviour
{
    [Header("TextFields")]
    [SerializeField] private TextMeshProUGUI unitCapacity_txt;
    [SerializeField] private TextMeshProUGUI speedAttack_txt;
    [SerializeField] private TextMeshProUGUI damage_txt;
    [SerializeField] private TextMeshProUGUI cost_txt;

    [Header("LibertyTower")]
    [SerializeField] private TowerCatalog catalog;

    [Header("Other")]
    [SerializeField] private Transform rootContent;
    [SerializeField] private GameObject prefabPlate;
    [SerializeField] private GameObject downInfo;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private Color normalTextColor;
    [SerializeField] private Color unknownTextColor;

    private List<BuyPlateTower> plates = new();
    private TowerDefinition selectedTower;

    public event Action<TowerDefinition> OnTowerSelected;
    public event Action<TowerDefinition> OnTowerHoverShowInfo;
    public event Action OnTowerHideInfo;


    public void Start()
    {
        CreateCatalog();
        HideInfo();

        WalletManager.Instance.OnCoinsChanged += UpdateCostText;
    }

    public void CreateCatalog()
    {
        if (catalog.towers.Count < 12)
        {
            for (int i = 0; i < 12; i++)
            {
                AddTowerPlate();
            }
        }
        else
        {
            foreach (var item in catalog.towers)
            {
                AddTowerPlate();
            }
        }

        for (int i = 0; i < plates.Count; i++) 
        {
            if (i >= catalog.towers.Count || catalog.towers[i] == null)
                plates[i].SetUnknown();
            else
                plates[i].SetTower(catalog.towers[i]);

            plates[i].OnHover += SetTower;
            plates[i].OnClick += BuyTower;
            plates[i].OnExit += HideInfo;
            plates[i].OnHoverIsUnknown += ShowUnknownInfo;
        }
    }

    private void AddTowerPlate()
    {
        var tp = Instantiate(prefabPlate, rootContent)
            .TryGetComponent<BuyPlateTower>(out var plate);

        plate.SetScrollRect(scrollRect);

        plates.Add(plate);
    }

    private void SetTower(TowerDefinition tower)
    {
        selectedTower = tower;
        ShowInfo();
    }

    private void HideInfo()
    {
        downInfo.SetActive(false);

        unitCapacity_txt.text = "";
        speedAttack_txt.text = "";
        damage_txt.text = "";
        cost_txt.text = "";

        selectedTower = null;
        OnTowerHideInfo?.Invoke();
    }


    private void ShowInfo()
    {
        downInfo.SetActive(true);

        unitCapacity_txt.color = normalTextColor;
        speedAttack_txt.color = normalTextColor;
        damage_txt.color = normalTextColor;

        unitCapacity_txt.text = selectedTower.levels[selectedTower.levels.Length-1].unitCapacity.ToString();
        speedAttack_txt.text = $"{selectedTower.unitType.unitLevelData[0].attackCooldown.ToString()}s";
        Debug.Log($"{selectedTower.unitType.unitLevelData[0].attackCooldown.ToString()}s");
        damage_txt.text = selectedTower.unitType.unitLevelData[0].damage.ToString();
        cost_txt.text = selectedTower.levels[0].cost.ToString();

        UpdateCostText();

        OnTowerHoverShowInfo?.Invoke(selectedTower);
    }

    private void ShowUnknownInfo()
    {
        downInfo.SetActive(true);
        
        unitCapacity_txt.text = "XXX";
        unitCapacity_txt.color = unknownTextColor;

        speedAttack_txt.text = "XXX";
        speedAttack_txt.color = unknownTextColor;

        damage_txt.text = "Unknown";
        damage_txt.color = unknownTextColor;

        cost_txt.text = "Unknown";
        cost_txt.color = unknownTextColor;

        OnTowerHoverShowInfo?.Invoke(null);
    }

    private void UpdateCostText(int x)
    {
        UpdateCostText();
    }
    private void UpdateCostText()
    {
        if (selectedTower == null)
            return;
        if (!CanBuy())
        {
            cost_txt.color = Color.red;
            return;
        }

        cost_txt.color = Color.orange;
    }
    private bool CanBuy()
    {
        return WalletManager.Instance.Coins >= selectedTower.levels[0].cost;
    }

    private void BuyTower(TowerDefinition tower)
    {
        OnTowerSelected?.Invoke(tower);
    }
}
