using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public SoundSO sound;
    public void ButtonClick()
    {
        AudioManager.Instance.PlaySFX(sound);
    }
}
