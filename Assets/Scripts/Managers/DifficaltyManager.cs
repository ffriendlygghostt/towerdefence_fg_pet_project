using UnityEngine;

public class DifficultyManager : Manager<DifficultyManager>
{
    [Header("Difficulty Multipliers")]
    public float HpMultiplier { get; private set; } = 1f;
    public float SpeedMultiplier { get; private set; } = 1f;
    public float CoinMultiplier { get; private set; } = 1f;
    public float ExpMultiplier { get; private set; } = 1f;

    private void Awake()
    {
        base.Awake();
    }

    public void SetFloor(int floor)
    {
        if (floor <= 20)
        {
            HpMultiplier = 1f + 0.10f * floor; //10%
            SpeedMultiplier = 1f + 0.03f * floor; //3%
            CoinMultiplier = 1f + 0.25f * floor; //25%
            ExpMultiplier = 1f + 0.1f * floor; //10%
        }
        else
        {
            SetEndGameFloor(floor);
        }
    }

    private void SetEndGameFloor(int floor)
    {
        int extraFloors = floor - 20;

        HpMultiplier = 3f + extraFloors *0.0f;
    }
}
