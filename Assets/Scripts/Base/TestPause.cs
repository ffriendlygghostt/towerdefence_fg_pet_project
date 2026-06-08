using UnityEngine;

public class TestPause : MonoBehaviour
{
    public void PressDefeat()
    {
        if (GameFlowManager.Instance.State == GameState.Loading) return;
        BaseManager.Instance.TakeDamage(500);
    }
}
