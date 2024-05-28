using UnityEngine;

public class ObstacleAvoidance : MonoBehaviour
{
    private RandomMovement parentRandomMovement;

    void Start()
    {
        parentRandomMovement = GetComponentInParent<RandomMovement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Character character = collision.GetComponent<Character>();
        if (character != null && parentRandomMovement != null)
        {
            parentRandomMovement.AvoidObstacle();
        }
    }
}
