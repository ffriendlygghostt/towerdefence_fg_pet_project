using System;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public enum EnemyDirection
{
    Up, Down, Left, Right
}
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private EnemyController controller;
    [SerializeField] private EnemyDirection currentDirection = EnemyDirection.Left;
    public EnemyDirection GetCurrentDirection() => currentDirection;

    [Header("Path setup (for testing)")] 
    [SerializeField] public Transform[] pathZones;
    [SerializeField] private int currentZoneIndex = 0;

    private Vector3 targetPoint;
    private bool isMoving = false;


    private void Awake()
    {
        controller = GetComponent<EnemyController>();
        //TEMP: заглушка
        if (controller == null) controller = new EnemyController();
        if (controller == null) Debug.LogError("No Enemy controller!!!");
        controller.OnSpeedChanged += UpdateMoveSpeed;

        moveSpeed = controller.GetMoveSpeed();
    }

    private void Start()
    {
        if (pathZones.Length > 0)
        {
            SetNextTarget();
        }

        StartMovement();
    }

    private void Update()
    {
        if (!isMoving || pathZones?.Length == 0) return;
        MoveTowardsTarget();
    }

    private void MoveTowardsTarget()
    {
        Vector3 direction = (targetPoint -  transform.position).normalized;
        UpdateAnimations(direction);

        transform.position = Vector3.MoveTowards(transform.position,
            targetPoint, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPoint) < 0.05f)
        {
            currentZoneIndex++;
            if (currentZoneIndex >= pathZones.Length)
            {
                StopMovement();
                return;
            }
            SetNextTarget();
        }
    }

    private void SetNextTarget()
    {
        var zone = pathZones[currentZoneIndex];
        var collider = zone.GetComponent<Collider2D>();
        if (collider != null)
        {
            var bounds = collider.bounds;
            float x = Random.Range(bounds.min.x, bounds.max.x);
            float y = Random.Range(bounds.min.y, bounds.max.y);
            targetPoint = new Vector3(x, y, transform.position.z);
        }
        if (collider == null)
        {
            targetPoint = zone.position;
        }
    }

    public void SetPath(Transform[] path)
    {
        pathZones = path;
        currentZoneIndex = 0;
        if (path.Length > 0)
        {
            SetNextTarget();
        }
        isMoving = false;
    }
    private void UpdateAnimations(Vector3 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            currentDirection = direction.x > 0 ? EnemyDirection.Right : EnemyDirection.Left;
        }
        else
        {
            currentDirection = direction.y > 0 ? EnemyDirection.Up : EnemyDirection.Down;
        }

        controller.PlayWalk(currentDirection);
    }

    public void StopMovement()
    {
        isMoving = false;
    }

    public void StartMovement()
    {
        isMoving = true;
    }

    private void UpdateMoveSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }
}
