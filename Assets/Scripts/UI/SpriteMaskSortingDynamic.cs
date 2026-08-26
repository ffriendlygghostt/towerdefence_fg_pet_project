using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(SpriteMask))]
public class SpriteMaskSortingDynamic : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Смещение порядка слоя")]
    public int offset = 0;

    [Tooltip("Множитель для точности расчёта")]
    public float factor = 100f;

    private SpriteMask spriteMask;

    private void Awake()
    {
        spriteMask = GetComponent<SpriteMask>();
        UpdateSorting();
    }

    private void LateUpdate()
    {
        UpdateSorting();
    }

    private void UpdateSorting()
    {
        if (spriteMask == null) return;

        int order = Mathf.RoundToInt(-transform.position.y * factor) + offset;

        spriteMask.frontSortingOrder = order;
    }
}