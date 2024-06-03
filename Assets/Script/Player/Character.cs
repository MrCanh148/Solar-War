using DG.Tweening;
using System.Collections.Generic;
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
    Star = 2,
    BlackHole = 3,
};

public class Character : MonoBehaviour
{
    public CharacterType characterType;
    public GeneralityType generalityType;
    public Rigidbody2D rb;
    public Transform tf;
    public Vector2 velocity;
    public Vector2 externalVelocity;
    public Vector2 mainVelocity;
    public bool isPlayer;
    public bool canControl;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [SerializeField] GameObject canvar;
    public List<Character> satellites;
    public CircleCollider2D circleCollider2D;

    //Orbit
    public Character host;
    public float radius; // Bán kính quỹ đạo
    public float spinSpeed; // Tốc độ quay
    public float angle;
    public bool isCapture;
    public LineRenderer lineRenderer;

    public int Kill;
    public float Shield;
    [SerializeField] private GameObject ShieldObject;
    public float MaxShield = 20;
    private float TimeResShield = 0f;
    public bool EvolutionDone = false;
    public bool IsKill = false;

    public Character killer;

    protected virtual void Start()
    {
        OnInit();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (ShieldObject == null) return;
    }

    protected virtual void OnInit()
    {
        //lineRenderer.enabled = false;
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
            //tf.position = Vector3.Lerp(tf.position, host.tf.position + new Vector3(x, y, 0f), 0.5f);

            //transform.RotateAround(host.tf.position, Vector3.forward, angle);

            // Tăng góc quay theo tốc độ
            angle += spinSpeed * Time.deltaTime;
            //lineRenderer.enabled = true;
        }

