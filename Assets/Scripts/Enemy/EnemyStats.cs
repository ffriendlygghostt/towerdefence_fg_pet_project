using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [Header("Main parameters")]
    public string enemyName;
    public float maxHealth = 100f;
    public float moveSpeed = 2f;
    public float damage = 10f;
    public int goldReward = 0;
    public float speedAnimator = 1f;
    public EnemyType type;

    [HideInInspector] public float currentHealth;
    [HideInInspector] public int difficultyLevel = 1;

    private EnemyController controller;

    private void Awake()
    {
        controller = GetComponent<EnemyController>();
        if (controller == null)
        {
            Debug.LogWarning("Enemy has not EnemyController!!!");
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth < 0) currentHealth = 0;
        if (currentHealth == 0)
                controller.Die();
    }

    public float GetDamage()
    {
        return damage;
    }

    public bool IsDead()
    {
        return currentHealth <= 0;
    }

    public void ResetStats()
    {
        maxHealth = maxHealth * DifficultyManager.Instance.HpMultiplier;
        currentHealth = maxHealth;
        moveSpeed = moveSpeed * DifficultyManager.Instance.SpeedMultiplier;
    }
}


