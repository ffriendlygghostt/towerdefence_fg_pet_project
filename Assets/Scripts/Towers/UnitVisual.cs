using UnityEngine;

public class UnitVisual : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        if (spriteRenderer == null)
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
    }
    public void SetMaskInteraction(SpriteMaskInteraction interaction)
    {
        spriteRenderer.maskInteraction = interaction;
    }
}
