using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(SpriteRenderer))]
public class SortingLayerDynamic : MonoBehaviour
{
    [Header("Settings")] 
    [Tooltip("Смещение, добавляемое к порядку слоя")]
    public int offset = 0;
    [Tooltip("Множитель для точности расчёта")]
    public float factor = 100f;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateSorting();
    }

    private void LateUpdate()
    {
        UpdateSorting();
    }

    private void UpdateSorting()
    {
        if (spriteRenderer == null) return;
        spriteRenderer.sortingOrder = Mathf.RoundToInt(-transform.position.y * factor) + offset;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
