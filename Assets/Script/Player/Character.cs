using System.Collections;
using UnityEngine;

public enum CharacterType
{
    Asteroid = 0,
    SmallPlanet = 1,
    LifePlanet = 2,
    GasGiantPlanet = 3,
    SmallStar = 4,
    MediumStar = 5,
    BigStar = 6,
    NeutronStar = 7,
    BlackHole = 8,
    BigCrunch = 9,
    BigBang = 10,

};


public enum GeneralityType
{
    Asteroid = 0,
    Planet = 1,
    Star = 3,
    BlackHole = 4,
};

public class Character : MonoBehaviour
{
    public CharacterType characterType;
    public GeneralityType generalityType;
    public Rigidbody2D rb;
    public Transform tf;
    [SerializeField] protected LayerMask characterLayer;
    public Vector2 velocity;
    public Vector2 externalVelocity;
    public Vector2 mainVelocity;
    public bool isPlayer;
    public bool canControl;
    private SpriteRenderer spriteRenderer;
    [SerializeField] GameObject canvar;

    protected virtual void Start()
    {
        OnInit();
        spriteRenderer = GetComponent<SpriteRenderer>();
        EvolutionCharacter();
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

        tf.Rotate(Vector3.forward, 100 * Time.deltaTime);

        if (canvar != null)
            canvar.transform.rotation = Quaternion.identity;
    }

    //=================================== VA CHAM DAN HOI ============================================ 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Character character = collision.gameObject.GetComponent<Character>();
        if (character.characterType == CharacterType.Asteroid)
        {
            if (characterType == CharacterType.Asteroid)
            {
                if (isPlayer)
                {
                    HandleCollision(this, character);
                }
                else if (this.GetInstanceID() > character.GetInstanceID())
                {
                    HandleCollision(this, character);
                }
            }
        }

        /*if (collision.gameObject.tag != "Player")
        {
            spriteRenderer.enabled = false;
            canControl = false;
            StartCoroutine(TeleNewPos());
        }*/
    }

    public void HandleCollision(Character c1, Character c2)
    {

        float gravitational = (c1.mainVelocity * c1.rb.mass - c2.mainVelocity * c2.rb.mass).magnitude;
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
        SpawnPlanets.instance.ActiveCharacter(c2);
        c1.EvolutionCharacter();
        if (c1.isPlayer)
        {
            ShowUI.instance.UpdateInfo();
        }
    }

    protected virtual void ResetExternalVelocity()
    {
        externalVelocity = Vector2.zero;
    }

    public void EvolutionCharacter()
    {
        if (characterType == CharacterType.SmallPlanet)
        {
            generalityType = GeneralityType.Asteroid;
        }
        else if (characterType == CharacterType.SmallPlanet || characterType == CharacterType.LifePlanet || characterType == CharacterType.GasGiantPlanet)
        {
            generalityType = GeneralityType.Planet;
        }
        else if (characterType == CharacterType.SmallStar || characterType == CharacterType.MediumStar || characterType == CharacterType.NeutronStar)
        {
            generalityType = GeneralityType.Star;
        }
        else if (characterType == CharacterType.BlackHole || characterType == CharacterType.BigCrunch || characterType == CharacterType.BigBang)
        {
            generalityType = GeneralityType.BlackHole;
        }
    }


    private IEnumerator TeleNewPos()
    {
        yield return new WaitForSeconds(2f);
        RespawnPlace();
        spriteRenderer.enabled = true;
        canControl = true;
    }
    private void RespawnPlace()
    {
        Vector2 newPos = new Vector2(0, 0);

        newPos.x = Random.Range(-200f, 200f);
        newPos.y = Random.Range(-200f, 200f);

        transform.position = newPos;
    }
}