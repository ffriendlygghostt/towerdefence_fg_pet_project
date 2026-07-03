using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelBootstrap : MonoBehaviour
{
    [SerializeField] private Camera levelCamera;
    void Start()
    {
        if (levelCamera == null)
        {
            Scene activeScene = SceneManager.GetActiveScene();
            GameObject[] rootGO = activeScene.GetRootGameObjects();

            foreach (GameObject go in rootGO)
            {
                levelCamera = go.GetComponentInChildren<Camera>(true);
                if (levelCamera != null)
                {
                    break;
                }
            }
        }
        WorldCameraService.Instance.SetActiveCam(levelCamera);
    }
}
