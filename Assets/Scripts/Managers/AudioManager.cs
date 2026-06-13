using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.AddressableAssets.Build;
using UnityEngine;


public class AudioManager : Manager<AudioManager>
{
    [Header("AudioSource Prefab")]
    public GameObject audioSourcePrefab;

    [Header("Pool Settings")]
    public int initialPoolSize = 10;
    public int expandPoolSize = 10;

    private AudioSourcePool sfxPool;
    private readonly HashSet<AudioSource> loopSources = 
        new HashSet<AudioSource>();

    [Header("Music Sources for crossfade")]
    private AudioSource musicSourceA;
    private AudioSource musicSourceB;
    private bool usingSourceA = true;

    private float musicVolume = 1f;
    private float sfxVolume = 1f;
    private Coroutine musicCoroutine;
    private float currentPitch = 1f;

    [Header("UISource")]
    private AudioSource uiSource;

    [Header("AudioLibrary")]
    [SerializeField] private AudioLibrary library;



    protected override void Awake()
    {
        base.Awake();

        sfxPool = new AudioSourcePool(audioSourcePrefab, initialPoolSize, expandPoolSize, transform);

        musicSourceA = new GameObject("MusicSourceA").AddComponent<AudioSource>();
        musicSourceB = new GameObject("MusicSourceB").AddComponent<AudioSource>();
        musicSourceA.transform.SetParent(transform);
        musicSourceB.transform.SetParent(transform);

        musicSourceA.loop = true;
        musicSourceB.loop = true;

        uiSource = new GameObject("UISource").AddComponent<AudioSource>();
        uiSource.transform.SetParent(transform);
        uiSource.loop = false;
        uiSource.ignoreListenerPause = true;

        library.Init();
    }

    private void Start()
    {
        LoadSettings();

        SettingsService.Instance.OnMusicVolumeChanged += SetMusicVolume;
        SettingsService.Instance.OnSFXVolumeChanged += SetSFXVolume;

        SpeedGameManager.Instance.OnSpeedGameChanged += HandleSpeedChanged;
    }

    private void LoadSettings()
    {
        musicVolume = SettingsService.Instance.MusicVolume;
        sfxVolume = SettingsService.Instance.SFXVolume;

        musicSourceA.volume = musicVolume;
        musicSourceB.volume = musicVolume;
    }

    public void PlaySFX(SoundSO sound)
    {
        if (sound.type != SoundType.SFX) 
            return;

        AudioSource source = sfxPool.Get();
        source.clip = sound.clip;
        source.volume = sound.volume * sfxVolume;
        source.loop = false;
        source.pitch = currentPitch;
        source.Play();

        StartCoroutine(ReturnToPoolAfterPlay(source));
    }

    private IEnumerator ReturnToPoolAfterPlay(AudioSource source)
    {
        yield return new WaitWhile(() => source.isPlaying);
        sfxPool.Return(source);
    }


    public void PlayUISFX(SoundSO sound)
    {
        if (sound.type != SoundType.UI) return;

        uiSource.pitch = 1f;
        uiSource.PlayOneShot(sound.clip, sound.volume * sfxVolume);
    }

    public AudioSource PlayLoopSFX(SoundSO sound)
    {
        if (sound.type != SoundType.LoopSFX)
            return null;

        AudioSource source = sfxPool.Get();

        source.clip = sound.clip;
        source.volume = sound.volume * sfxVolume;
        source.loop = true;
        source.pitch = currentPitch;

        source.Play();

        loopSources.Add(source);

        return source;
    }

    public void StopLoopSFX(AudioSource source)
    {
        if (source == null)
            return; 

        loopSources.Remove(source);
        sfxPool.Return(source);
    }

    private void PauseLoopSFX()
    {
        foreach(var source in loopSources)
        {
            source.Pause();
        }
    }

    private void ResumeLoopSFX()
    {
        foreach(var source in loopSources)
        {
            source.UnPause();
        }
    }

