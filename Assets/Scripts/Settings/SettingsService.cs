using System;
using UnityEngine;

public class SettingsService : Manager<SettingsService>
{
    public float MusicVolume { get; private set; } = 1f;
    public float SFXVolume { get; private set; } = 1f;
    public bool IsFullScreen { get; private set; } = true;
    public int ResolutionIndex { get; private set; } = 0;

    public event Action<float> OnMusicVolumeChanged;
    public event Action<float> OnSFXVolumeChanged;
    public event Action<bool> OnIsFullScreenChanged;
    public event Action<int> OnResolutionIndexChanged;

    private void Start()
    {
        InitializeFromSave();
    }
    private void InitializeFromSave()
    {
        var data = SaveManager.Instance.Data.settings;

        MusicVolume = data.musicVolume;
        SFXVolume = data.sfxVolume;
        IsFullScreen = data.isFullScreen;
        ResolutionIndex = data.resolutionIndex;
    }

    public void SetMusicVolume(float value)
    {
        if (Mathf.Abs(MusicVolume - value) < 0.001f) return;

        MusicVolume = value;
        OnMusicVolumeChanged?.Invoke(value);
    }

    public void SetSFXVolume(float value)
    {
        if (Mathf.Abs(SFXVolume - value) < 0.001f) return;

        SFXVolume = value;
        OnSFXVolumeChanged?.Invoke(value);
    }

    public void SetIsFullScreen(bool value)
    {
        if (IsFullScreen == value) return;
        IsFullScreen = value;
        OnIsFullScreenChanged?.Invoke(value);
    }

    public void SetResolutionChanged(int value)
    {
        if (ResolutionIndex == value) return;
        ResolutionIndex = value;
        OnResolutionIndexChanged?.Invoke(value);
    }

    public void SetDefaultSettings()
    {
        SetSFXVolume(1f);
        SetMusicVolume(1f);
        SetIsFullScreen(true);
        SetResolutionChanged(0);
    }

    public void SaveSettings()
    {
        var data = SaveManager.Instance.Data.settings;

        data.musicVolume = MusicVolume;
        data.sfxVolume = SFXVolume;
        data.isFullScreen = IsFullScreen;
        data.resolutionIndex = ResolutionIndex;

        SaveManager.Instance.Save();
    }
}