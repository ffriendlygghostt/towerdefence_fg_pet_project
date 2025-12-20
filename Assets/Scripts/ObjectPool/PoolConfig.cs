using UnityEngine;

[CreateAssetMenu(fileName = "PoolConfig", menuName = "Pooling/Pool Config")]
public class PoolConfig : ScriptableObject
{
    public GameObject prefab;
    public int initialSize = 10;
    public int expandSize = 5;
}
