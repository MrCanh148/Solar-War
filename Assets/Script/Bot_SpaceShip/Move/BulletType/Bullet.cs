using UnityEngine;
using UnityEngine.TextCore.Text;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float TimeLimit = 2f;
    private float timeAppear = 0f;
    public int damage;
    [HideInInspector] public Character characterOwner;

    private void Start()
    {
        AudioManager.instance.PlaySFX("Pistol");
    }

    private void Update()
    {
        timeAppear += Time.deltaTime;
        if (timeAppear > TimeLimit)
            Destroy(gameObject);

        transform.Translate(Vector2.up * speed * Time.deltaTime);


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Character target = collision.gameObject.GetComponent<Character>();
        Rigidbody2D rbTarget = collision.gameObject.GetComponent<Rigidbody2D>();
        Shield shieldTarget = collision.gameObject.GetComponent<Shield>();

        if (target == null) return;

        if (target.generalityType == GeneralityType.Asteroid || target.generalityType == GeneralityType.Planet)
        {
            if (target != characterOwner)
            {
                if (target.characterType == CharacterType.LifePlanet)
                {
                    if (shieldTarget.ShieldPlanet > 0)
                    {
                        shieldTarget.ShieldPlanet -= damage;
                        shieldTarget.TakeDamage = true;
                    }
                    else
                        rbTarget.mass -= damage;
                }
                else
                {
                    rbTarget.mass -= damage;
                    if (rbTarget.mass < 1 || (target.characterType == CharacterType.SmallPlanet && rbTarget.mass < 20)
                                          || (target.characterType == CharacterType.SmallStar && rbTarget.mass < 180))
                    {
                        if (collision.gameObject.tag == "Player")
                            ReSpawnPlayer.Instance.ResPlayer();
                        else
                        {
                            if (target.host != null)
                                target.host.satellites.Remove(target);
                            SpawnPlanets.instance.ActiveCharacter2(target);
                        }
                    }
                }
            }

            Destroy(gameObject);
        }
    }
}
