using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TowerRange : MonoBehaviour
{
    private SpriteRenderer spriteRend;
    private Color defaultColor;
    private Color zeroColor = new Color(0, 0, 0, 0);

    private readonly List<IDamageable> targets = new();

    private void Awake()
    {
        spriteRend = GetComponent<SpriteRenderer>();
        defaultColor = spriteRend.color;
    }

    private void Start()
    {
        Debug.Assert(spriteRend != null);
        Hide();
    }

    public void Show()
    {
        spriteRend.color = defaultColor;
    }

    public void Hide()
    {
        spriteRend.color = zeroColor;
    }

    public void SetRadius(float radius)
    {
        transform.localScale = Vector3.one * radius;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<IDamageable>(out var target))
        {
            if (!targets.Contains(target))
                targets.Add(target);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent<IDamageable>(out var target))
        {
            targets.Remove(target);
        }
    }

    public IDamageable GetTarget()
    {
        for (int i = targets.Count - 1; i >= 0; i--)
        {
            IDamageable target = targets[i];

            if (target == null || target.IsDead)
            {
                targets.RemoveAt(i);
                continue;
            }
            return target;
        }
        return null;
    }
}
