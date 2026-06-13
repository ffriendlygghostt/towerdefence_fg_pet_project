using System;
using UnityEngine;

public class WalletManager : Manager<WalletManager>
{
    public int Coins { get; private set; }

    public event Action<int> OnCoinsChanged;

    private int baseCoins = 150;

    private int baseRefillCoins = 4;
    public int refilCoins { get; private set; }


    private float coinTimer;

    private int multiplierCoins = 1;
    private int bonusCoins = 0;

    private int bonusRefilCoins = 0;

    private void Awake()
    {
        base.Awake();
        ResetWallet();
    }
    private void Update()
    {
        coinTimer += Time.deltaTime * SpeedGameManager.Instance.SpeedMultiplier;
        if (coinTimer >= 1f)
        {
            coinTimer -= 1f;
            Add(refilCoins);
        }
    }

    public void ResetWallet()
    {
        Coins = baseCoins + bonusCoins * multiplierCoins;
        OnCoinsChanged?.Invoke(Coins);
        refilCoins = refilCoins + bonusRefilCoins;
    }


    public void Add(int amount)
    {
        Coins += amount;
        OnCoinsChanged?.Invoke(Coins);
    }
    public bool TrySpend(int amount)
    {
        if (Coins < amount) return false;
        Coins -= amount;
        OnCoinsChanged?.Invoke(Coins);
        return true;
    }




    public void AddMultiplierMoney(int multiplier)
    {
        multiplierCoins += multiplier;
    }
    public void RemoveMultiplierMoney(int multiplier)
    {
        multiplierCoins -= multiplier;
    }

    public void AddBonusMoney(int bonus)
    {
        bonusCoins += bonus;
    }
    public void RemoveBonusMoney(int bonus)
    {
        bonusCoins -= bonus;
    }

    public void AddBonusRefilCoins(int bonus)
    {
        bonusRefilCoins += bonus;
    }
    public void RemoveBonusRefilCoins(int bonus)
    {
        bonusRefilCoins -= bonus;
    }
}
