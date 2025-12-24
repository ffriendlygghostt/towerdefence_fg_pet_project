using System;
using System.Collections;
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

    private Color baseColor;
    private Color hitFlash = new Color(0.7f, 0.7f, 0.7f, 0.8f);
    private float hitFlashDuration = 0.3f;
    private Coroutine hitFlashCoroutine;

    private float deathFadeDelay = 5f;
    private float fadeDuration = 1.5f;
    private Coroutine fadeOutCoroutine;

    public event Action OnDeathAnimationFinished;


    private void OnEnable()
    {
        ReturnBaseColor();
        SpeedGameManager.Instance.OnSpeedGameChanged += SetAnimatorSpeed;
    }
    private void Awake()
    {
        if (animator == null) { animator = GetComponent<Animator>(); }
        if (spriteRenderer == null) { spriteRenderer = GetComponent<SpriteRenderer>(); }
        baseColor = spriteRenderer.color;
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
        FadeOut();
        animator.speed = 1f * SpeedGameManager.Instance.SpeedMultiplier;

        if (hitFlashCoroutine != null)
        {
            StopCoroutine(hitFlashCoroutine);
            hitFlashCoroutine = null;
        }

        spriteRenderer.color = baseColor;

        
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

    

    public void PlayHitFlash()
    {
        if (fadeOutCoroutine != null) return;
        if (hitFlashCoroutine != null)
        {
            StopCoroutine(hitFlashCoroutine);
        }
        hitFlashCoroutine = StartCoroutine(HitFlashRoutine());
    }

    private IEnumerator HitFlashRoutine()
    {
        spriteRenderer.color = hitFlash;
        yield return new WaitForSeconds(hitFlashDuration);
        spriteRenderer.color = baseColor;
        hitFlashCoroutine = null;
    }

    public void FadeOut()
    {
        if (fadeOutCoroutine != null)
        {
            StopCoroutine(fadeOutCoroutine);
        } fadeOutCoroutine = StartCoroutine(DeathFadeRoutine());
    }

    private IEnumerator DeathFadeRoutine()
    {
        yield return new WaitForSeconds(deathFadeDelay);
        float time = 0f;
        Color color = spriteRenderer.color;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, time / fadeDuration);
            spriteRenderer.color = color;
            yield return null;
        }

        color.a = 0f;
        spriteRenderer.color = color;

        fadeOutCoroutine = null;
        OnDeathAnimationFinished?.Invoke();
    }

    public void ReturnBaseColor()
    {
        spriteRenderer.color = baseColor;
    }

    private void OnDisable()
    {
        SpeedGameManager.Instance.OnSpeedGameChanged -= SetAnimatorSpeed;
    }
}
