using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecordsPanel : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI bestScoreText;
    [SerializeField] public TextMeshProUGUI mostWavesText;
    [SerializeField] public TextMeshProUGUI totalWavesText;
    [SerializeField] public TextMeshProUGUI totalKilledText;

    private void OnEnable()
    {
        var stats = SaveManager.Instance.Data.stats;

        bestScoreText.text = stats.bestScore.ToString();
        mostWavesText.text = stats.mostWaves.ToString();
        totalWavesText.text = stats.totalWaves.ToString();
        totalKilledText.text = stats.totalKilled.ToString();
    }
}
