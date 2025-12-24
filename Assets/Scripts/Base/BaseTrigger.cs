using UnityEngine;

public class BaseTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent<EnemyController>(out var enemy))
        {
            return;
        }
        BaseManager.Instance.TakeDamage(enemy.GetDamage());
        enemy.ReachBase();
    }
}
