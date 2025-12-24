using System;
using UnityEngine;

public class WalletManager : Manager<WalletManager>
{
    public int Coins { get; private set; }

    public event Action<int> OnCoinsChanged;

    public int startCoins { get; private set; } = 150;
    public int refillCoins { get; private set; } = 4;

    private float coinTimer;

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

    public void ResetWallet()
    {
        Coins = startCoins;
        OnCoinsChanged?.Invoke(Coins);
    }

    private void Update()
    {
        coinTimer += Time.deltaTime * SpeedGameManager.Instance.SpeedMultiplier;
        if (coinTimer >= 1f)
        {
            coinTimer -= 1f;
            Add(refillCoins);
        }
    }
}
