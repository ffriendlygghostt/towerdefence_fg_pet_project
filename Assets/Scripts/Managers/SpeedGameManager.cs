using System;
using UnityEngine;

public enum GameSpeed
{
    Pause, X1, X2, X4
}

public class SpeedGameManager : Manager<SpeedGameManager>
{
    public float SpeedMultiplier { get; private set; } = 0f;
    public GameSpeed CurrentSpeed { get; private set; } = GameSpeed.Pause;

    protected override void Awake()
    {
        base.Awake();
    }

    private void SetSpeed(GameSpeed speed, float multiplier)
    {
        CurrentSpeed = speed;
        SpeedMultiplier = Mathf.Max(0f, multiplier);

        OnSpeedGameChanged?.Invoke(CurrentSpeed);
    }

    public void Pause() => SetSpeed(GameSpeed.Pause, 0f);
    public void Resume() => SetSpeed(GameSpeed.X1, 1f);
    public void Speed2x() => SetSpeed(GameSpeed.X2, 2f);
    public void Speed4x() => SetSpeed(GameSpeed.X4, 4f); 

    public event Action<GameSpeed> OnSpeedGameChanged;
}