    public void PlayMusic(SoundSO music, float fadeDuration = 2f)
    {
        if (music.type != SoundType.Music) return;

        AudioSource active = usingSourceA ? musicSourceA : musicSourceB;
        AudioSource next = usingSourceA ? musicSourceB : musicSourceA;

        next.clip = music.clip;
        next.volume = 0;
        next.loop = music.loop;
        next.Play();

        musicCoroutine = StartCoroutine(CrossFadeMusic(active, next, fadeDuration));
        usingSourceA = !usingSourceA;
    }

    private IEnumerator CrossFadeMusic(AudioSource from, AudioSource to, float duration)
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            from.volume = Mathf.Lerp(musicVolume, 0, t / duration);
            to.volume = Mathf.Lerp(0, musicVolume, t / duration);
            yield return null;
        }
        from.Stop();
        from.clip = null;
        to.volume = musicVolume;

        musicCoroutine = null;
    }


    public void SetMusicVolume(float value)
    {
        musicVolume = value;
        musicSourceA.volume = usingSourceA ? musicVolume : 0;
        musicSourceB.volume = usingSourceA ? 0 : musicVolume;
    }

    public void SetSFXVolume(float value) => sfxVolume = value;

    public void PlaySFXType(SoundEnum sound)
    {
        var clip = library.GetClip(sound);
        if (clip != null)
        {
            PlaySFX(clip);
        }
    }

    public void PlayUISFXType(SoundEnum sound)
    {
        var clip = library.GetClip(sound);
        if (clip != null)
        {
            PlayUISFX(clip);
        }
    }

    public void PlayMusicType(MusicType type)
    {
        var music = library.GetMusic(type);
        if (music != null)
        {
            PlayMusic(music);
        }
    }

    public void StopMusic(float fadeDuration = 1.5f)
    {
        if (musicCoroutine != null)
        {
            StopCoroutine(musicCoroutine);
            musicCoroutine = null;
        }

        if (musicSourceA.isPlaying)
            StartCoroutine(FadeOutAndStop(musicSourceA, fadeDuration));

        if (musicSourceB.isPlaying)
            StartCoroutine(FadeOutAndStop(musicSourceB, fadeDuration));
    }

    private IEnumerator FadeOutAndStop(AudioSource source, float fadeDuration)
    {
        float startVolume = source.volume;
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
            yield return null;
        }

        source.Stop();
        source.clip = null;
        source.volume = musicVolume;

    }

    private void HandleSpeedChanged(GameSpeed speed)
    {
        if (speed == GameSpeed.Pause)
        {
            PauseAllGameAudio();
            return;
        }

        ResumeMusic();
        ResumeLoopSFX();
        SetPitchBySpeed(SpeedGameManager.Instance.SpeedMultiplier);
    }

    private void PauseAllGameAudio()
    {
        PauseMusic();
        StopAllSFX();
        PauseLoopSFX();
    }

    public void PauseMusic()
    {
        if (musicSourceA.isPlaying) musicSourceA.Pause();
        if (musicSourceB.isPlaying) musicSourceB.Pause();
    }

    public void ResumeMusic()
    {
        musicSourceA.UnPause();
        musicSourceB.UnPause();
    }

    private void StopAllSFX()
    {
        var active = new List<AudioSource>(sfxPool.ActiveSources);

        foreach (var source in active)
        {
            if (loopSources.Contains(source))
                continue;

            source.Stop(); 
            sfxPool.Return(source);
        }
    }

    private void SetPitchBySpeed(float speed)
    {
        if (speed <= 1f)
            currentPitch = 1f;
        else
            currentPitch = Mathf.Lerp(1f, 1.5f, (speed - 1f) / 3);  //~1.25(2x)  //~1.5(4x)

        foreach (var source in sfxPool.ActiveSources)
        {
            source.pitch = currentPitch;
        }
    }
}