        if (host != null && tf != null && lineRenderer.enabled == true)
        {
            lineRenderer.SetPosition(1, tf.position);
            lineRenderer.SetPosition(0, host.tf.position);
        }
    }



    protected virtual void FixedUpdate()
    {
        velocity.x -= velocity.x * GameManager.instance.status.deceleration * Time.fixedDeltaTime;
        velocity.y -= velocity.y * GameManager.instance.status.deceleration * Time.fixedDeltaTime;
        velocity = new Vector2(velocity.x, velocity.y);
        mainVelocity = velocity + externalVelocity;
        if (this.host == null)
        {
            rb.velocity = mainVelocity;
        }
        else
        {
            rb.velocity = Vector2.zero;
            Vector2 direction = host.tf.position - tf.position;

            Vector2 tmp;
            if (spinSpeed >= 0f)
            {

                if (tf.position.y >= host.tf.position.y)
                {
                    tmp = new Vector2(-spinSpeed, 0);
                }
                else
                {
                    tmp = new Vector2(spinSpeed, 0);
                }
            }
            else
            {
                if (tf.position.y >= host.tf.position.y)
                {
                    tmp = new Vector2(spinSpeed, 0);
                }
                else
                {
                    tmp = new Vector2(-spinSpeed, 0);
                }
            }
            Vector2 dirVeloc = CalculateProjection(tmp, direction);

            velocity = dirVeloc.normalized * spinSpeed;
            //Debug.Log(velocity);
        }


        if (characterType == CharacterType.Asteroid)
            tf.Rotate(Vector3.forward, 100 * Time.deltaTime);
        else
            tf.rotation = Quaternion.identity;

        if (canvar != null)
            canvar.transform.rotation = Quaternion.identity;

        if (EvolutionDone)
        {
            if (Shield < MaxShield)
                Shield += Time.deltaTime;
        }

        if (characterType != CharacterType.LifePlanet)
            Shield = 5;
          
        if (Shield > 0 && characterType == CharacterType.LifePlanet && EvolutionDone)
            ShieldObject.SetActive(true);
        else if (Shield <= 0 || characterType != CharacterType.LifePlanet)
            ShieldObject.SetActive(false);

        if (Shield <= 0 && characterType == CharacterType.LifePlanet)
        {
            TimeResShield += Time.deltaTime;
            if (TimeResShield > 5f)
            {
                EvolutionDone = true;
                TimeResShield = 0;
            }
            else
                EvolutionDone = false;
        }
         
    }

    //=================================== VA CHAM DAN HOI ============================================ 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Character character = collision.gameObject.GetComponent<Character>();
        if (character == null)
            return;
        /*if (isPlayer)
        {
            HandleCollision(this, character);
        }
        else if (this.GetInstanceID() > character.GetInstanceID())
        {
            HandleCollision(this, character);
            Debug.Log("Va cham");
        }*/
        HandleCollision2(character);
    }

    public void HandleCollision(Character c1, Character c2)
    {

        float gravitational = (c1.mainVelocity - c2.mainVelocity).magnitude;
        //Debug.Log("gravitational = " + gravitational);
        if (c1.generalityType == GeneralityType.Asteroid && c2.generalityType == GeneralityType.Asteroid)
        {
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

        if (c1.characterType != c2.characterType)
        {
            if (c2.characterType == CharacterType.Asteroid)
            {
                c1.rb.mass -= (int)c2.rb.mass;
            }
            else
            {
                c1.rb.mass -= (int)c2.rb.mass / 10;
            }

            Debug.Log((int)c2.rb.mass / 10);
            c2.rb.mass -= (int)c1.rb.mass / 10;
            Vector2 velocityC1 = (2 * c2.rb.mass * c2.mainVelocity + (c1.rb.mass - c2.rb.mass) * c1.mainVelocity) / (c1.rb.mass + c2.rb.mass);
            Vector2 velocityC2 = (2 * c1.rb.mass * c1.mainVelocity + (c2.rb.mass - c1.rb.mass) * c2.mainVelocity) / (c1.rb.mass + c2.rb.mass);
            if (c1.rb.mass > SpawnPlanets.instance.GetRequiredMass(c1.characterType))
            {
                c1.velocity = new Vector2(velocityC1.x, velocityC1.y);
                c1.ResetExternalVelocity();
            }
            else
            {
                if (!c1.isPlayer)
                    c1.gameObject.SetActive(false);
            }

            if (c2.rb.mass > SpawnPlanets.instance.GetRequiredMass(c2.characterType))
            {
                c2.velocity = new Vector2(velocityC2.x, velocityC2.y);
                c2.ResetExternalVelocity();
            }
            else
            {
                if (!c2.isPlayer)
                    c2.gameObject.SetActive(false);
            }
        }


    }

    public void HandleCollision2(Character character)
    {
        float gravitational = (mainVelocity - character.mainVelocity).magnitude;
        //Debug.Log("gravitational = " + gravitational);
        if (generalityType == GeneralityType.Asteroid && character.generalityType == GeneralityType.Asteroid)
        {
            if (gravitational <= GameManager.instance.status.minimumMergeForce)
            {
                Vector2 velocity = (2 * rb.mass * mainVelocity + (character.rb.mass - rb.mass) * character.mainVelocity) / (rb.mass + character.rb.mass);
                character.velocity = new Vector2(velocity.x, velocity.y);
                character.ResetExternalVelocity();
            }
            else
            {
                if (this.isPlayer)
                {
                    MergeCharacter(this, character);
                    Vector2 velocityS = (character.rb.mass * character.velocity + rb.mass * velocity) / (rb.mass + character.rb.mass);
                    velocity = new Vector2(velocityS.x, velocityS.y);
                }
                else if (this.GetInstanceID() > character.GetInstanceID())
                {
                    MergeCharacter(this, character);
                    Vector2 velocityS = (character.rb.mass * character.velocity + rb.mass * velocity) / (rb.mass + character.rb.mass);
                    velocity = new Vector2(velocityS.x, velocityS.y);
                }
            }
        }
        if (characterType != character.characterType)
        {
            if (characterType == CharacterType.Asteroid)
            {
                character.rb.mass -= (int)rb.mass;

            }
            else
            {
                if (generalityType > character.generalityType)
                {
                    if (!character.isPlayer)
                    {
                        character.gameObject.SetActive(false);
                        if (character.host != null)
                        {
                            character.host.satellites.Remove(character);
                        }
                        return;
                    }
                }
                else
                {
                    character.rb.mass -= SpawnPlanets.instance.GetRequiredMass(character.characterType + 1) / 10;
                }
            }
            Vector2 velocity = (2 * rb.mass * mainVelocity + (character.rb.mass - rb.mass) * character.mainVelocity) / (rb.mass + character.rb.mass);

            if (character.rb.mass >= SpawnPlanets.instance.GetRequiredMass(character.characterType))
            {

                character.velocity = new Vector2(velocity.x, velocity.y);
                character.ResetExternalVelocity();
            }
            else
            {

                if (!character.isPlayer)
                {
                    //SpawnPlanets.instance.ActiveCharacter(character);
                    character.gameObject.SetActive(false);
                    if (character.host != null)
                    {
                        character.host.satellites.Remove(character);
                    }
                }

            }
        }
    }

    public void MergeCharacter(Character c1, Character c2)
    {
        ChangeMass(c1, (int)c2.rb.mass);
        //c1.rb.mass += c2.rb.mass;
        c2.gameObject.SetActive(false);
        SpawnPlanets.instance.ActiveCharacter(c2);
    }

    public void ChangeMass(Character character, int number)
    {
        int tmpMass = (int)(character.rb.mass + number);
        float duration = Mathf.Abs(number) * 0.1f;
        DOTween.To(() => character.rb.mass, x => character.rb.mass = x, tmpMass, duration);
    }

    protected virtual void ResetExternalVelocity()
    {
        externalVelocity = Vector2.zero;
    }

    public void AbsorbCharacter(Character host, Character character)
    {

        DOTween.To(() => character.radius, x => character.radius = x, 1, 0.3f)

           .OnComplete(() =>
           {
               character.tf.gameObject.SetActive(false);
               host.satellites.Remove(character);
               character.lineRenderer.enabled = false;
               ResetRadiusSatellite(host);
           })
           .Play();
    }

    public Character GetCharacterWithMinimumMass()
    {
        Debug.Log(satellites.Count);
        Character character = null;
        if (satellites.Count > 0)
        {
            character = satellites[0];
            foreach (var c in satellites)
            {
                if (c.rb.mass < character.rb.mass)
                {
                    character = c;
                }
            }
            satellites.Remove(character);
        }
        return character;

    }



    private Vector2 CalculateProjection(Vector2 v1, Vector2 v2)
    {
        // Tính phép chiếu vector v1 lên v2
        Vector2 projV1OnV2 = Vector2.Dot(v1, v2.normalized) * v2.normalized;

        // Tính vector vuông góc với v2
        Vector2 orthogonalV2 = new Vector2(-v2.y, v2.x);

        // Tính phép chiếu vector v1 lên vector vuông góc với v2
        Vector2 projV1OnOrthogonalV2 = Vector2.Dot(v1, orthogonalV2.normalized) * orthogonalV2.normalized;

        return projV1OnOrthogonalV2;
    }

    public void ResetRadiusSatellite(Character owner)
    {
        for (int i = 0; i < owner.satellites.Count; i++)
        {
            Character character = owner.satellites[i];
            float tmpRadius = 0.5f + character.circleCollider2D.radius * 0.1f + i * (character.circleCollider2D.radius * 0.1f * 2 + 0.1f);
            DOTween.To(() => character.radius, x => character.radius = x, tmpRadius, 0.3f);

        }
    }
}