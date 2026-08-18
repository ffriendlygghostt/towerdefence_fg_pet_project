using UnityEngine;

public class Arrow : Projectile
{

    protected override void Hit()
    {
        base.Hit();  ///AudioManager.Instance.PlaySFX(soundDamage);
        target.TakeDamage(damage);
        ReturnToPool();
    }
}
