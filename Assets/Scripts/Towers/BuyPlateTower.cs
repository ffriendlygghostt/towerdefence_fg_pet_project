using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuyPlateTower : MonoBehaviour, 
    IPointerEnterHandler, 
    IPointerExitHandler, 
    IPointerClickHandler,
    IScrollHandler
{
    public Sprite unknownTower;
    public Image icon;
    private bool isUnknown;

    private TowerDefinition towerType;

    private Vector3 defaultScale;

    public event Action<TowerDefinition> OnHover;
    public event Action OnExit;
    public event Action<TowerDefinition> OnClick;
    public event Action OnHoverIsUnknown;

    private ScrollRect scrollRect;

    private void Start()
    {
        defaultScale = icon.rectTransform.localScale;
    }
    public void OnHoverButton()
    {
        icon.rectTransform.DOKill();
        icon.rectTransform
            .DOScale(defaultScale * 0.95f, 0.1f)
            .SetEase(Ease.OutQuad);

        if (isUnknown)
            OnHoverIsUnknown?.Invoke();
        else
            OnHover?.Invoke(towerType);
    }

    public void OnExitButton()
    {
        icon.rectTransform.DOKill();
        icon.rectTransform
            .DOScale(defaultScale, 0.1f)
            .SetEase(Ease.OutBack);

        OnExit?.Invoke();
    }

    public void OnClickButton()
    {
        if (isUnknown)
            return;
        OnClick?.Invoke(towerType);
    }

    public void SetUnknown()
    {
        icon.sprite = unknownTower;
        isUnknown = true;
    }
    public void SetTower(TowerDefinition tower)
    {
        this.towerType = tower;
        icon.sprite = tower.icon;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnHoverButton();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnExitButton();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClickButton();
    }

    public void SetScrollRect(ScrollRect rect)
    {
        scrollRect = rect;
    }
    public void OnScroll(PointerEventData eventData)
    {
        scrollRect.OnScroll(eventData);
    }
}
