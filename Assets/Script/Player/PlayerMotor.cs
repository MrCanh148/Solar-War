using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    [SerializeField] float MaxSpeed;
    [SerializeField] float rotateSpeed;

    private Rigidbody2D rb;
    private float dirX, dirY;
    private float currentSpeed = 0;
    private float decreaseSpeed = 0;
    private bool isMoving = false;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        dirX = Input.GetAxis("Horizontal");
        dirY = Input.GetAxis("Vertical");

        if (dirX != 0 || dirY != 0)
            isMoving = true;
        else
            isMoving = false;

        if (isMoving)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, MaxSpeed, Time.deltaTime);
            decreaseSpeed = Mathf.MoveTowards(decreaseSpeed, MaxSpeed * 1.25f, Time.deltaTime);
        } 
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, Time.deltaTime);
        }
        

        if (rb.velocity == Vector2.zero)
            decreaseSpeed = 0;

        transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            Vector2 movementDirection = new Vector2(dirX, dirY).normalized;
            rb.AddForce(movementDirection * currentSpeed, ForceMode2D.Force);
        }
        else
            rb.velocity = Vector2.MoveTowards(rb.velocity, Vector2.zero, Time.deltaTime * decreaseSpeed);
    }
}
