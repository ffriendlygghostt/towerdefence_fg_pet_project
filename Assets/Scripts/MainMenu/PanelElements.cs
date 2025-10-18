using UnityEngine;

public class PanelElementsController : MonoBehaviour
{
    [Header("Elements")]
    public Transform[] elements; 
    public Vector2[] directions; 

    [Header("Settings")]
    public float idleRadius = 0.2f;      
    public float idleSpeed = 1.15f;     
    public float hoverRadius = -0.1f;     
    public float hoverSpeed = 4f;       
    public float returnSpeed = 4f; 

    private Vector3[] startPositions;
    private bool isHover = false;
    private float[] progress;
    private int[] directionSign;

    private Vector3[] returnPositions;

    void Start()
    {
        startPositions = new Vector3[elements.Length];
        progress = new float[elements.Length];
        directionSign = new int[elements.Length];
        returnPositions = new Vector3[elements.Length];

        for (int i = 0; i < elements.Length; i++)
        {
            startPositions[i] = elements[i].localPosition;
            progress[i] = 0f;
            directionSign[i] = 1;

            if (directions.Length <= i)
                directions = new Vector2[elements.Length];
            directions[i] = directions[i].normalized;
        }
    }

    void Update()
    {
        for (int i = 0; i < elements.Length; i++)
        {
            Vector3 targetPos;

            if (isHover)
            {
                targetPos = startPositions[i] + new Vector3(directions[i].x, directions[i].y, 0f) * hoverRadius;
                elements[i].localPosition = Vector3.Lerp(elements[i].localPosition, targetPos, Time.deltaTime * hoverSpeed);
            }
            else
            {
                if ((elements[i].localPosition - (startPositions[i] + new Vector3(directions[i].x, directions[i].y, 0f) * idleRadius * progress[i])).magnitude > 0.01f)
                {
                    targetPos = startPositions[i] + new Vector3(directions[i].x, directions[i].y, 0f) * idleRadius * progress[i];
                    elements[i].localPosition = Vector3.Lerp(elements[i].localPosition, targetPos, Time.deltaTime * returnSpeed);
                }
                else
                {
                    progress[i] += directionSign[i] * idleSpeed * Time.deltaTime;

                    if (progress[i] >= 1f)
                    {
                        progress[i] = 1f;
                        directionSign[i] = -1;
                    }
                    else if (progress[i] <= 0f)
                    {
                        progress[i] = 0f;
                        directionSign[i] = 1;
                    }

                    targetPos = startPositions[i] + new Vector3(directions[i].x, directions[i].y, 0f) * idleRadius * progress[i];
                    elements[i].localPosition = targetPos;
                }
            }
        }
    }

    public void OnHoverEnter()
    {
        isHover = true;
    }

    public void OnHoverExit()
    {
        isHover = false;
    }
}
