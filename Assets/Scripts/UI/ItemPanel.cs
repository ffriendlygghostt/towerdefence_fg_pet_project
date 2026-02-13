using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemPanel : MonoBehaviour
{
    public Action<ItemPanel> OnClicked;

    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text selectText;
    [SerializeField] private GameObject dimOverlay;

    [SerializeField] private Color selectColor;
    [SerializeField] private Color selectedColor;

    private ArtefactSO artefact;

    public void Initialize(ArtefactSO data)
    {
        artefact = data;
        nameText.text = data.name;
        descriptionText.text = data.description;
        iconImage.sprite = data.icon;

        SetSelected(false);
        dimOverlay.SetActive(false);
    }

    public void Click()
    {
        OnClicked?.Invoke(this);
    }

    public void SetSelected(bool selected)
    {
        selectText.text = selected ? "SELECTED" : "SELECT";
        selectText.color = selected ? selectedColor : selectColor;
        dimOverlay.SetActive(!selected);
    }

    public ArtefactSO GetArtefact() => artefact;

    public void Clear()
    {
        nameText.text = "";
        descriptionText.text = "";
        iconImage.sprite = null;
        SetSelected(false);
        selectText.text = "SELECT";
        selectText.color = selectColor;
        dimOverlay.SetActive(false);
    }

}
