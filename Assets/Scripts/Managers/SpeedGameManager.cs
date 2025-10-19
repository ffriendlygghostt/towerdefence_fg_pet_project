using System;
using UnityEngine;

public class SpeedGameManager : Manager<SpeedGameManager>
{
    public float SpeedMultiplier { get; private set; } = 1f;

    protected override void Awake()
    {
        base.Awake();
    }

    public void SetSpeed(float multiplier)
    {
        SpeedMultiplier = Mathf.Max(0f, multiplier);
        OnSpeedGameChanged?.Invoke(SpeedMultiplier);
    }

    public void Pause() => SetSpeed(0f);
    public void Resume() => SetSpeed(1f);
    public void Speed2x() => SetSpeed(2f);
    public void Speed4x() => SetSpeed(4f);

    public event Action<float> OnSpeedGameChanged;
}
