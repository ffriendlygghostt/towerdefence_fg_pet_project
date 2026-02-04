using TMPro;
using UnityEngine;

public class DefeatScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreTxt;
    [SerializeField] private TMP_Text killsTxt;
    [SerializeField] private TMP_Text wavesTxt;
    [SerializeField] private TMP_Text floorTxt;
    [SerializeField] private TMP_Text timeTxt;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void ShowStatsRun() => ShowStats(GameManager.Instance.GetRunStats());

    private void ShowStats(RunStats stats)
    {
        scoreTxt.text = stats.score.ToString();
        killsTxt.text = stats.kills.ToString();
        wavesTxt.text = stats.waves.ToString();
        floorTxt.text = stats.floor.ToString();
        timeTxt.text = TimeFormatter.ToTime(stats.runTime);
    }

    public void OnRestart() => GameFlowManager.Instance.RestartGame();
    public void OnLeave() => GameFlowManager.Instance.LeaveGame();
}
