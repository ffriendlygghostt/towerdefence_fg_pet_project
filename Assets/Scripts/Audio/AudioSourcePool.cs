using System.Collections.Generic;
using UnityEngine;

public class AudioSourcePool : MonoBehaviour
{
    private Queue<AudioSource> pool  = new Queue<AudioSource>();
    private Transform parent;
    private int expandSize;
    private GameObject sourcePrefab;

    private HashSet<AudioSource> activeSources = new HashSet<AudioSource>();
    public IReadOnlyCollection<AudioSource> ActiveSources => activeSources;

    public AudioSourcePool(GameObject prefab, int initialSize, int expandSize, Transform parent)
    {
        this.parent = parent;
        this.expandSize = Mathf.Max(10, expandSize);
        sourcePrefab = prefab;
        CreateBatch(initialSize);
    }

    private void CreateBatch(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject go = GameObject.Instantiate(sourcePrefab, parent);
            go.SetActive(false);
            pool.Enqueue(go.GetComponent<AudioSource>());
        }
    }

    public AudioSource Get()
    {
        if (pool.Count == 0) CreateBatch(expandSize);

        AudioSource source = pool.Dequeue();
        source.gameObject.SetActive(true);

        activeSources.Add(source);

        return source;
    }

    public void Return(AudioSource source)
    {
        if (!activeSources.Remove(source))
            return;

        source.Stop();
        source.clip = null;
        source.pitch = 1f;

        source.gameObject.SetActive(false);

        pool.Enqueue(source);
    }
}
