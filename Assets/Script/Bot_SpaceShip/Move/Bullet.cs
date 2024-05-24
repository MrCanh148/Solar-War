using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    private float timeAppear = 0f;

    private void Update()
    {
        timeAppear += Time.deltaTime;
        if (timeAppear > 2f)
            Destroy(gameObject);

        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }

    }

}
