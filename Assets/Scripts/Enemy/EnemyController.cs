using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private EnemyStats stats;
    [SerializeField] private EnemyMovement movement;
    [SerializeField] private EnemyAnimatorController animationController;

    public EnemyType Type => stats.type;

    public event Action<float> OnSpeedChanged;
    private void Awake()
    {
        stats = GetComponent<EnemyStats>();
        movement = GetComponent<EnemyMovement>();
        animationController = GetComponent<EnemyAnimatorController>();

        if (stats == null || movement == null || animationController == null)
        {
            Debug.LogWarning("Enemy has not: EnemyStats | EnemyMovement | EnemyAnimation");
        }
    }

    public void Die()
    {
        movement.StopMovement();
        animationController.PlayDeath(movement.GetCurrentDirection());
        
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

    public void PlayWalk(EnemyDirection direction)
    {
        animationController.PlayWalk(direction);
    }

    public void ResetState()
    {
        stats.ResetStats();
        movement.ResetMovement();
        animationController.ResetAnimation();
        gameObject.SetActive(true);
    }
}
