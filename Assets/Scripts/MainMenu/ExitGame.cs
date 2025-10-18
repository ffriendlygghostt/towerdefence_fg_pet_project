using UnityEngine;

public class ExitGame : MonoBehaviour
{
    public void ExitButton()
    {
        FadeManager.FadeOutThen(() => {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }, 1.5f);
}
}
