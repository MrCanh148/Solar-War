using UnityEngine;

public abstract class BotAirSpace : MonoBehaviour
{
    [SerializeField] protected float moveSpeed = 3f; // Tốc độ di chuyển của bot
    [SerializeField] protected float rotationSpeed = 50f; // Tốc độ quay của đối tượng

    public enum AirSpaceType
    {
        Fighter = 0,
        Cruiser = 1,
        MissileBoat = 2,
    };


    protected Vector2 targetPosition;
    protected Rigidbody2D rb;
    protected Vector2 currentDirection;
   

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        InitializeBot();
    }

    protected abstract void InitializeBot();
    protected abstract void Move();

 
}
