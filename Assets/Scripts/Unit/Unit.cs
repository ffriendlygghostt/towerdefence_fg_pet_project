using UnityEngine;

public class Unit : MonoBehaviour
{
    private UnitLevelData currentLevelData;

    private Tower tower;
    private TowerRange range;

    private UnitAttack attack;
    private UnitAnimationController animationController;

    private IDamageable currentTarget;


    private void Awake()
    {
        if (attack == null)
            attack = GetComponent<UnitAttack>();
        if (animationController == null)
            animationController = GetComponent<UnitAnimationController>();

        Debug.Assert(attack != null);
        Debug.Assert(animationController != null);
    }

    public void Initialize(
        UnitLevelData data,
        Tower owner,
        TowerRange rangeOwner)
    {
        currentLevelData = data;
        tower = owner;
        range = rangeOwner;

        attack = GetComponent<UnitAttack>();
        animationController = GetComponent<UnitAnimationController>();

        attack.Initialize(currentLevelData);
    }

    private void Update()
    {
        currentTarget = range.GetTarget();

        if (currentTarget == null || currentTarget.IsDead)
        {
            animationController.PlayIdle();
            return;
        }

        animationController.LookAt(currentTarget.Transform.position);

        if (attack.TryAttack(currentTarget))
        {
            animationController.PlayAttack();
        }
    }

    public void SetMaskInteraction(SpriteMaskInteraction interaction)
    {
        animationController.SetMaskInteraction(interaction);
    }
}
