using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextFloorScreen : MonoBehaviour
{
    public Action OnEndRunWinMenu;
    public Button continueButton;

    [SerializeField] private List<ItemPanel> itemPanels;
    [SerializeField] private int itemAmount = 3;
    public int GetItemAmount() => itemAmount;
    public void SetItemAmount(int amount) => itemAmount = amount;

    private ItemPanel selectedPanel;
    private ArtefactSO selectedArtefactSO;


    private void Awake()
    {
        Hide();
        foreach (var itemPanel in itemPanels)
        {
            itemPanel.OnClicked += ClickedPanel;
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
        LoadNewItems();
    }

    public void Hide() =>
        gameObject.SetActive(false);

    public void ContinueButton()
    {
        if (continueButton == null) return;
        ArtefactManager.Instance.EquipArtefact(selectedArtefactSO);
        GameFlowManager.Instance.ContinueAfterArtifact();
    }
    public void EndRunButton() =>
        GameFlowManager.Instance.LeaveGame();

    public void LoadNewItems()
    {
        ClearPanes();
        var items = ArtefactManager.Instance.GetRandomArtefacts(itemAmount);

        int count = Mathf.Min(items.Count, itemPanels.Count);
        for (int i = 0; i < count; i++)
        {
            itemPanels[i].Initialize(items[i]);
        }
    }
    private void ClearPanes()
    {
        foreach (var item in itemPanels)
        {
            item.Clear();
        }
        selectedPanel = null;
        selectedArtefactSO = null;
        continueButton.interactable = false;
        
    }
    // Метод не очищает панели как объекты, поэтому отписки пока не требуются.
    // Панели по стандарту будет три. Для игры этого может быть достаточно (даже вне рамок демо).
    // Однако, логику можно будет слегка изменить, чтобы допустить возможность менять количество панелей
    //     динамически, по ходу забега. В таком случае отписки следует добавить сюда.


    private void ClickedPanel(ItemPanel panel)
    {
        if (panel == selectedPanel) { return; }
        selectedPanel = panel;
        continueButton.interactable = true;
        
        foreach (var item in itemPanels)
        {
            if (selectedPanel == item) 
            { 
                selectedPanel.SetSelected(true);
                selectedArtefactSO = selectedPanel.GetArtefact();
            }
            else item.SetSelected(false);
        }
    }

}
