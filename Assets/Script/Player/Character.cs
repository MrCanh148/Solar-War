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
    public Transform tf;
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

    }

    protected virtual void FixedUpdate()
    {
        velocity.x -= velocity.x * GameManager.instance.status.deceleration * Time.fixedDeltaTime;
        velocity.y -= velocity.y * GameManager.instance.status.deceleration * Time.fixedDeltaTime;
        velocity = new Vector2(velocity.x, velocity.y);
        mainVelocity = velocity + externalVelocity;
        rb.velocity = mainVelocity;

    }

    //=================================== VA CHAM DAN HOI ============================================ 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Character character = collision.gameObject.GetComponent<Character>();
        if (character.characterType == CharacterType.Asteroid)
        {
            if (characterType == CharacterType.Asteroid)
            {
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