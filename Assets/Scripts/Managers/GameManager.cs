using UnityEngine;

public struct RunStats
{
    public int kills;
    public int score;
    public int waves;
    public int floor;
    public float runTime;
}

public class GameManager : Manager<GameManager>
{
    private int killsThisRun;
    private int scoreThisRun;
    private int wavesCleared;
    private int floorThisRun;

    private float runTime;
    private bool isRunning = false;



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

    private void Update()
    {
        if (!isRunning) return;
        runTime += Time.deltaTime;
    }

    public void StartTimer()
    {
        runTime = 0;
        isRunning = true;
    }

    public void EndTimer()
    {
        isRunning = false;
    }


    public RunStats GetRunStats()
    {
        return new RunStats
        {
            kills = killsThisRun,
            score = scoreThisRun,
            waves = wavesCleared,
            floor = floorThisRun,
            runTime = runTime
        };
    }

    private void Reset()
    {
        killsThisRun = 0;
        scoreThisRun = 0;
        wavesCleared = 0;
    }

    public void StartGame()
    {
        StartTimer();
    }
}
