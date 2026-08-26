using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// WaveGenerator V3
/// Требуется доработать длину групп, чтобы добиться меньшего разнобоя
///         доработать модификаторы (по вкусу)
///         отбалансировать логику (когда всё будет играться)
/// </summary>
public class WaveGenerator : Manager<WaveGenerator>
{
    [Header("EnemyPrefabs")]
    [SerializeField] private List<GameObject> enemyPrefabs;
    
    private Dictionary<EnemyRoleTier, List<EnemyType>> roleMap;
    private Dictionary<EnemyType, EnemyStats> statsMap;

    private List<List<EnemyType>> waves;
    private List<int> waveBudgets;

    private int maxWaveCount = 5;

    private void Start()
    {
        InitMap();
    }

    private void InitMap()
    {
        statsMap = new();
        roleMap = new();

        foreach (var prefab in enemyPrefabs)
        {
            var stats = prefab.GetComponent<EnemyStats>();

            statsMap[stats.type] = stats;

            if (!roleMap.ContainsKey(stats.role))
            {
                roleMap[stats.role] = new List<EnemyType>();
            }
            roleMap[stats.role].Add(stats.type);
        }
    }



    
    public void InitializationGenerator(int maxWave, int weightFloor)
    {
        maxWaveCount = maxWave;

        waves = new List<List<EnemyType>>(maxWave);

        waveBudgets = BuildWaveBudgets(maxWave, weightFloor);

        for (int wave = 0; wave < maxWave; wave++)
        {
            waves.Add(BuildWave(wave));
        }
    }

    private List<int> BuildWaveBudgets(int maxWave, int totalWeight)
    {
        List<int> result = new();

        int sum = 0;
        for(int i = 1; i <= maxWave; i++)
        {
            sum += i;
        }

        int allocated = 0;

        for (int i = 1; i <= maxWave; i++)
        {
            int budget = totalWeight * i / sum; // 1000:  66   133   200   266   335
            result.Add(budget);
            allocated += budget;
        }

        result[result.Count - 1] += totalWeight - allocated;
        return result;
    }

    private List<EnemyType> BuildWave(int waveIndex)
    {
        int budget = waveBudgets[waveIndex];
        List<EnemyType> result = new();

        HashSet<WaveModifier> modifiers =
                GenerateModifiers(waveIndex);

        if (IsBossWave(waveIndex) && roleMap.ContainsKey(EnemyRoleTier.Boss))
        {
            EnemyType boss = PickBoss();
            int bossCost = statsMap[boss].cost;

            if (bossCost <= budget)
            {
                result.Add(boss);
                budget -= bossCost;
            }
        }

        int safetyCounter = 1000;

        while (budget > 0 &&
            safetyCounter-- > 0 &&
            HasEnemyFitBudget(budget))
        {
            EnemyRoleTier role = PickRoleByWave(waveIndex);

            // роль открыта.
            if (!IsRoleUnlocked(role, waveIndex))
                continue;

            // для роли есть враги.
            if (!roleMap.ContainsKey(role) || roleMap[role].Count == 0)
                continue;

            EnemyType type = PickEnemy(role);
            int cost = statsMap[type].cost;

            // Если не помещается в бюджет
            if (cost > budget)
                continue;

            

            int count = GetGroupCount(
                role,
                waveIndex,
                modifiers);

            for (int i=0; i<count; i++)
            {
                if (cost > budget)
                {
                    break;
                }
                result.Add(type);
                budget -= cost;
            }
        }

        return result;
    }

    ///////////////////////////////
    /// Role Selection
    ///////////////////////////////
    private EnemyRoleTier PickRoleByWave(int wave)
    {
        int roll = Random.Range(0, 100);

        if (wave == 0)
            return EnemyRoleTier.Swarm_1;

        if (wave <= 1)
        {
            if (roll < 70) return EnemyRoleTier.Swarm_1;
            if (roll < 90) return EnemyRoleTier.Tank_1;
            return EnemyRoleTier.Fast_1;
        }

        if (wave <= 2)
        {
            if (roll < 50) return EnemyRoleTier.Swarm_2;
            if (roll < 75) return EnemyRoleTier.Tank_2;
            return EnemyRoleTier.Fast_2;
        }

        return (EnemyRoleTier)Random.Range(0, roleMap.Count);
    }

    ///////////////////////////////
    /// Helpers
    ///////////////////////////////
    private bool IsBossWave(int wave)
    {
        return wave == maxWaveCount - 1;
    }

    private EnemyType PickEnemy(EnemyRoleTier role)
    {
        var pool = roleMap[role];
        return pool[Random.Range(0, pool.Count)];
    }

