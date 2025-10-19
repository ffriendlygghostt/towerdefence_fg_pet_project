using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteAlways] 
[RequireComponent(typeof(Image))]
public class UIImageFlipper : MonoBehaviour
{
    [Header("Flip Settings")]
    public bool flipX = false;
    public bool flipY = false;

    private RectTransform rectTransform;

    private void OnEnable()
    {
        rectTransform = GetComponent<RectTransform>();
        ApplyFlip();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!Application.isPlaying)
        {
            rectTransform = GetComponent<RectTransform>();
            ApplyFlip();
            EditorApplication.QueuePlayerLoopUpdate();
        }
    }
#endif

    private void ApplyFlip()
    {
        if (rectTransform == null) return;

        rectTransform.localScale = new Vector3(
            flipX ? -1f : 1f,
            flipY ? -1f : 1f,
            1f
        );
    }
}