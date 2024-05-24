using DG.Tweening;
using System.Collections;
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
    public SpriteRenderer spriteRenderer;
    [SerializeField] GameObject canvar;
    [SerializeField] private float distanceTele;
    private Vector2 currentPos;
    public List<Character> satellites;

    //Orbit
    public Character host;
    public float radius; // Bán kính quỹ đạo
    public float spinSpeed; // Tốc độ quay
    public float angle;
    public bool isCapture;
    public LineRenderer lineRenderer;



    protected virtual void Start()
    {
        OnInit();
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    protected virtual void OnInit()
    {


    }

    private void Update()
    {
        if (isCapture)
        {
            Debug.Log("test");
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

        if (characterType == CharacterType.Asteroid)
            tf.Rotate(Vector3.forward, 100 * Time.deltaTime);

        if (canvar != null)
            canvar.transform.rotation = Quaternion.identity;
    }

    //=================================== VA CHAM DAN HOI ============================================ 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Character character = collision.gameObject.GetComponent<Character>();

        if (character == null)
            return;

        if (character.generalityType == this.generalityType)
        {
            if (isPlayer)
            {
                HandleCollision(this, character);
            }
            else if (this.GetInstanceID() > character.GetInstanceID())
            {
                HandleCollision(this, character);
                Debug.Log("Va cham");
            }


        }


        // Nếu 2 plant Khác hệ thì destroy cái bé
        if (character.generalityType > this.generalityType && isPlayer)
        {
            if (isPlayer) //Nếu plant là Player
            {
                spriteRenderer.enabled = false;
                canControl = false;
                currentPos = transform.position;
                StartCoroutine(TeleNewPos());
            }
            else
            {
                Destroy(gameObject);
            }
        }

    }

    public void HandleCollision(Character c1, Character c2)
    {

        float gravitational = (c1.mainVelocity - c2.mainVelocity).magnitude;
        Debug.Log("gravitational = " + gravitational);
        if (c1.generalityType == GeneralityType.Asteroid)
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
        else
        {
            c1.rb.mass -= (int)c2.rb.mass / 10;
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
                c1.gameObject.SetActive(false);
            }

            if (c2.rb.mass > SpawnPlanets.instance.GetRequiredMass(c2.characterType))
            {
                c2.velocity = new Vector2(velocityC2.x, velocityC2.y);
                c2.ResetExternalVelocity();
            }
            else
            {
                c2.gameObject.SetActive(false);
            }
        }


    }

    public void MergeCharacter(Character c1, Character c2)
    {
        c1.rb.mass++;
        c2.gameObject.SetActive(false);
        SpawnPlanets.instance.ActiveCharacter(c2);
    }

    protected virtual void ResetExternalVelocity()
    {
        externalVelocity = Vector2.zero;
    }




    // Sử lý Player khi bị destroy
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

        newPos.x = Random.Range(-distanceTele, distanceTele) + currentPos.x;
        newPos.y = Random.Range(-distanceTele, distanceTele) + currentPos.y;

        transform.position = newPos;
    }

    public void AbsorbCharacter(Character host, Character character)
    {
        //character.tf.position = Vector3.Lerp(character.tf.position, tf.position, 0.8f);
        /*character.tf.DOScale(Vector3.zero, 0.5f)
                     .OnComplete(() =>
                     {
                         // Vô hiệu hóa vật B
                         character.tf.gameObject.SetActive(false);
                     });*/
        character.tf.DOMove(host.tf.position, 1f);
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
}