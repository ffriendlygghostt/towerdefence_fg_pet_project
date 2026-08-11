using UnityEngine;

public class UnitAttack : MonoBehaviour
{
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private float projectileSpeed = 4f;
    [SerializeField] private ProjectileType typeProjectile;
         
    private UnitLevelData levelData;

    private IDamageable target;
    private float attackTimer;

    public void Initialize(UnitLevelData levelData)
    {
        this.levelData = levelData;
        attackTimer = levelData.attackCooldown;
    }

    public bool TryAttack(IDamageable target)
    {
        if (target == null || target.IsDead)
            return false;

        if (attackTimer > 0f)
            return false;

        this.target = target;
        return true;
    }

    private void Update()
    {
        if (attackTimer > 0f)
        {
            attackTimer -= Time.deltaTime * 
                SpeedGameManager.Instance.SpeedMultiplier;
        }
    }

    private void SpawnProjectile()
    {
        if (target == null || target.IsDead)
            return;

        var projectileGO = PoolManager.Instance.Get(typeProjectile.ToString());

        Projectile projectile = projectileGO.GetComponent<Projectile>();

        projectile.Initialize(
            levelData.damage,
            projectileSpeed,
            target,
            projectileSpawnPoint
            );

        attackTimer = levelData.attackCooldown;
        target = null;
    }
}
