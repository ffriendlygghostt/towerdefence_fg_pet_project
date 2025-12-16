using UnityEngine;

public class StartGame : MonoBehaviour
{
    public void StartGameClick()
    {
        GameFlowManager.Instance.StartRun();
    }
}
