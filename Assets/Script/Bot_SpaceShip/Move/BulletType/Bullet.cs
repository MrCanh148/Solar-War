using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float TimeLimit = 2f;
    private float timeAppear = 0f;
    public int damage;
    [HideInInspector] public Character characterOwner;

    private void Update()
    {
        timeAppear += Time.deltaTime;
        if (timeAppear > TimeLimit)
            Destroy(gameObject);

        transform.Translate(Vector2.up * speed * Time.deltaTime);


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Character target = collision.gameObject.GetComponent<Character>();
        Rigidbody2D rbTarget = collision.gameObject.GetComponent<Rigidbody2D>();

        if (target == null) return;

        if (target.generalityType == GeneralityType.Asteroid || target.generalityType == GeneralityType.Planet)
        {
            if (target != characterOwner)
            {
                rbTarget.mass -= damage;
                target.killer = characterOwner;
                //if (!target.gameObject.activeSelf)
                //{
                //    //characterOwner.Kill++;

                //    if (collision.gameObject.tag == "Player")
                //        ReSpawnPlayer.Instance.ResPlayer();

                //    else
                //    {
                //        if (target.host != null)
                //        {
                //            target.host.satellites.Remove(target);
                //        }
                //        Destroy(collision.gameObject);
                //    }
                //}
            }

            Destroy(gameObject);
        }
    }
}
