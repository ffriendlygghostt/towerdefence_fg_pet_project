using UnityEngine;

public class Arrow : Projectile
{
    protected override void Hit()
    {
        target.TakeDamage(damage);
        ReturnToPool();
    }
}
