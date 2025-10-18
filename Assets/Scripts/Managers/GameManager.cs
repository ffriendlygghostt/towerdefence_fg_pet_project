using UnityEngine;

public class GameManager : Manager<GameManager>
{
    private int killsThisRun;
    private int scoreThisRun;
    private int wavesCleared;

    public void AddKill()
    {
        killsThisRun++;
    }

    public void AddScore(int points)
    {
        scoreThisRun += points;
    }

    public void WaveCleared()
    {
        wavesCleared++;
    }

    public void EndRun()
    {
        var stats = SaveManager.Instance.Data.stats;
        stats.totalKilled += killsThisRun;
        stats.totalWaves += wavesCleared;
        
        if (scoreThisRun > stats.bestScore)
        {
            stats.bestScore = scoreThisRun;
        }

        if (wavesCleared > stats.mostWaves)
        {
            stats.mostWaves = wavesCleared;
        }

        SaveManager.Instance.Save();
        Reset();
    }

    private void Reset()
    {
        killsThisRun = 0;
        scoreThisRun = 0;
        wavesCleared = 0;
    }
}
