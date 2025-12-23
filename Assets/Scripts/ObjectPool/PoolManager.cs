using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Manager<PoolManager>
{
    [SerializeField] public PoolConfig[] poolConfigs;

    private Dictionary<string, ObjectPool> pools = new Dictionary<string, ObjectPool>();

    private Transform poolsRoot;

    protected virtual void Awake()
    {
        base.Awake();

        poolsRoot = new GameObject("Pools").transform;
        poolsRoot.SetParent(transform);

        foreach (var config in poolConfigs)
        {
            if (!config.prefab.TryGetComponent<IPoolIdentity>(out var identity))
            {
                Debug.LogError($"Prefab {config.prefab.name} has no IPoolIdentity");
                continue;
            }

            string poolId = identity.GetPoolId();

            if (pools.ContainsKey(poolId))
            {
                Debug.LogError($"Duplicate poolId: {poolId}");
                continue;
            }

            var parent = new GameObject($"{poolId}_Pool").transform;
            parent.SetParent(poolsRoot);

            var pool = new ObjectPool(
                config.prefab,
                config.initialSize,
                config.expandSize,
                parent
                );

            pools.Add(poolId, pool);
        }
    }

    public GameObject Get(string poolId)
    {
        if (!pools.TryGetValue(poolId, out var pool))
        {
            Debug.LogError($"Pool with id '{poolId}' not found");
            return null;
        }
        return pool.Get();
    }

    public void Return(GameObject obj)
    {
        if (!obj.TryGetComponent<IPoolIdentity>(out var identity))
        {
            Debug.LogError($"Object {obj.name} has no IPoolIdentity");
            return;
        }

        string poolId = identity.GetPoolId();

        if (!pools.TryGetValue(poolId, out var pool))
        {
            Debug.LogError($"Pool with id '{poolId}' not found for object {obj.name}");
            return;
        }

        pool.Return(obj);
    }
}
