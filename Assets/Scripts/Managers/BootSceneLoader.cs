using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BootSceneLoader : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] public Slider progressBar;
    [SerializeField] public Image background;
    [SerializeField] private string startScene = "MainMenu";

    [Header("Settings")]
    [SerializeField] private float minLoadTime = 1.5f;   
    [SerializeField] private float smoothSpeed = 0.03f; 

    [Header("ClearObjects")] [SerializeField]
    public GameObject[] clearObjects;

    void Start()
    {
        StartCoroutine(LoadMainScene());
    }

    private IEnumerator LoadMainScene()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(startScene, LoadSceneMode.Additive);
        op.allowSceneActivation = false;

        float timer = 0f;
        float targetProgress = 0f;
        float displayedProgress = 0f;

        while (!op.isDone)
        {
            timer += Time.deltaTime;

            float sceneProgress = Mathf.Clamp01(op.progress / 0.9f);
            float timerProgress = Mathf.Clamp01(timer / minLoadTime);

            // Выбираем то, что дальше (грузим минимум minLoadTime)
            targetProgress = Mathf.Max(sceneProgress, timerProgress);

            // Плавно приближаем отображаемое значение
            displayedProgress = Mathf.Lerp(displayedProgress, targetProgress, smoothSpeed * Time.deltaTime * 60f);
            progressBar.value = displayedProgress;

            // Проверка на завершение
            if (sceneProgress >= 1f && timerProgress >= 1f)
            {
                yield return new WaitForSeconds(1f);
                op.allowSceneActivation = true;
                while (!op.isDone)
                    yield return null;
            }

            yield return null;
        }

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(startScene));
        foreach (var clearObject in clearObjects) { Destroy(clearObject); }
    }
    
}
