using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemySpawnerManager : Manager<EnemySpawnerManager>
{
    [Header("Demo Spawn Settings")]
    [SerializeField] private EnemyType[] enemiesToSpawn;
    [SerializeField] private int countPerEnemy = 2;
    [SerializeField] private float spawnDelay = 1.33f;

    [Header("Wave Timings")]
    [SerializeField] private float preWaveDelay = 10f;
    [SerializeField] private float postWaveDelay = 15f;

    [Header("UI")]
    [SerializeField] private GameObject timerWaveCanvas;
    [SerializeField] private TextMeshProUGUI timerTXT;

    [Header("Path Preview")]
    [SerializeField] private GameObject skullPreviewPrefab;
    [SerializeField] private float previewRepeatDelay = 0.7f;
    [SerializeField] private float previewSpawnDelay = 0.02f;

    private int[] aliveEnemiesPerWave;
    private bool[] waveSpawnFinished;
    private bool[] waveCleared;

    public int CurrentWave { get; private set; } = 0;
    private int maxWave = 5;

    private SceneEnabler currentSceneData;
    private bool isSpawning = false;
    private Coroutine waveLoopCoroutine;

    private int[] currentWavePathIndexes;

    private float[] alphasPreview = { 1f, 0.65f, 0.45f };

    // -------------------------
    // INITIALIZATION
    // -------------------------

    public void SceneEnabled(int MaxWave = 5)
    {
        currentSceneData = FindObjectOfType<SceneEnabler>();

        if (currentSceneData == null) { Debug.LogError("SceneEnabler not found!"); }

        maxWave = MaxWave;

        HudManager.Instance.SetTotalWaveIcons(maxWave);

        WaveGenerator.Instance.InitializationGenerator(maxWave, DifficultyManager.Instance.WeightWaves);

        ResetNotify(MaxWave);
    }

    // -------------------------
    // PUBLIC API
    // -------------------------

    public void StartSpawning()
    {
        if (isSpawning) return;

        if (currentSceneData == null) { Debug.LogError("No SceneEnabler assigned.");
            return; 
        }
        isSpawning = true;
        waveLoopCoroutine = StartCoroutine(WaveLoop());
    }

    public void StopSpawning()
    {
        if (!isSpawning && waveLoopCoroutine == null)
            return;

        isSpawning = false;
        if(waveLoopCoroutine != null)
        {
            StopCoroutine(waveLoopCoroutine);
            waveLoopCoroutine = null;
        }

        CurrentWave = 0;
        HudManager.Instance.TimerWaveHide();
    }

    // -------------------------
    // MAIN LOOP
    // -------------------------

    private IEnumerator WaveLoop()
    {
        while (isSpawning && CurrentWave != maxWave)
        {
            CurrentWave++;
            if(CurrentWave == maxWave)
            {
                isSpawning = false;
            }

            currentWavePathIndexes = GetPathsForWave();

            HudManager.Instance.SetCurrentWaveIcon(CurrentWave);

            yield return StartCoroutine(PreWaveDelay());

            aliveEnemiesPerWave[CurrentWave] = 0;
            yield return StartCoroutine(SpawnWave());

            yield return StartCoroutine(PostWaveDelay());
        }

        waveLoopCoroutine = null;
        HudManager.Instance.TimerWaveHide();
    }

    // -------------------------
    // PRE-WAVE
    // -------------------------

    private IEnumerator PreWaveDelay()
    {
        float timer = preWaveDelay;
        HudManager.Instance.TimerWaveShow();

        Coroutine previewRoutine = StartCoroutine(PathPreviewLoop());

        while(timer > 0f)
        {
            HudManager.Instance.TimerWaveSet(timer);

            timer -= Time.deltaTime *
                SpeedGameManager.Instance.SpeedMultiplier;

            yield return null;
        }

        if (previewRoutine != null)
        {
            StopCoroutine(previewRoutine);
        }

        HudManager.Instance.TimerWaveHide();
    }

    private IEnumerator PathPreviewLoop()
    {
        yield return new WaitForSeconds(1f);
        SpawnPreviewForCurrentWave();
        yield return WaitScaled(0.3f);

        while (true)
        {
            SpawnPreviewForCurrentWave();
            yield return WaitScaled(previewRepeatDelay);
        }
    }

    private void SpawnPreviewForCurrentWave()
    {
        if (skullPreviewPrefab == null)
            return;

        foreach (int pathIndex in currentWavePathIndexes)
        {
            var path = currentSceneData.enemyPaths[pathIndex].points;

            if (path == null || path.Length == 0) continue;

            StartCoroutine(SpawnSkullTrail(path));
        }
    }

    private IEnumerator SpawnSkullTrail(Transform[] path)
    {
        for (int i = 0; i < 3; i++) 
        {
            GameObject skull = Instantiate(skullPreviewPrefab);
            var preview = skull.GetComponent<PathPreview>();
            if (preview != null)
            {
                preview.Init(path);
            }

            var sp = skull.GetComponent<SpriteRenderer>();
            if (sp != null)
            {
                Color color = sp.color;
                color.a = alphasPreview[i];
                sp.color = color;
            }
            yield return null;

            yield return WaitScaled(previewSpawnDelay);
        }

    }

    // -------------------------
    // SPAWN WAVE
    // -------------------------

    private IEnumerator SpawnWave()
    {
        List<EnemyType> waveEnemies = 
            WaveGenerator.Instance.GetEnemyWaveList(CurrentWave-1);

        if (waveEnemies == null || waveEnemies.Count == 0)
        {
            Debug.LogWarning("Wave is empty!");
            yield break;
        }

        foreach (var enemyType in waveEnemies)
        {
            SpawnEnemy(enemyType, currentWavePathIndexes);

            yield return WaitScaled(spawnDelay);
        }
        waveSpawnFinished[CurrentWave] = true;
    }

    // -------------------------
    // POST WAVE
    // -------------------------

    private IEnumerator PostWaveDelay() 
    { 
        yield return WaitScaled(postWaveDelay); 
    }

    // -------------------------
    // PATH SELECTION
    // -------------------------

    private int[] GetPathsForWave()
    {
        //Customs
        if (currentSceneData.useCustomWaveSettings &&
            currentSceneData.waveCustomPaths != null &&
            CurrentWave - 1 < currentSceneData.waveCustomPaths.Length)
        {
            var custom = currentSceneData.waveCustomPaths[CurrentWave - 1].pathIndexes;

        // Random
            if (custom == null || custom.Length == 0)
            {
                return GetRandomSinglePath();
            }

            return custom;
        }

        return GetRandomSinglePath();
    }

    private int[] GetRandomSinglePath()
    {
        int index = Random.Range(0, currentSceneData.enemyPaths.Length);
        return new int[] { index };
    }

    // -------------------------
    // ENEMY SPAWN
    // -------------------------

    private void SpawnEnemy(EnemyType type, int[] pathIndexes)
    {
        var go = PoolManager.Instance.Get(type.ToString());

        if (go == null) return;

        int pathIndex = pathIndexes[Random.Range(0, pathIndexes.Length)];

        Transform[] path =
            currentSceneData.enemyPaths[pathIndex].points;

        if (path == null || path.Length == 0) 
        {
            Debug.LogError($"Path {pathIndex} is empty");
            return;
        }


        go.transform.position = path[0].position;

        var movement = go.GetComponent<EnemyMovement>();
        var controller = go.GetComponent<EnemyController>();

        if (controller == null)
        {
            Debug.LogError("EnemyController is Null! Spawn canceling!");
            return;
        }
        controller.SetWaveIndex(CurrentWave);
        aliveEnemiesPerWave[CurrentWave]++;

        movement.SetPath(path);
        go.SetActive(true);
        movement.StartMovement();
    }

    // -------------------------
    // ENEMY DIE
    // -------------------------
    public void NotifyEnemyKilled(int waveIndex)
    {
        aliveEnemiesPerWave[waveIndex]--;
        if (waveSpawnFinished[waveIndex] &&
            !waveCleared[waveIndex] &&
            aliveEnemiesPerWave[waveIndex] == 0)
        {
            waveCleared[waveIndex] = true;
            GameManager.Instance.WaveCleared();
        }
    }

    private void ResetNotify(int maxWave = 5)
    {
        aliveEnemiesPerWave = new int[maxWave+1];
        waveSpawnFinished = new bool[maxWave+1];
        waveCleared = new bool[maxWave+1];
    }

    // -------------------------
    // CUSTOM WAIT
    // -------------------------

    private IEnumerator WaitScaled(float duration)
    {
        float timer = duration;

        while (timer > 0f)
        {
            timer -= Time.deltaTime *
                SpeedGameManager.Instance.SpeedMultiplier;
            yield return null;
        }
    }
}
    


