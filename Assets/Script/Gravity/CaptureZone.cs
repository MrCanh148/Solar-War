using UnityEngine;

public class CaptureZone : MonoBehaviour
{
    public Character owner;
    public float timer;
    public bool onZone;
    Character ortherCharacter;

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
        // Character character = collision.GetComponent<Character>();
        if (ortherCharacter != null && owner != null && (owner.generalityType == ortherCharacter.generalityType + 1) && ortherCharacter.host == null)
        {
            onZone = true;
            timer = 0f;
        }
        else
        {
            onZone = false;
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
            }
        }

    }

    public void BecomeSatellite(Character character)
    {
        character.host = owner;
        character.isCapture = true;
        SetSatellite(character);

        //Debug.Log(character.angle);
    }

    public void SetSatellite(Character character)
    {
        Vector2 direction = owner.tf.position - character.tf.position;
        Vector2 dirVeloc = CalculateProjection(character.mainVelocity, direction);

        if (direction.x <= 0 && direction.y <= 0)  // Goc I
        {

            if (dirVeloc.y > 0 || dirVeloc.x < 0)
            {
                character.spinSpeed = CalculateMagnitudeV1Perpendicular(character.mainVelocity, direction);
                Debug.Log(character.spinSpeed);
            }
            else if (dirVeloc.y < 0 || dirVeloc.x > 0)
            {
                character.spinSpeed = -CalculateMagnitudeV1Perpendicular(character.mainVelocity, direction);
                Debug.Log(character.spinSpeed);
            }
        }
        else if (direction.x >= 0 && direction.y <= 0)  // Goc II
        {

            if (dirVeloc.y < 0 || dirVeloc.x < 0)
            {
                character.spinSpeed = CalculateMagnitudeV1Perpendicular(character.mainVelocity, direction);
                Debug.Log(character.spinSpeed);
            }
            else if (dirVeloc.y > 0 || dirVeloc.x > 0)
            {
                character.spinSpeed = -CalculateMagnitudeV1Perpendicular(character.mainVelocity, direction);
                Debug.Log(character.spinSpeed);
            }
        }
        else if (direction.x >= 0 && direction.y >= 0)  // Goc III
        {

            if (dirVeloc.y < 0 || dirVeloc.x > 0)
            {
                character.spinSpeed = CalculateMagnitudeV1Perpendicular(character.mainVelocity, direction);
                Debug.Log(character.spinSpeed);
            }
            else if (dirVeloc.y > 0 || dirVeloc.x < 0)
            {
                character.spinSpeed = -CalculateMagnitudeV1Perpendicular(character.mainVelocity, direction);
                Debug.Log(character.spinSpeed);
            }
        }
        else if (direction.x <= 0 && direction.y >= 0)  // Goc IV
        {

            if (dirVeloc.y > 0 || dirVeloc.x > 0)
            {
                character.spinSpeed = CalculateMagnitudeV1Perpendicular(character.mainVelocity, direction);
                Debug.Log(character.spinSpeed);
            }
            else if (dirVeloc.y < 0 || dirVeloc.x < 0)
            {
                character.spinSpeed = -CalculateMagnitudeV1Perpendicular(character.mainVelocity, direction);
                Debug.Log(character.spinSpeed);
            }
        }

        //character.spinSpeed = CalculateMagnitudeV1Perpendicular(character.mainVelocity, direction);
        character.radius = direction.magnitude;
        character.spinSpeed = 1f;
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
}
