using UnityEngine;

public enum CharacterType
{
    Asteroid,
    Planet,
    Star,
    BlackHole,
    SmallPlanet,
    LifePlanet,
    GasGiantPlanet,
    SmallStar,
    MediumStar,
    BigStar,
    NeutronStar
}

public class Character : MonoBehaviour
{
    public CharacterType characterType;
    public Rigidbody2D rb;
    public Character host;
    public Transform tf;
    public float radius; // Bán kính quỹ đạo
    public float spinSpeed; // Tốc độ quay
    public float angle;
    public bool isCapture;
    public float captureZoneRadius;
    [SerializeField] public LineRenderer lineRenderer;
    [SerializeField] protected LayerMask characterLayer;
    public Vector2 velocity;
    public Vector2 externalVelocity;
    public Vector2 mainVelocity;

    protected virtual void Start()
    {
        OnInit();
    }

    protected virtual void OnInit()
    {
        radius = 5f;
        spinSpeed = 1f;
        angle = 0f;
        isCapture = false;
    }

    private void Update()
    {
        if (isCapture)
        {
            // Tính toán vị trí mới dựa trên góc quay và bán kính
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;

            // Cập nhật vị trí của đối tượng
            tf.position = host.tf.position + new Vector3(x, y, 0f);
            //transform.RotateAround(host.tf.position, Vector3.forward, angle);

            // Tăng góc quay theo tốc độ
            angle += spinSpeed * Time.deltaTime;
            if (host != null && tf != null)
            {
                lineRenderer.SetPosition(1, tf.position);
                lineRenderer.SetPosition(0, host.tf.position);
            }
        }
            
    }

    protected virtual void FixedUpdate()
    {
        velocity.x -= velocity.x * GameManager.instance.status.deceleration * Time.fixedDeltaTime;
        velocity.y -= velocity.y * GameManager.instance.status.deceleration * Time.fixedDeltaTime;
        velocity = new Vector2(velocity.x, velocity.y);
        mainVelocity = velocity + externalVelocity;
        rb.velocity = mainVelocity;

    }

    public void Attract(Character character)
    {
        Rigidbody2D rigidbody = character.rb;
        Vector3 direction = tf.position - character.tf.position;
        float distance = direction.magnitude;
        if (distance <= 0.01f)
            return;

        float forceMagnitude = (GameManager.instance.status.GravitationalConstant * rigidbody.mass * rb.mass) / Mathf.Pow(distance, 2);
        Vector2 force = direction.normalized * forceMagnitude;
        rigidbody.AddForce(force);
    }

    //=================================== VA CHAM DAN HOI ============================================ 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Character character = collision.gameObject.GetComponent<Character>();
        if (character.characterType == CharacterType.Asteroid)
        {
            if (characterType == CharacterType.Asteroid)
            {
                //Debug.Log(name + ": " + mainVelocity.magnitude + " " + character.name + ": " + character.mainVelocity.magnitude);
                if (this.GetInstanceID() > character.GetInstanceID())
                {
                    HandleCollision(this, character);
                }
            }
        }
    }

    public void HandleCollision(Character c1, Character c2)
    {

        float gravitational = (c1.velocity * c1.rb.mass - c2.velocity * c2.rb.mass).magnitude;
        if (gravitational <= GameManager.instance.status.minimumMergeForce)
        {
            Vector2 velocityC1 = (2 * c2.rb.mass * c2.mainVelocity + (c1.rb.mass - c2.rb.mass) * c1.mainVelocity) / (c1.rb.mass + c2.rb.mass);

            Vector2 velocityC2 = (2 * c1.rb.mass * c1.mainVelocity + (c2.rb.mass - c1.rb.mass) * c2.mainVelocity) / (c1.rb.mass + c2.rb.mass);

            c1.velocity = new Vector2(velocityC1.x, velocityC1.y);
            c1.ResetExternalVelocity();

            c2.velocity = new Vector2(velocityC2.x, velocityC2.y);
            c2.ResetExternalVelocity();
        }
        else
        {
            MergeCharacter(c1, c2);
            Vector2 velocityS = (c2.rb.mass * c2.velocity + c1.rb.mass * c1.velocity) / (c1.rb.mass + c2.rb.mass);
            c1.velocity = new Vector2(velocityS.x, velocityS.y);
        }
    }

    public void MergeCharacter(Character c1, Character c2)
    {
        c1.rb.mass++;
        c2.gameObject.SetActive(false);
    }

    protected virtual void ResetExternalVelocity()
    {
        externalVelocity = Vector2.zero;
    }
}