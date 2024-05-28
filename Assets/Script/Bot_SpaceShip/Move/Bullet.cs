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
        Character character = collision.gameObject.GetComponent<Character>();
        if (character == null) return;

        if (character.generalityType == GeneralityType.Asteroid || character.generalityType == GeneralityType.Planet)
        {
            if (collision.gameObject.tag == "Player")
                ReSpawnPlayer.Instance.ResPlayer();
            else
                Destroy(collision.gameObject);

            Destroy(gameObject);
        }
    }
}
