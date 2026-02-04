using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : Manager<SceneLoader>
{
    private Scene currentScene;
    private bool isLoading = false;
    private Coroutine loadRoutine;

    public void LoadScene(string newSceneName, Action onLoaded = null, Action onCompleteFade = null)
    {
        if (isLoading) return;
        if (loadRoutine != null)
        {
            return;
        }

        isLoading = true;

        loadRoutine = StartCoroutine(LoadSceneRoutine(newSceneName, onLoaded, onCompleteFade));
    }

    public void LoadScene(LevelEntry levelEntry, Action onLoaded = null, Action onCompleteFade = null)
    {
        if (isLoading) return;
        if (loadRoutine != null)
        {
            return;
        }

        isLoading = true;

        loadRoutine = StartCoroutine(LoadSceneRoutine(levelEntry.sceneName, onLoaded, onCompleteFade));
    }

    private IEnumerator LoadSceneRoutine(string sceneName, Action onLoaded = null, Action onCompleteFade = null)
    {
        bool fadeDone = false;
        FadeManager.FadeOutThen(() => fadeDone = true, 1.5f);
        yield return new WaitUntil(() => fadeDone);

        if (currentScene.IsValid())
        {
            yield return SceneManager.UnloadSceneAsync(currentScene);
            currentScene = default;
        }

        AsyncOperation loadOp = 
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        yield return loadOp;

        Scene newScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1); // LastID
        SceneManager.SetActiveScene(newScene);
        currentScene = newScene;


        FadeManager.FadeInThen(
            () => { onCompleteFade?.Invoke(); },
            1.5f
            );

        onLoaded?.Invoke();

        isLoading = false;
        loadRoutine = null;
    }

    public void SetCurrentScene(Scene scene)
    {
        currentScene = scene;
    }
}
