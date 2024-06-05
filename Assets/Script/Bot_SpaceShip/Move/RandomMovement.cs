using UnityEngine;

public class RandomMovement : BotAirSpace
{
    [SerializeField] private float wanderRadius = 5f; // Bán kính di chuyển ngẫu nhiên xung quanh điểm trung tâm
    [SerializeField] private float changeDirectionInterval = 2f; // Thời gian để thay đổi hướng di chuyển
    public AirSpaceType type;
    [HideInInspector] public Transform centerPoint;
    private float changeDirectionTimer;

    protected override void InitializeBot()
    {
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

        Move();
    }

    void SetRandomTargetPosition()
    {
        if (centerPoint == null)
        {
            targetPosition = (Vector2)transform.position + Random.insideUnitCircle.normalized * wanderRadius;
        }
        else
        {
            Vector2 randomDirection = Random.insideUnitCircle * wanderRadius;
            targetPosition = (Vector2)centerPoint.position + randomDirection;
        }
    }

    protected override void Move()
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

    public void SetCenterPoint(Transform newCenterPoint)
    {
        centerPoint = newCenterPoint;
    }

    public void AvoidObstacle()
    {
        currentDirection = Quaternion.Euler(0, 0, 60) * currentDirection;
        rb.velocity = currentDirection * moveSpeed;

        float angle = Mathf.Atan2(currentDirection.y, currentDirection.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
}
