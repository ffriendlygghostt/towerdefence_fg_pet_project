using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [Header("Main parameters")]
    public string enemyName;
    public float maxHealth = 100f;
    public float moveSpeed = 2f;
    public float damage = 10f;
    public int goldReward = 0;

    [HideInInspector] public float currentHealth;
    [HideInInspector] public int difficultyLevel = 1;

    private EnemyController controller;

    private void Awake()
    {
        currentHealth = maxHealth;
        controller = GetComponent<EnemyController>();
        if (controller == null)
        {
            Debug.LogWarning($"Для врага {gameObject.name} испольузется заглушка EnemyController!");
            controller = new EnemyController();
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

    // TEMP: временный локальный класс, убрать когда будет настоящий EnemyController
    private class EnemyController : MonoBehaviour 
    {
        public virtual void Die()
        {
            Debug.Log("EnemyController.Die() вызван, но контроллера нет. Заглушка.");
        }
    }
}


