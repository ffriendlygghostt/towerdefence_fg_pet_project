using Unity.VisualScripting;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public void SettingsClick() =>
        SettingsMenuController.Instance.Show();
}
