using UnityEngine;

public class SceneEnabler : MonoBehaviour
{
    [Header("Paths for enemies")]
    public EnemyPath[] enemyPaths;
    [Header("Custom wave settings")]
    public WavePathConfig[] waveCustomPaths;
    [Header("Other settings")]
    public bool useCustomWaveSettings = false;

}
