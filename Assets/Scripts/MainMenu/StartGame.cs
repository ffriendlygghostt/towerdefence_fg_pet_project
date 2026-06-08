using UnityEngine;

public class StartGame : MonoBehaviour
{
    public void StartGameClick()
    {
        if (SettingsMenuController.Instance.gameObject.activeInHierarchy)
        {
            SettingsMenuController.Instance.Hide();
        }

        GameFlowManager.Instance.StartRun();
    }
}
