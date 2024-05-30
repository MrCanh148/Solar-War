using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed = 200f; // Tốc độ quay của tên lửa để điều chỉnh hướng
    [SerializeField] private float TimeLimit = 3f;
    [SerializeField] private int damage;
    private float timeAppear = 0f;
    private GameObject target;

    private void Update()
    {
        timeAppear += Time.deltaTime;
        if (timeAppear > TimeLimit)
            Destroy(gameObject);

        MoveTowardsTarget();
    }

    private void MoveTowardsTarget()
    {
        if (target != null)
        {
            Vector2 direction = (target.transform.position - transform.position).normalized;

            // Điều chỉnh hướng bay của tên lửa dựa trên hướng đến target
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Di chuyển tên lửa theo hướng đã điều chỉnh
            transform.Translate(Vector2.up * speed * Time.deltaTime, Space.Self);
        }
        else
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
    }

    public void SetTarget(GameObject newTarget)
    {
        target = newTarget;
    }
}
