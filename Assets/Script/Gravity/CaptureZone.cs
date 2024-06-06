using DG.Tweening;
using UnityEngine;

public class CaptureZone : MonoBehaviour
{
    public Character owner;
    public float timer;
    public bool onZone;
    Character ortherCharacter;
    public float limitedRadius = 0.5f;
    private bool CanCaptureZone = true;


    private void Start()
    {
        ortherCharacter = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Character character = Cache.GetCharacterCollider(collision);

        if (CanCaptureZone)
        {
            if (character != null)
            {
                ortherCharacter = character;
            }
            if (ortherCharacter != null && owner != null && (owner.generalityType == ortherCharacter.generalityType + 1) && ortherCharacter.host == null && !ortherCharacter.isPlayer)
            {
                onZone = true;
                timer = 0f;

            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Character character = Cache.GetCharacterCollider(collision);
        if (ortherCharacter == character)
        {
            onZone = false;
            ortherCharacter = null;

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
            if (ortherCharacter != null && ortherCharacter.host == null)
            {
                BecomeSatellite(ortherCharacter);
                owner.satellites.Add(ortherCharacter);
                timer = 0f;
                onZone = false;
                ortherCharacter = null;
            }
        }

        if (owner.generalityType == GeneralityType.Star)
        {
            if (owner.NunmberOrbit < owner.MaxOrbit)
                CanCaptureZone = true;
            else
                CanCaptureZone = false;
        }

        if (owner.generalityType == GeneralityType.BlackHole)
        {

            CanCaptureZone = false;
        }
    }

    public void BecomeSatellite(Character character)
    {
        character.host = owner;
        SetSatellite(character);
    }

    public void SetSatellite(Character character)
    {
        float distance = (character.tf.position - owner.tf.position).magnitude;
        character.radius = distance;
        character.isCapture = true;
        DOTween.To(() => character.radius, x => character.radius = x, SetRadius(character), 0.3f).Play();
        character.spinSpeed = RamdomSpinSpeed(Random.Range(0.5f, 1.5f));
        character.angle = Mathf.Atan2(character.tf.position.y - owner.tf.position.y, character.tf.position.x - owner.tf.position.x);
    }

    public float SetRadius(Character character)
    {
        if (owner.generalityType == GeneralityType.Planet)
        {
            limitedRadius = GameManager.instance.status.coefficientRadiusPlanet * owner.circleCollider2D.radius * owner.tf.localScale.x;

        }
        else if (owner.generalityType == GeneralityType.Star)
        {
            limitedRadius = GameManager.instance.status.coefficientRadiusStar * owner.circleCollider2D.radius * owner.tf.localScale.x;

        }
        float radius = limitedRadius + owner.satellites.Count * (character.circleCollider2D.radius * character.tf.localScale.x * GameManager.instance.status.coefficientDistanceCharacter);

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

}
