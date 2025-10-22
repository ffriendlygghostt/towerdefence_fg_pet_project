using System;
using UnityEngine;

public enum EnemyAnimation
{
    Walk_Side, Walk_Up, Walk_Down, Death_Side, Death_Up, Death_Down
}

public class EnemyAnimatorController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private EnemyAnimation currentAnimation;
    private bool rightFlip = false;

    private void OnEnable()
    {
        SpeedGameManager.Instance.OnSpeedGameChanged += SetAnimatorSpeed;
    }

    private void Awake()
    {
        if (animator == null) { animator = GetComponent<Animator>(); }
        if (spriteRenderer == null) { spriteRenderer = GetComponent<SpriteRenderer>(); }
    }

    private void PlayAnimation()
    {
        if (animator == null) return;
        animator.Play(currentAnimation.ToString());
        HandleFlip();
    }

    private void HandleFlip()
    {
        if (spriteRenderer == null) return;
        if (currentAnimation != EnemyAnimation.Death_Side &&
            currentAnimation != EnemyAnimation.Walk_Side)
        {
            spriteRenderer.flipX = false;
            return;
        }

        spriteRenderer.flipX = rightFlip;
    }

    public void PlayWalk(EnemyDirection direction, float speedMultiplier = 1f)
    {
        rightFlip = false;

        switch (direction)
        {
            case EnemyDirection.Up: currentAnimation = EnemyAnimation.Walk_Up; break;
            case EnemyDirection.Down: currentAnimation = EnemyAnimation.Walk_Down; break;
            case EnemyDirection.Left: currentAnimation = EnemyAnimation.Walk_Side; break;
            case EnemyDirection.Right: currentAnimation = EnemyAnimation.Walk_Side;
                rightFlip = true; break;
            default: currentAnimation = EnemyAnimation.Walk_Side; break;
        }

        PlayAnimation();
        animator.speed = speedMultiplier * SpeedGameManager.Instance.SpeedMultiplier;
    }

    public void PlayDeath(EnemyDirection direction)
    {
        rightFlip = false;
        switch (direction)
        {
            case EnemyDirection.Up: currentAnimation = EnemyAnimation.Death_Up; break;
            case EnemyDirection.Down: currentAnimation = EnemyAnimation.Death_Down; break;
            case EnemyDirection.Left: currentAnimation = EnemyAnimation.Death_Side; break;
            case EnemyDirection.Right: currentAnimation = EnemyAnimation.Death_Side;
                rightFlip = true; break;
            default: currentAnimation = EnemyAnimation.Death_Side; break;
        }

        PlayAnimation();
        animator.speed = 1f * SpeedGameManager.Instance.SpeedMultiplier;
    }

    public void ResetAnimation()
    {
        rightFlip = false;
        currentAnimation = EnemyAnimation.Walk_Side;
        animator.speed = 1f * SpeedGameManager.Instance.SpeedMultiplier;
    }

    private void SetAnimatorSpeed(float newSpeed)
    {
        animator.speed = newSpeed;
    }
    private void OnDisable()
    {
        SpeedGameManager.Instance.OnSpeedGameChanged -= SetAnimatorSpeed;
    }
}
