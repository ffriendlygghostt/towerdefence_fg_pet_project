using System;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [Header("Main parameters")]
    public string enemyName;
    public float maxHealth = 100f;
    public float baseMaxHealth = 50f;
    public float moveSpeed = 2f;
    public float baseMoveSpeed = 0.5f;
    public int goldReward = 0;
    public int chanceDrop = 0;
    public int pointExp = 0;
    public float damage = 10f;
    public float speedAnimator = 1f;
    public EnemyType type;
    public EnemyRoleTier role;
    public int cost = 0;

    [HideInInspector] public float currentHealth;
    [HideInInspector] public int difficultyLevel = 1;

    public event Action OnDeathAction;
    

    private void Awake()
    {
        ResetStats();
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            OnDeathAction?.Invoke();
        }
    }

    public float GetDamage() => damage;
    public bool IsDead() => currentHealth <= 0;

    public void ResetStats()
    {
        maxHealth = baseMaxHealth * DifficultyManager.Instance.HpMultiplier;
        currentHealth = maxHealth;
        moveSpeed = baseMoveSpeed * DifficultyManager.Instance.SpeedMultiplier;
        pointExp = Convert.ToInt32(maxHealth * moveSpeed + damage);
    }
}


