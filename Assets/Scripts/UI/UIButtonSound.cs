using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public SoundSO customClickSound;
    public SoundSO customHoverSound;
    public SoundSO customErrorSound;

    [SerializeField] private bool useAutomaticHandlers = true;


    /////////////////////
    ///  Auto
    /////////////////////

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!useAutomaticHandlers)
            return;

        PlayHoverSound();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!useAutomaticHandlers)
            return;

        PlayClickSound();
    }


    /////////////////////
    ///  Handler Methods
    /////////////////////

    public void PlayClickSound()
    {
        if (customClickSound == null)
            AudioManager.Instance.PlayUISFXType(SoundDefaultEnum.ButtonClick);
        else
            AudioManager.Instance.PlayUISFX(customClickSound);
    }

    public void PlayHoverSound()
    {
        if (customHoverSound == null)
            AudioManager.Instance.PlayUISFXType(SoundDefaultEnum.ButtonHover);
        else
            AudioManager.Instance.PlayUISFX(customHoverSound);
    }

    public void PlayErrorSound()
    {
        if (customErrorSound == null)
            AudioManager.Instance.PlayUISFXType(SoundDefaultEnum.ButtonError);
        else
            AudioManager.Instance.PlayUISFX(customErrorSound);
    }

}
