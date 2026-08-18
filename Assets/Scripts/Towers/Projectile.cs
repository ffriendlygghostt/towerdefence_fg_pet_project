using UnityEngine;

public abstract class Projectile : MonoBehaviour, IPoolIdentity, IPoolable
{
    public ProjectileType type;

    protected float damage;
    protected float speed;

    protected IDamageable target;

    public SoundSO soundDamage;

    public virtual void Initialize(
        float damage, 
        float speed, 
        IDamageable target,
        Transform spawnPosition
    )
    {
        this.damage = damage;
        this.speed = speed;
        this.target = target;

        transform.SetPositionAndRotation(
            spawnPosition.position,
            spawnPosition.rotation);
    }

    public string GetPoolId()
    {
        return type.ToString();
    }

    protected virtual void Update()
    {
        if (target == null || target.IsDead)
        {
            ReturnToPool();
            return;
        }

        Vector3 direction = 
            target.Transform.position - transform.position;

        if (direction != Vector3.zero)
        {
            transform.right = direction;
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            target.Transform.position,
            speed *
            Time.deltaTime *
            SpeedGameManager.Instance.SpeedMultiplier
        );

        if (Vector3.Distance(
            transform.position, 
            target.Transform.position) < 0.05f)
        {
            Hit();
        }
    }

    protected virtual void Hit()
    {
        AudioManager.Instance.PlaySFX(soundDamage);
    }

    protected virtual void ReturnToPool()
    {
        PoolManager.Instance.Return(gameObject);
    }

    public virtual void OnSpawn() { }
    public virtual void OnDespawn() 
    {
        damage = 0;
        speed = 0;
        target = null;
    }
}
