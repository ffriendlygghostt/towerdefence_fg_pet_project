using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class LevelManager : Manager<LevelManager>
{
    [Header("Levels")]
    [SerializeField] private List<LevelEntry> levels = new();

    private int currentLevelIndex = -1;
    private Queue<int> lastPlayed = new(3);

    public int CurrentLevelIndex => currentLevelIndex;
    public LevelEntry CurrentLevel =>
        currentLevelIndex >= 0 ? levels[currentLevelIndex] : null;

    public LevelEntry GetRandomLevel()
    {
        if (levels.Count == 0)
        {
            Debug.LogError("No levels configured!");
            return null;
        }

        if (levels.Count < 5)
        {
            currentLevelIndex = Random.Range(0, levels.Count);
            return levels[currentLevelIndex];
        }

        int index;
        int safety = 0;

        do
        {
            index = Random.Range(0, levels.Count);
            safety++;
        }
        while (lastPlayed.Contains(index) && safety < 20);

        RegisterPlayed(index);
        currentLevelIndex = index;
        return levels[index];
    }

    private void RegisterPlayed(int index)
    {
        lastPlayed.Enqueue(index);

        if (lastPlayed.Count >2)
            lastPlayed.Dequeue();
    }
}
