using System.Collections;
using UnityEngine;

public class ReSpawnPlayer : MonoBehaviour
{
    public static ReSpawnPlayer Instance;

    [SerializeField] private float distanceTele;

    private Character character;
    private Player player;
    private Vector2 currentPos;
    private bool resetVelocity = false;

    private void Start()
    {

        Instance = this;
        character = GetComponent<Character>();
        player = GetComponent<Player>();
    }

    private void Update()
    {
        if (resetVelocity)
        {
            player.ResetVelocity();
            resetVelocity = false;
        }
     
    }

    private IEnumerator TeleNewPos()
    {
        yield return new WaitForSeconds(2f);
        RespawnPlace();
        character.spriteRenderer.enabled = true;
        character.canControl = true;
        resetVelocity = true;
    }

    private void RespawnPlace()
    {
        Vector2 newPos = new Vector2(0, 0);

        newPos.x = (Random.value > 0.5f ? distanceTele : -distanceTele) + currentPos.x;
        newPos.y = (Random.value > 0.5f ? distanceTele : -distanceTele) + currentPos.y;

        transform.position = newPos;
        Debug.Log(transform.position);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Character character1 = collision.gameObject.GetComponent<Character>();

        if (character == null || character1 == null)
        {
            return;
        }

        if (character1.generalityType > character.generalityType)
        {
            ResPlayer();
        }
    }

    public void ResPlayer()
    {     
        character.spriteRenderer.enabled = false;
        character.canControl = false;
        currentPos = transform.position;
        StartCoroutine(TeleNewPos());
    }
}
