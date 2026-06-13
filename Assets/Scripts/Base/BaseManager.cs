using System;
using UnityEngine;

public class BaseManager : Manager<BaseManager>
{
    private float BaseMaxHp = 220f;
    public float MaxHp { get; private set; }
    public float CurrentHp { get; private set; }

    private float multiplierHP = 1;
    private float bonusHP = 0;

    public event Action<float, float> OnHealthChanged;
    public event Action<float> OnHealthChange;
    public event Action OnBaseDestroyed;

    protected override void Awake()
    {
        base.Awake();
        ResetBase();
    }

    public void ResetBase()
    {
        MaxHp = BaseMaxHp + bonusHP * multiplierHP;
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
        }
    }

    public void AddMultiplierHP(float multiplier)
    {
        multiplierHP += multiplier;
    }
    public void RemoveMultiplierHP(float multiplier)
    {
        multiplierHP -= multiplier;
    }

    public void AddBonusHP(float bonus)
    {
        bonusHP += bonus;
    }
    public void RemoveBonusHP(float bonus)
    {
        bonusHP -= bonus;
    }
}
