using DG.Tweening;
using UnityEngine;

public class TowerUIManager : Manager<TowerUIManager>
{
    [Header("Panels")]
    public GameObject buyPanel;
    public GameObject infoPanel;

    private BuyMenu buyMenu;
    private InfoMenu infoMenu;

    [Header("Offset")]
    [SerializeField] private float verticalOffset = 0.6f;
    [SerializeField] private float horizontalOffset = 1.5f;
    [SerializeField] private float topHudPadding = 0.05f;

    private Vector3 verticalOffsetV3;

    private RectTransform buyRect;
    private RectTransform infoRect;

    private CanvasGroup rootCG;
    private Canvas canvas;

    private Camera cam;

    private Vector3 buyDefaultScale;
    private Vector3 infoDefaultScale;

    private Plate currentPlate;

    protected override void Awake()
    {
        base.Awake();

        cam = Camera.main;

        buyRect = buyPanel.GetComponent<RectTransform>();
        infoRect = infoPanel.GetComponent<RectTransform>();

        buyDefaultScale = buyRect.localScale;
        infoDefaultScale = infoRect.localScale;

        rootCG = GetComponent<CanvasGroup>();
        canvas = GetComponent<Canvas>();

        verticalOffsetV3 = new(0, verticalOffset, 0);

        topHudPadding *= Screen.height;

        HideInstant();
    }

    private void Start()
    {
        WorldCameraService.Instance.UpdateActiveCam += SetWorldCamera;
        //InputManager.OnBackgroundClick += HideBuyMenu;
        //InputManager.OnBackgroundClick += HideInfoMenu;

        buyMenu = buyPanel.GetComponent<BuyMenu>();
        infoMenu = infoPanel.GetComponent<InfoMenu>();

        buyMenu.OnTowerSelected += tower =>
        {
            if (currentPlate.Build(tower))
                HideBuyMenu();
        };

        buyMenu.OnTowerHoverShowInfo += BuyMenuHoverTD;
        buyMenu.OnTowerHideInfo += BuyMenuExit;

        infoMenu.OnHoverUpgradeTower += OnHoveredUpgradeButton;
        infoMenu.OnExitUpgradeTower += OnExitedUpgradeButton;
        infoMenu.ClickUpgradeButton += OnClickUpgradeButton;
    }

    private void OnClickUpgradeButton(TowerLevelData levelData)
    {
        if (levelData == null)
            currentPlate.HidePreviewRange();
        else
            currentPlate.ShowPreviewRange(levelData.radiusScale);
    }

    private void HideInstant()
    {
        if (currentPlate != null)
        {
            currentPlate.Deselect();
            currentPlate = null;
        }

        buyPanel.SetActive(false);
        infoPanel.SetActive(false);
    }

    ////////////////////
    /////// BUY_MENU
    ////////////////////

    public void ToggleBuyMenu(Plate tower)
    {
        if (currentPlate == tower)
        {
            HideBuyMenu();
            return;
        }

        ShowBuyMenu(tower);
    }

    public void ShowBuyMenu(Plate tower)
    {
        HideInstant();
        currentPlate = tower;

        ShowPanel(buyPanel, buyRect, buyDefaultScale, tower);
    }

    public void HideBuyMenu()
    {
        HidePanel(buyPanel, buyRect, buyDefaultScale);
        currentPlate = null;
    }


    ////////////////////
    /////// INFO_MENU
    ////////////////////

    public void ToggleInfoMenu(Plate tower)
    {
        if (currentPlate == tower)
        {
            HideInfoMenu();
            return;
        }

        ShowInfoMenu(tower); 
    }

    public void ShowInfoMenu(Plate tower)
    {
        HideInstant();
        currentPlate = tower;
        ShowPanel(infoPanel, infoRect, infoDefaultScale, tower);

        infoMenu.SetTower(tower.CurrentTower);
        tower.Select();
    }

    public void HideInfoMenu()
    {
        HidePanel(infoPanel, infoRect, infoDefaultScale);

        if (currentPlate != null)
        {
            currentPlate.Deselect();
            currentPlate = null;
        }
    }

    private void ShowPanel(GameObject panelGO, RectTransform panel, Vector3 targetScale, Plate tower)
    {
        panelGO.SetActive(true);
        panel.localScale = targetScale;

        SetPanelPosition(panel, tower.transform);

        rootCG.alpha = 0;
        panel.localScale = targetScale * 0.7f;

        rootCG.blocksRaycasts = true;
        rootCG.interactable = true;

        rootCG.DOFade(1f, 0.25f);

        panel.DOKill();
        panel.DOScale(targetScale * 1.05f, 0.18f)
            .SetEase(Ease.OutBack)
            .OnComplete(() => panel.DOScale(targetScale, 0.08f));
    }

    private void HidePanel(GameObject panelGO, RectTransform panel, Vector3 targetScale)
    {
        panelGO.SetActive(false);
        rootCG.blocksRaycasts = false;
        rootCG.interactable = false;

        rootCG.DOFade(0f, 0.15f);

        panel.DOKill();
        panel.DOScale(targetScale * 0.8f, 0.15f);
    }

    private void SetPanelPosition(RectTransform panel, Transform tower)
    {
        panel.position = tower.position + verticalOffsetV3;

        Vector3 viewportPos = cam.WorldToViewportPoint(tower.position);

        panel.position += (viewportPos.x < 0.5f)
            ? Vector3.right * horizontalOffset
            : Vector3.left * horizontalOffset;

        Vector3[] corners = new Vector3[4];


        panel.GetWorldCorners(corners);

        float bottom = cam.WorldToScreenPoint(corners[0]).y;
        float top = cam.WorldToScreenPoint(corners[1]).y;

        float correction = 0;

        if (bottom < 0)
            correction = -bottom;
        else if (top > Screen.height)
            correction = (Screen.height - topHudPadding) - top;

        Vector3 worldUp = cam.transform.up;
        panel.position += worldUp * correction
            * (Vector3.Distance(cam.transform.position, panel.position) / cam.pixelHeight);
    }

    public void SetWorldCamera(Camera levelCamera)
    {
        cam = levelCamera;
        canvas.worldCamera = levelCamera;
    }

    private void BuyMenuHoverTD(TowerDefinition tower)
    {
        currentPlate.ShowPreviewRangeBuy(tower);
    }

    private void BuyMenuExit()
    { 
        currentPlate.HidePreviewRange();
    }
        

    private void OnHoveredUpgradeButton(float radius)
    {
        currentPlate.ShowPreviewRange(radius);
    }

    private void OnExitedUpgradeButton()
    {
        currentPlate.HidePreviewRange();
    }
}