using DG.Tweening;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Plate : MonoBehaviour,
                IPointerEnterHandler,
                IPointerExitHandler,
                IPointerClickHandler
{
    public Tower CurrentTower { get; private set; }
    public bool IsEmpty => CurrentTower == null;

    public CircleCollider2D radius;
    public GameObject plateBuildings;

    public SpriteRenderer previewTower;
    public float delayShowGhostTower = 0.2f;

    public float hoverScaleMultiply = 1.08f;
    public float scaleDuration = 0.15f;

    private float scale;
    private float hoverScale;

    private WaitForSeconds waitSpawnGhostTower;
    private Coroutine WaitSpawnGhostTowerRoutine;

    public GameObject towerPrefab;

    public Transform previewRange;

    private bool rangeIsActive;

    private bool hoverRangeVisible;
    private bool selectedRangeVisible;

    private void ValidateComponents()
    {
        Debug.Assert(previewRange != null);
        Debug.Assert(previewTower != null);
    }

    private void Start()
    {
        ValidateComponents();

        previewTower.enabled = false;

        waitSpawnGhostTower = new WaitForSeconds(delayShowGhostTower);
        scale = plateBuildings.transform.localScale.x * hoverScaleMultiply;
        hoverScale = scale * hoverScaleMultiply;

        HidePreviewRange();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!IsEmpty)
        {
            hoverRangeVisible = true;
            UpdateTowerRange();

            CurrentTower.HoverEnter();
        }
        else
        {
            WaitSpawnGhostTowerRoutine = StartCoroutine(WaitSpawnGhostTower());

            plateBuildings.transform.DOKill();
            plateBuildings.transform
                .DOScale(hoverScale, scaleDuration)
                .SetEase(Ease.OutQuad);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!IsEmpty)
        {
            hoverRangeVisible = false;
            UpdateTowerRange();

            CurrentTower.HoverExit();
        }
        else
        {
            plateBuildings.transform.DOKill();
            plateBuildings.transform
                .DOScale(scale, scaleDuration)
                .SetEase(Ease.InQuad);

            if (WaitSpawnGhostTowerRoutine != null)
            {
                StopCoroutine(WaitSpawnGhostTowerRoutine);
                WaitSpawnGhostTowerRoutine = null;
            }
        }  
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (IsEmpty)
            TowerUIManager.Instance.ToggleBuyMenu(this);
        else
        {
            TowerUIManager.Instance.ToggleInfoMenu(this);
        }

    }

    private IEnumerator WaitSpawnGhostTower()
    {
        yield return waitSpawnGhostTower;

        previewTower.sprite = GhostTowersManager.Instance.GetRandomGhostSprite();
        previewTower.enabled = true;
    }

    public bool Build(TowerDefinition towerType)
    {
        if (!WalletManager.Instance.TrySpend(towerType.levels[0].cost))
            return false;

        HidePreviewRange();

        var tower = Instantiate(towerPrefab, transform);
        CurrentTower = tower.GetComponent<Tower>();

        CurrentTower.SetTowerDefinition(towerType);

        plateBuildings.SetActive(false);

        return true;
    }

    public void ShowPreviewRangeBuy(TowerDefinition tower)
    {
        previewRange.localScale = Vector3.one *
            tower.levels[0].radiusScale;
        rangeIsActive = true;
        previewRange.gameObject.SetActive(true);
    }

    public void ShowPreviewRange(float radius)
    {
        previewRange.gameObject.SetActive(true);
        previewRange.localScale = Vector3.one * radius;
    }

    public void HidePreviewRange()
    {
        previewRange.gameObject.SetActive(false);
    }

    private void UpdateTowerRange()
    {
        if (IsEmpty)
            return;

        if (hoverRangeVisible || selectedRangeVisible)
            CurrentTower.ShowRange();
        else
            CurrentTower.HideRange();
    }

    public void Select()
    {
        selectedRangeVisible = true;
        UpdateTowerRange();
        rangeIsActive = true;
    }

    public void Deselect()
    {
        selectedRangeVisible = false;
        UpdateTowerRange();
    }
}