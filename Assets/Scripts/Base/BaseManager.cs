using System;
using UnityEngine;

public class BaseManager : Manager<BaseManager>
{
    public float MaxHp { get; private set; } = 220f;
    public float CurrentHp { get; private set; }

    public event Action<float, float> OnHealthChanged;
    public event Action OnBaseDestroyed;

    protected override void Awake()
    {
        base.Awake();

    }

    public void ResetBase()
    {
        CurrentHp = MaxHp;
        OnHealthChanged?.Invoke(CurrentHp, MaxHp);
    }

    public void TakeDamage(float damage)
    {
        CurrentHp -= damage;
        CurrentHp = Mathf.Max(0f, CurrentHp);

        OnHealthChanged?.Invoke(CurrentHp, MaxHp);

        if (CurrentHp <= 0f)
        {
            OnBaseDestroyed?.Invoke();
            GameFlowManager.Instance.Defeat();
        }
    }
}
