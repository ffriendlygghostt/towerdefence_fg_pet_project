using System.Collections;
using UnityEngine;

public class PathPreview : MonoBehaviour
{
    public float speed = 4f;
    private Transform[] path;
    private int index = 0;

    public void Init(Transform[] pathPoints)
    {
        path = pathPoints;
        transform.position = path[0].position;
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        while(index < path.Length)
        {
            var target = path[index].position;

            while (Vector3.Distance(transform.position, target) > 0.05f)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    target,
                    speed * Time.deltaTime * SpeedGameManager.Instance.SpeedMultiplier);
                yield return null;
            }
            index++;
        }
        Destroy(gameObject);
    }
}
