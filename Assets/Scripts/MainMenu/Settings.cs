using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Settings : MonoBehaviour
{
    public void SettingsClick()
    {
        if (!SettingsMenuController.Instance.isActiveAndEnabled)
        {
            SettingsMenuController.Instance.Show();
        }
        else if (SettingsMenuController.Instance.isActiveAndEnabled)
        {
            SettingsMenuController.Instance.Hide();
        }
    }
        
}
