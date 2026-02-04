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
    public GameSpeed PastSpeed = GameSpeed.Pause;

    protected override void Awake()
    {
        base.Awake();
    }

    private void SetSpeed(GameSpeed speed, float multiplier)
    {
        PastSpeed = CurrentSpeed;
        CurrentSpeed = speed;
        SpeedMultiplier = Mathf.Max(0f, multiplier);

        OnSpeedGameChanged?.Invoke(CurrentSpeed);
    }

    public void Pause() => SetSpeed(GameSpeed.Pause, 0f);
    public void Resume() => SetSpeed(GameSpeed.X1, 1f);
    public void Speed2x() => SetSpeed(GameSpeed.X2, 2f);
    public void Speed4x() => SetSpeed(GameSpeed.X4, 4f); 
    public void SetSpeedState(GameSpeed speed)
    {
        switch (speed)
        {
            case GameSpeed.Pause:
                Pause();
                break;
            case GameSpeed.X1:
                Resume();
                break;
            case GameSpeed.X2:
                Speed2x();
                break;
            case GameSpeed.X4:
                Speed4x();
                break;
            default:
                Pause();
                break;
        };
    }

    public void SetPastSpeed()
    {
        SetSpeedState(PastSpeed);
    }

    public event Action<GameSpeed> OnSpeedGameChanged;
}
