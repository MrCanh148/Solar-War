using UnityEngine;

public class BulletTurret : MonoBehaviour
{
    public Transform target;
    public Transform tf;
    public float speed = 5f;
    public Turret source;
    float timeAttack = 1f;



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
        ShootTarget shootTarget = collision.GetComponent<ShootTarget>();
        if (shootTarget != null)
        {
            //test.OnHit();
            gameObject.SetActive(false);
            source.bullets.Add(this);
        }
    }
}
