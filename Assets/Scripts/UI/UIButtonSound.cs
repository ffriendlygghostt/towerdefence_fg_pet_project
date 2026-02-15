using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public SoundSO customClickSound;
    public SoundSO customHoverSound;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (customHoverSound == null)
            AudioManager.Instance.PlayUISFXType(SoundEnum.HoverButtonDefault);
        else
            AudioManager.Instance.PlayUISFX(customHoverSound); 
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (customClickSound == null)
            AudioManager.Instance.PlayUISFXType(SoundEnum.ClickButtonDefault);
        else
            AudioManager.Instance.PlayUISFX(customClickSound);
    }
}
