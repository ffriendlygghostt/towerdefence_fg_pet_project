using TMPro;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class HudManager : Manager<HudManager>
{
    [SerializeField] private TMP_Text coinsText;
    [SerializeField] private TMP_Text healthText;

    [SerializeField] private GameObject prestartCanvas;

    public bool IsHudOn { get; private set; } = false;

    protected override void Awake()
    {
        base.Awake();
        Hide();
    }

    private void Start()
    {
        BaseManager.Instance.OnHealthChanged += UpdateHealthUI;
        WalletManager.Instance.OnCoinsChanged += UpdateCoinsUI;
    }

    private void UpdateHealthUI(float x, float health)
    {
        healthText.text = health.ToString();
    }

    private void UpdateCoinsUI(int coins)
    {
        coinsText.text = coins.ToString();
    }

    public void Show()
    {
        IsHudOn = true;
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        IsHudOn = false;
        this.gameObject.SetActive(false);
    }

    public void HidePrestart()
    {
        prestartCanvas.SetActive(false);
    }

    public void ShowPrestart()
    {
        prestartCanvas.SetActive(true);
    }

    private void OnDestroy()
    {
        if (BaseManager.Instance != null)
            BaseManager.Instance.OnHealthChanged -= UpdateHealthUI;

        if (WalletManager.Instance != null)
            WalletManager.Instance.OnCoinsChanged -= UpdateCoinsUI;
    }

}
