using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class ObjectPool : MonoBehaviour
{
    private GameObject prefab;
    private Transform parent;
    private int expandSize;

    private Queue<GameObject> pool = new Queue<GameObject>();

    public ObjectPool(GameObject prefab, int initialSize, int expandSize, Transform parent)
    {
        this.prefab = prefab;
        this.parent = parent;
        this.expandSize = Mathf.Max(5, expandSize);

        CreateBatch(initialSize);
    }

    private void CreateBatch(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject obj = GameObject.Instantiate(prefab, parent);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public GameObject Get()
    {
        if (pool.Count == 0)
        {
            CreateBatch(expandSize);
        }

        GameObject obj = pool.Dequeue();
        obj.transform.SetParent(null);
        SceneManager.MoveGameObjectToScene(obj, SceneManager.GetActiveScene());
        obj.SetActive(true);

        return obj;
    }
    
    public void Return(GameObject obj)
    {
        obj.SetActive(false);
        //SceneManager.MoveGameObjectToScene(obj, gameObject.scene);
        obj.transform.SetParent(parent);
        pool.Enqueue(obj);
    }
}
