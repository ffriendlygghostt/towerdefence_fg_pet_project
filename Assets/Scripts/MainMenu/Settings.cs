using UnityEngine;

public class Settings : MonoBehaviour
{
    public Canvas settingsCanvas;

    public void SettingsClick()
    {
        if (settingsCanvas == null)
        {
            settingsCanvas.enabled = true;
        }
        else
        {
            Debug.LogWarning("No SettingsCanvas!");
        }
    }
}