    private EnemyType PickBoss()
    {
        var bosses = roleMap[EnemyRoleTier.Boss];
        return bosses[Random.Range(0, bosses.Count)];
    }

    private bool IsRoleUnlocked(EnemyRoleTier role, int wave)
    {
        return role switch
        {
            EnemyRoleTier.Swarm_1 => true,
            EnemyRoleTier.Tank_1 => wave >= 0,
            EnemyRoleTier.Fast_1 => wave >= 0,

            EnemyRoleTier.Tank_2 => wave >= 1,
            EnemyRoleTier.Swarm_2 => wave >= 1,

            EnemyRoleTier.Tank_3 => wave >= 2,
            EnemyRoleTier.Fast_2 => wave >= 2,
            EnemyRoleTier.Swarm_3 => wave >= 2,

            EnemyRoleTier.Fast_3 => wave >= 3,

            EnemyRoleTier.Boss => wave >= 4,

            _ => false
        };
    }

    private bool HasEnemyFitBudget(int budget)
    {
        foreach (var stats in statsMap.Values)
        {
            if (stats.cost <= budget)
            {
                return true;
            }
        }
        return false;
    }

    ///////////////////////////////
    /// Grouping
    ///////////////////////////////
    private int GetGroupCount(EnemyRoleTier role, int wave, HashSet<WaveModifier> modifiers)
    {
        if (modifiers.Contains(WaveModifier.SwarmRush))
        {
            if (role == EnemyRoleTier.Swarm_1) 
            { 
                return Random.Range(8, 14); 
            }
            if (role == EnemyRoleTier.Swarm_2)
            {
                return Random.Range(7, 12);
            }
            if (role == EnemyRoleTier.Swarm_3)
            {
                return Random.Range(5, 10);
            }
        }

        if (modifiers.Contains(WaveModifier.TankOverflow))
        {
            if (role == EnemyRoleTier.Tank_1)
                return Random.Range(6, 12);

            if (role == EnemyRoleTier.Tank_2)
                return Random.Range(5, 10);
        }

        if (modifiers.Contains(WaveModifier.FastRush))
        {
            if (role == EnemyRoleTier.Fast_1)
                return Random.Range(6, 8);

            if (role == EnemyRoleTier.Fast_2)
                return Random.Range(4, 7);
        }



        switch (role)
        {
            case EnemyRoleTier.Swarm_1:
                return Random.Range(4, 8);
            case EnemyRoleTier.Swarm_2:
                return Random.Range(3, 7);
            case EnemyRoleTier.Swarm_3:
                return Random.Range(3, 6);


            case EnemyRoleTier.Tank_1:
                if (wave <= 1)
                {
                    return Random.Range(1, 3);
                }
                if (wave <= 3)
                {
                    return Random.Range(3, 6);
                }
                return Random.Range(2, 4);

            case EnemyRoleTier.Tank_2:
                if (wave <= 2)
                {
                    return Random.Range(2, 3);
                }
                if (wave <= 4)
                {
                    return Random.Range(3, 6);
                }
                return Random.Range(2, 3);

            case EnemyRoleTier.Tank_3:
                return 1;

            case EnemyRoleTier.Fast_1:
                if (wave <= 2)
                {
                    return Random.Range(2, 4);
                }
                if (wave <= 4)
                {
                    return Random.Range(4, 8);
                }
                return Random.Range(2, 4);

            case EnemyRoleTier.Fast_2:
                if (wave <= 2)
                {
                    return Random.Range(1, 3);
                }
                if (wave <= 4)
                {
                    return Random.Range(3, 5);
                }
                return Random.Range(1, 2);

            case EnemyRoleTier.Fast_3:
                return 1;

        }
        return 1;
    }



    ///////////////////////////////
    /// GETING
    ///////////////////////////////
    public List<EnemyType> GetEnemyWaveList(int currentWave)
    {
        if (waves == null || currentWave < 0 || currentWave >= waves.Count)
        {
            Debug.LogError("WavesList is Null!");
            return new List<EnemyType>();
        }
        return waves[currentWave];
    }


    private HashSet<WaveModifier> GenerateModifiers(int wave)
    {
        HashSet<WaveModifier> modifiers = new();

        if (Random.value < 0.08f)
        {
            modifiers.Add(WaveModifier.TankOverflow);
        }

        if (Random.value < 0.16f)
        {
            modifiers.Add(WaveModifier.SwarmRush);
        }

        if (Random.value < 0.06f)
        {
            modifiers.Add(WaveModifier.FastRush);
        }
        
        return modifiers;
    }

}


public enum WaveModifier
{
    TankOverflow,
    SwarmRush,
    FastRush
}

