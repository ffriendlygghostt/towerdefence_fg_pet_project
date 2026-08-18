using UnityEngine;

[CreateAssetMenu(menuName = "Audio/Sound")]
public class SoundSO : ScriptableObject
{
    public string soundName;
    public AudioClip clip;
    [Range(0, 1)] public float volume = 1f;
    public bool loop = false;
    public SoundType type = SoundType.SFX;

    public bool limited = false;
    public int maxSimultaneous = 5;
}

public enum SoundType
{
    Music,
    SFX,
    LoopSFX,
    UI
}

public enum SoundDefaultEnum
{
    ButtonClick,
    ButtonHover,
    ButtonError,

    UnitBuy,
    UnitUpgrade,
    UnitMaxUpgrade,

    TowerBuy,
    TowerUpgrade,
    TowerUpgradeMax,
    TowerCrash,

    EnemyAttack,
    EnemyDie1,
    EnemyDie2,
    EnemyDie3,
    EnemyDie4,

    Taymer,

    WaveStart,
    WinFloor,
    GameOver
}

public enum MusicType
{
    Menu,
    FightRandom,
    Boss
}