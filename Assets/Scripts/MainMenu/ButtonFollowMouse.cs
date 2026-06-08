using UnityEngine;
using UnityEngine.EventSystems;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class ButtonFollowMouse : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Settings")]
    [SerializeField] public float moveAmount = 10f;
    [SerializeField] public float smoothTime = 0.1f;

    private RectTransform rectTransform;
    private Vector3 originalPosition;
    private Vector3 velocity;

    private bool isHovered = false;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = transform.localPosition;
    }
    void Update()
    {
        if (isHovered)
        {
            Vector2 mousePos;
#if ENABLE_INPUT_SYSTEM
            mousePos = Mouse.current.position.ReadValue();
#else
            mousePos = Input.mousePosition;
#endif
            Canvas rootCanvas = rectTransform.root.GetComponent<Canvas>();
            Camera uiCamera = null;

            if (rootCanvas != null && rootCanvas.renderMode == RenderMode.ScreenSpaceCamera)
            {
                uiCamera = rootCanvas.worldCamera;
            }


            Vector2 localMousePos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransform,
                mousePos,
                uiCamera,
                out localMousePos
                );

            Vector2 clamped = Vector2.ClampMagnitude(localMousePos, moveAmount);
            Vector3 targetPos = originalPosition + new Vector3(clamped.x, clamped.y, 0);

            rectTransform.localPosition = Vector3.SmoothDamp(rectTransform.localPosition,
                targetPos, ref velocity, smoothTime);
        }
        else
        {
            rectTransform.localPosition = Vector3.SmoothDamp(rectTransform.localPosition,
                originalPosition, ref velocity, smoothTime);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
    }
}
