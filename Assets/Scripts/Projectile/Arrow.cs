using UnityEngine;

public class Arrow : Projectile
{

    protected override void Hit()
    {
        base.Hit();
        target.TakeDamage(damage);
        ReturnToPool();
    }
}
