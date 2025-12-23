using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : Manager<SceneLoader>
{
    private Scene currentScene;

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    public void LoadScene(LevelEntry levelEntry)
    {
        StartCoroutine(LoadSceneRoutine(levelEntry.sceneName));
    }

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        currentScene = SceneManager.GetActiveScene();

        bool fadeDone = false;
        FadeManager.FadeOutThen(() => fadeDone = true, 2f);
        yield return new WaitUntil(() => fadeDone);

        AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        yield return loadOp;

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));

        AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(currentScene);
        yield return unloadOp;

        FadeManager.FadeIn(2f);
    }
}
