using UnityEngine;

public enum UnitAnimation
{
    UnitIdle,
    UnitAttack
}

public class UnitAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private UnitAnimation? currentAnimation;

    private void Start()
    {
        UpdateAnimatorSpeed(SpeedGameManager.Instance.SpeedMultiplier);

        PlayIdle();
    }

    private void OnEnable()
    {
        SpeedGameManager.Instance.OnSpeedGameChangedMultiplie += UpdateAnimatorSpeed;
    }

    private void OnDisable()
    {
        SpeedGameManager.Instance.OnSpeedGameChangedMultiplie -= UpdateAnimatorSpeed;
    }

    private void UpdateAnimatorSpeed(float speed)
    {
        animator.speed = speed;
    }
    public void PlayIdle()
    {
        Play(UnitAnimation.UnitIdle);
    }

    public void PlayAttack()
    {
        Play(UnitAnimation.UnitAttack);
    }

    private void Play(UnitAnimation animation)
    {
        if (currentAnimation == animation)
            return;

        currentAnimation = animation;
        animator.Play(animation.ToString());
    }

    public void LookAt(Vector3 targetPos)
    {
        spriteRenderer.flipX = targetPos.x > transform.position.x;
    }

    public void SetMaskInteraction(SpriteMaskInteraction interaction)
    {
        spriteRenderer.maskInteraction = interaction;
    }
}

