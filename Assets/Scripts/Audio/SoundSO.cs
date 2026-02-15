using UnityEngine;

[CreateAssetMenu(menuName = "Audio/Sound")]
public class SoundSO : ScriptableObject
{
    public string soundName;
    public AudioClip clip;
    [Range(0, 1)] public float volume = 1f;
    public bool loop = false;
    public SoundType type = SoundType.SFX;
}

public enum SoundType
{
    Music,
    SFX,
    UI
}

public enum SoundEnum
{
    ClickButtonDefault,
    HoverButtonDefault
}

public enum MusicType
{
    Menu,
    FightRandom,
    Boss
}