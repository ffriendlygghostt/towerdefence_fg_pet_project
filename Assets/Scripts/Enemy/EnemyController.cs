using System;
using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour, IPoolIdentity
{
    [SerializeField] private EnemyStats stats;
    [SerializeField] private EnemyMovement movement;
    [SerializeField] private EnemyAnimatorController animationController;
    [SerializeField] private EnemyHealthBar enemyHealthBar;

    public EnemyType Type => stats.type;

    public event Action<float> OnSpeedChanged;


    private void Awake()
    {
        stats = GetComponent<EnemyStats>();
        movement = GetComponent<EnemyMovement>();
        animationController = GetComponent<EnemyAnimatorController>();
        enemyHealthBar = GetComponent<EnemyHealthBar>();

        if (stats == null || movement == null || animationController == null
            || enemyHealthBar == null)
        {
            Debug.LogWarning("Enemy has not: EnemyStats | EnemyMovement | EnemyAnimation | EnemyHealthBar");
        }
    }

    private void OnEnable()
    {
        stats.ResetStats();
        movement.ResetMovement();
        animationController.ResetAnimation();
        enemyHealthBar.Init(stats.maxHealth);
        enemyHealthBar.Show();

        stats.OnDeathAction += Die;
    }

    private void OnDisable()
    {
        stats.OnDeathAction -= Die;
    }

    public void Die()
    {
        movement.StopMovement();
        animationController.PlayDeath(movement.GetCurrentDirection());
        enemyHealthBar.Hide();
    }

    public void ReachBase()
    {
        movement.StopMovement();
        enemyHealthBar.Hide();
        //PoolManager.Instance.Return(this);
    }
    public float GetMoveSpeed() => stats.moveSpeed;

    public void ChangeSpeed(float newSpeed)
    {
        OnSpeedChanged?.Invoke(newSpeed);
        if (stats.moveSpeed != newSpeed)
        {
            stats.moveSpeed = newSpeed;
        }
    }

    public void TakeDamage(float damage)
    {
        stats.TakeDamage(damage);
        enemyHealthBar.UpdateHealth(damage);
        animationController.PlayHitFlash();
    }

    public void PlayWalk(EnemyDirection direction)
    {
        animationController.PlayWalk(direction);
    }

    public float GetDamage()
    {
        return stats.GetDamage();
    }

    public string GetPoolId() => Type.ToString();
}
