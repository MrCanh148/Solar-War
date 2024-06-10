using UnityEngine;

public class BulletTurret : MonoBehaviour
{
    public Transform target;
    public Transform tf;
    public float speed = 5f;
    public Turret source;
    float timeAttack = 1f;
    public Character owner;
    public float damage = 5f;

    private void Start()
    {
        OnInit();
    }

    public void OnInit()
    {
        timeAttack = 0;
    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 direction = (target.transform.position - tf.position).normalized;
            tf.up = direction;
            //tf.position = Vector3.Lerp(tf.position, target.position, Time.deltaTime * speed);
            //tf.position -= speed * Time.fixedDeltaTime * tf.up;

            timeAttack += Time.fixedDeltaTime;
            if (timeAttack > 1)
            {
                OnDespawn();
            }


            tf.Translate(Vector2.up * speed * Time.fixedDeltaTime);
        }
        else
            OnDespawn();
    }

    public void OnDespawn()
    {
        if (source != null)
        {
            gameObject.SetActive(false);
            source.bullets.Add(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*Test2 test = collision.GetComponent<Test2>();
        if (test != null)
        {
            //test.OnHit();
            gameObject.SetActive(false);
            source.bullets.Add(this);
        }*/


        ShootTarget shootTarget = collision.GetComponent<ShootTarget>();
        if (shootTarget != null)
        {
            //test.OnHit();
            if (shootTarget.hostAlien != null && shootTarget.hostAlien.myFamily != owner.myFamily)
            {
                shootTarget.heart -= damage;
                if (shootTarget.heart <= 0)
                {
                    Destroy(shootTarget.gameObject);
                    owner.Kill++;
                }

                gameObject.SetActive(false);
                source.bullets.Add(this);
            }
        }
    }
}
