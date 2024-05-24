using System.Collections;
using UnityEngine;

public class CaptureZone : MonoBehaviour
{
    public Character owner;
    public float timer;
    public bool onZone;
    Character ortherCharacter;
    public float limitedRadius = 0.5f;


    private void Start()
    {
        ortherCharacter = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Character character = collision.GetComponent<Character>();
        if (character != null)
        {
            ortherCharacter = character;
        }
        Debug.Log(character);
        //Debug.Log("owner.generalityType = " + owner.generalityType + " ortherCharacter.generalityType" + ortherCharacter.generalityType);
        // Character character = collision.GetComponent<Character>();
        if (ortherCharacter != null)
        {
            Debug.Log("owner.generalityType = " + owner.generalityType + "| ortherCharacter.generalityType = " + ortherCharacter.generalityType);
        }


        if (ortherCharacter != null && owner != null && (owner.generalityType == ortherCharacter.generalityType + 1) && ortherCharacter.host == null)
        {
            onZone = true;
            timer = 0f;
            Debug.Log("Vao vong");
        }


    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Character character = collision.GetComponent<Character>();
        if (ortherCharacter == character)
        {
            onZone = false;
            ortherCharacter = null;
            Debug.Log("ra khoi vong");
        }
    }


    private void Update()
    {
        if (onZone)
        {
            timer += Time.deltaTime;
        }
        if (timer > GameManager.instance.status.timeToCapture)
        {
            if (ortherCharacter != null)
            {
                BecomeSatellite(ortherCharacter);
                owner.satellites.Add(ortherCharacter);
                timer = 0f;
                onZone = false;
                ortherCharacter = null;
            }
        }

    }

    public void BecomeSatellite(Character character)
    {
        character.host = owner;
        SetSatellite(character);
        character.MoveNewPosition();

    }

    public void SetSatellite(Character character)
    {
        Vector2 direction = owner.tf.position - character.tf.position;
        Vector2 dirVeloc = CalculateProjection(character.mainVelocity, direction);

        character.radius = SetRadius(character);
        character.spinSpeed = RamdomSpinSpeed(Random.Range(0.5f, 1.5f));
        //character.radius = 3f;
        character.angle = Mathf.Atan2(character.tf.position.y - owner.tf.position.y, character.tf.position.x - owner.tf.position.x);
        character.velocity = Vector2.zero;
    }

    private float CalculateMagnitudeV1Perpendicular(Vector2 v1, Vector2 v2)
    {
        // Tính phép chiếu vector v1 lên v2
        Vector2 projV1OnV2 = Vector2.Dot(v1, v2.normalized) * v2.normalized;
        // Tính vector vuông góc với v2
        Vector2 v1Perpendicular = v1 - projV1OnV2;
        // Tính độ lớn của vector v1Perpendicular
        float magnitudeV1Perpendicular = v1Perpendicular.magnitude;
        return magnitudeV1Perpendicular;
    }

    private Vector2 CalculateProjection(Vector2 v1, Vector2 v2)
    {
        // Tính phép chiếu vector v1 lên v2
        Vector2 projV1OnV2 = Vector2.Dot(v1, v2.normalized) * v2.normalized;

        if (projV1OnV2 == Vector2.zero)
        {
            return v1;
        }
        return projV1OnV2;
    }

    public float SetRadius(Character character)
    {
        float radius = limitedRadius + character.GetComponent<CircleCollider2D>().radius * 0.1f + owner.satellites.Count * (character.GetComponent<CircleCollider2D>().radius * 0.1f * 2 + 0.1f);
        Debug.Log(character.GetComponent<CircleCollider2D>().radius);
        Debug.Log(radius);
        return radius;
    }

    public float RamdomSpinSpeed(float n)
    {
        float number = n;
        int randomValue = Random.Range(0, 2);
        if (randomValue == 0)
        {
            number = -n;
        }
        else if (randomValue == 1)
        {
            number = n;
        }
        return number;
    }

    private IEnumerator WaitChangePosition()
    {
        yield return new WaitForSeconds(1f);
    }
}
