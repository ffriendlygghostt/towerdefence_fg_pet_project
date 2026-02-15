using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuController : Manager<SettingsMenuController>
{
    [Header("Audio")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("Resolution")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    private Resolution[] availableResolutions;
    private int currentResolutionIndex = 0;

    [Header("CheckBox")]
    [SerializeField] private Image checkboxImage;
    [SerializeField] private Sprite checkedSprite;
    [SerializeField] private Sprite uncheckedSprite;
    private bool isOnFullScreen = true;

    [Header("Github")]
    [SerializeField]
    private string githubUrl =
    "https://github.com/ffriendlygghostt";

    protected override void Awake()
    {
        base.Awake();

        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        InitResolutions();
        gameObject.SetActive(false);
    }

    private void Start() => ApplySavedSettings();

    public void Hide()
    {
        gameObject.SetActive(false);
        SettingsService.Instance.SaveSettings();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        SyncUI();
    }

    public void OnMusicVolumeChanged(float value) =>
        SettingsService.Instance.SetMusicVolume(value);

    public void OnSFXVolumeChanged(float value) => 
        SettingsService.Instance.SetSFXVolume(value);


    private void InitResolutions()
    {
        var allResolutions = Screen.resolutions;

        var filtered = allResolutions
            .GroupBy(r => new { r.width, r.height })
            .Select(g => g.OrderByDescending(r => r.refreshRateRatio.value).First())
            .ToList();

        filtered = filtered
            .OrderByDescending(r => r.width * r.height)
            .ToList();

        availableResolutions = filtered.ToArray();

        var options = availableResolutions
            .Select(r => $"{r.width}x{r.height}")
            .ToList();

        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(options);
    }


    public void OnResolutionChanged(int index)
    {
        Resolution res = availableResolutions[index];
        Screen.SetResolution(
            res.width,
            res.height,
            Screen.fullScreen
            );
        currentResolutionIndex = index;
    }

    private void SyncUI()
    {
        isOnFullScreen = Screen.fullScreen;
        checkboxImage.sprite = isOnFullScreen ? checkedSprite : uncheckedSprite;
    }

    public void DefaultSettings()
    {
        musicSlider.value = 1f;
        sfxSlider.value = 1f;

        checkboxImage.sprite = checkedSprite;
        Screen.SetResolution(
            availableResolutions[0].width,
            availableResolutions[0].height,
            Screen.fullScreen = true
            );

        resolutionDropdown.value = 0;
        currentResolutionIndex = 0;
        resolutionDropdown.RefreshShownValue();

        SettingsService.Instance.SetDefaultSettings();
    }

    public void FullScreenToggle()
    {
        isOnFullScreen = !isOnFullScreen;
        checkboxImage.sprite = isOnFullScreen ? checkedSprite : uncheckedSprite;
        Screen.fullScreen = isOnFullScreen;
    }


    public void GitHubButton() => Application.OpenURL(githubUrl);

    public void ApplySavedSettings()
    {
        var service = SettingsService.Instance;

        musicSlider.value = service.MusicVolume;
        sfxSlider.value = service.SFXVolume;

        isOnFullScreen = service.IsFullScreen; 
        currentResolutionIndex = service.ResolutionIndex;


        checkboxImage.sprite = isOnFullScreen ? checkedSprite : uncheckedSprite;
        Screen.fullScreen = isOnFullScreen;

        currentResolutionIndex = Mathf.Clamp(
            service.ResolutionIndex,
            0,
            availableResolutions.Length - 1
         );

        Resolution res = availableResolutions[currentResolutionIndex];
        Screen.SetResolution(res.width, res.height, isOnFullScreen);

        resolutionDropdown.SetValueWithoutNotify(currentResolutionIndex);
        resolutionDropdown.RefreshShownValue();
    }
}
