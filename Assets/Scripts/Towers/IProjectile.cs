
using UnityEngine;

public interface IProjectile
{
    void Initialize(int damage, float speed, IDamageable target);
}


public enum ProjectileType
{
    Arrow,
    FireBall,
    IceBall
}