using Unity.VisualScripting;
using UnityEngine;

public class BootSceneLoader : MonoBehaviour
{
    [SerializeField] private string startScene = "MainMenu";

    void Start()
    {
        StartCoroutine(LoadSceneAsync());
    }

    // Update is called once per frame
    
}
