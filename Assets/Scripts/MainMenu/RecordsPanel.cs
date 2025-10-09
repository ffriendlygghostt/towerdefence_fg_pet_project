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

        bestScoreText.text = stats.bestScore.ToString("00000");
        mostWavesText.text = stats.mostWaves.ToString("00000");
        totalWavesText.text = stats.totalWaves.ToString("00000");
        totalKilledText.text = stats.totalKilled.ToString("00000");
    }
}
