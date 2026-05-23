using UnityEngine;

public class DifficultyManager : Manager<DifficultyManager>
{
    [Header("Difficulty Multipliers")]
    public float HpMultiplier { get; private set; } = 1f;
    public float SpeedMultiplier { get; private set; } = 1f;
    public float CoinMultiplier { get; private set; } = 1f;

    public int baseWeight = 2000;
    public int WeightWaves { get; private set; } = 2000;

    public float WavePressureMultiplier { get; private set; } = 1f;

    public void SetFloor(int floor)
    {
        if (floor <= 20)
        {
            HpMultiplier = 1f + 0.07f * floor; //7%
            SpeedMultiplier = 1f + 0.025f * floor; //2.5%
            CoinMultiplier = 1f + 0.25f * floor; //25%
            WavePressureMultiplier = 1f + floor * 0.05f;
            WeightWaves = Mathf.RoundToInt(baseWeight * WavePressureMultiplier);
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
