using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : Manager<FadeManager>
{
    [Header("References")]
    [SerializeField] public CanvasGroup fadeCanvas;
    [SerializeField] public Image fadeImage;

    [Header("Default Settings")] [SerializeField]
    private float defaultDuration = 1.5f;
    [SerializeField] private Color defaultColor = Color.black;

    private Coroutine currentFade;

    protected override void Awake()
    {
        base.Awake();

        if (fadeCanvas != null)
        {
            fadeCanvas.alpha = 0f;
            fadeCanvas.blocksRaycasts = false;
        }
    }

    public static void FadeIn(float duration = -1f, Color? color = null)
    {
        if (Instance != null)
        {
            Instance.StartFade(false, duration, color);
        }
    }

    public static void FadeOut(float duration = -1f, Color? color = null)
    {
        if (Instance != null)
        {
            Instance.StartFade(true, duration, color);
        }
    }

    public static void FadeOutThen(System.Action onComplete, float duration = -1f, Color? color = null)
    {
        if (Instance != null)
            Instance.StartFade(true, duration, color, onComplete);
    }

    private void StartFade(bool fadeOut, float duration, Color? color = null, System.Action onComplete = null)
    {
        if (currentFade != null)
        {
            StopCoroutine(currentFade);
        }

        currentFade = StartCoroutine(FadeRoutine(fadeOut, duration, color, onComplete));
    }

    private IEnumerator FadeRoutine(bool fadeOut, float duration, Color? color, System.Action onComplete)
    {
        if (fadeCanvas == null)
        {
            yield break;
        }

        fadeCanvas.blocksRaycasts = true;
        fadeImage.color = color ?? defaultColor;

        float time = 0f;
        float start = fadeCanvas.alpha;
        float end = fadeOut ? 1.5f : 0f;

        if (duration <= 0)
        {
            duration = defaultDuration;
        }

        while (time < duration)
        {
            time += Time.deltaTime;
            fadeCanvas.alpha = Mathf.Lerp(start, end, time / duration);
            yield return null;
        }

        fadeCanvas.alpha = end;
        if (!fadeOut)
        {
            fadeCanvas.blocksRaycasts = false;
        }

        onComplete?.Invoke();
        currentFade = null;
    }
}
