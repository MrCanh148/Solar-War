using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float TimeLimit = 2f;
    [SerializeField] private int damage;
    private float timeAppear = 0f;


    private void Update()
    {
        timeAppear += Time.deltaTime;
        if (timeAppear > TimeLimit)
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
            {
                if (character.host != null)
                {
                    character.host.satellites.Remove(character);
                }
                Destroy(collision.gameObject);
            }
            Destroy(gameObject);
        }

        // neu cham object khac owner thi character chu the tang 
    }
}
