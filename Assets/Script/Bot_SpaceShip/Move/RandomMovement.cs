using System.Collections.Generic;
using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f; // Tốc độ di chuyển của bot
    [SerializeField] public float wanderRadius = 5f; // Bán kính di chuyển ngẫu nhiên xung quanh điểm trung tâm
    [SerializeField] private float changeDirectionInterval = 2f; // Thời gian để thay đổi hướng di chuyển
    [SerializeField] private float rotationSpeed = 50f; // Tốc độ quay của đối tượng

    [HideInInspector] public Transform centerPoint;
    [HideInInspector] public Vector2 targetPosition;
    [HideInInspector] public Vector2 currentDirection;
    private Rigidbody2D rb;
    private float changeDirectionTimer;

    //private float timeNotDie = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SetRandomTargetPosition();
        currentDirection = (targetPosition - (Vector2)transform.position).normalized;
    }

    void Update()
    {
        changeDirectionTimer -= Time.deltaTime;

        if (changeDirectionTimer <= 0)
        {
            SetRandomTargetPosition();
            changeDirectionTimer = changeDirectionInterval;
        }

        MoveTowardsTarget();
        //timeNotDie += Time.deltaTime;
    }

    public void SetCenterPoint(Transform newCenterPoint)
    {
        centerPoint = newCenterPoint;
    }

    void SetRandomTargetPosition()
    {
        if (centerPoint == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector2 randomDirection = Random.insideUnitCircle * wanderRadius;
        targetPosition = (Vector2)centerPoint.position + randomDirection;
    }

    public void MoveTowardsTarget()
    {
        Vector2 targetDirection = (targetPosition - (Vector2)transform.position).normalized;
        currentDirection = Vector2.Lerp(currentDirection, targetDirection, Time.deltaTime * moveSpeed);
        rb.velocity = currentDirection * moveSpeed;

        float angle = Mathf.Atan2(currentDirection.y, currentDirection.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        if (Vector2.Distance(transform.position, targetPosition) < 0.5f)
        {
            SetRandomTargetPosition();
        }
    }

    public void AvoidObstacle()
    {
        float ZPos = Random.Range(20, 360);
        currentDirection = Quaternion.Euler(0, 0, ZPos) * currentDirection;
        rb.velocity = currentDirection * moveSpeed;

        float angle = Mathf.Atan2(currentDirection.y, currentDirection.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

}
