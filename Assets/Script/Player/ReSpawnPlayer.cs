using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
        if (character == null)
        {
            Debug.LogError("Character component not found on the GameObject.");
        }
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

        newPos.x = Random.Range(-distanceTele, distanceTele) + currentPos.x;
        newPos.y = Random.Range(-distanceTele, distanceTele) + currentPos.y;

        transform.position = newPos;
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
