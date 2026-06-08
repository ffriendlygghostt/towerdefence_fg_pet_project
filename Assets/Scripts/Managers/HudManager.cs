using System.Collections;
using TMPro;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;

public class HudManager : Manager<HudManager>
{
    [SerializeField] private TMP_Text coinsText;
    [SerializeField] private TMP_Text healthText;

    [SerializeField] private GameObject prestartCanvas;
    [SerializeField] private GameObject defeatScreenCanvas;
    [SerializeField] private GameObject nextFloorCanvas;

    [SerializeField] private GameObject timerWaveCanvas;
    [SerializeField] private TextMeshProUGUI timerTXT;

    [SerializeField] private WaveHud waveHud;

    private PrestartUI prestartUI;
    private DefeatScreen defeatScreen;
    private NextFloorScreen nextFloorScreen;

    public bool IsHudOn { get; private set; } = false;

    protected override void Awake()
    {
        base.Awake();

        defeatScreen = defeatScreenCanvas.GetComponent<DefeatScreen>();
        prestartUI = prestartCanvas.GetComponent<PrestartUI>();
        nextFloorScreen = nextFloorCanvas.GetComponent<NextFloorScreen>();
        if (waveHud == null) { waveHud = GetComponent<WaveHud>(); }

        HideDefeatScreen();

    }
    private void Start()
    {
        Reset();

        BaseManager.Instance.OnHealthChanged += UpdateHealthUI;
        WalletManager.Instance.OnCoinsChanged += UpdateCoinsUI;

        prestartUI.OnPressed += CallStartPlaying;
        
        Hide();
    }


    private void UpdateHealthUI(float health, float x)
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
        UpdateInfoFields();
    }

    public void Hide()
    {
        IsHudOn = false;
        this.gameObject.SetActive(false);
    }

    public void HidePrestart()
    {
        prestartUI.Hide();
        prestartCanvas.SetActive(false);
    }

    public void ShowPrestart()
    {
        prestartCanvas.SetActive(true);
    }

    public void ShowPrestartButton()
    {
        prestartUI.Show();
    }

    public void ShowDefeatScreen()
    {
        defeatScreen.ShowStatsRun();
        defeatScreenCanvas.SetActive(true);
    }

    public void HideDefeatScreen()
    {
        defeatScreenCanvas.SetActive(false);
    }

    public void ShowStageCompletedCanvas()
    {
        nextFloorScreen.Show();
    }

    private void OnDestroy()
    {
        if (BaseManager.Instance != null)
            BaseManager.Instance.OnHealthChanged -= UpdateHealthUI;

        if (WalletManager.Instance != null)
            WalletManager.Instance.OnCoinsChanged -= UpdateCoinsUI;

        if (prestartUI != null)
            prestartUI.OnPressed -= CallStartPlaying;
    }

    private void CallStartPlaying()
    {
        HidePrestart();
        GameFlowManager.Instance.StartPlaying();
    }

    private void UpdateInfoFields()
    {
        coinsText.text = WalletManager.Instance.Coins.ToString();
        healthText.text = BaseManager.Instance.MaxHp.ToString();
    }

    public void Reset()
    {
        HideDefeatScreen();
        nextFloorScreen.Hide();
        ShowPrestart();
        UpdateInfoFields();
    }

    public void SetTotalWaveIcons(int totalwave)
    {
        waveHud.Init(totalwave);
    }
    public void SetCurrentWaveIcon(int wave)
    {
        waveHud.SetCurrentWave(wave);
    }



    public void TimerWaveHide()
    {
        if (timerWaveCanvas != null)
            timerWaveCanvas.SetActive(false);
    }
    public void TimerWaveShow()
    {
        if (timerWaveCanvas != null)
            timerWaveCanvas.SetActive(true);
    }
    public void TimerWaveSet(float time)
    {
        if (timerTXT == null) return;

        timerTXT.text =
            System.TimeSpan.FromSeconds(time)
                .ToString(@"mm\:ss");
    }
}