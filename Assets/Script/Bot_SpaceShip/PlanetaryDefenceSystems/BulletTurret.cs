using UnityEngine;

public class BulletTurret : MonoBehaviour
{
    public Transform target;
    public Transform tf;
    public float speed;
    public Turret source;


    private void Update()
    {
        if (target != null)
        {
            Vector3 direction = (target.transform.position - tf.position).normalized;
            tf.up = direction;
            tf.position = Vector3.Lerp(tf.position, target.position, Time.deltaTime * speed);



        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Test2 test = collision.GetComponent<Test2>();
        if (test != null)
        {
            test.OnHit();
            gameObject.SetActive(false);
            source.bullets.Add(this);
        }
    }
}
