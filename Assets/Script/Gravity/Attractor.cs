using UnityEngine;

public class Attractor : MonoBehaviour
{
    public float G = 6.674f;
    public Character owner;
    Character otherAttractor;



    private void OnTriggerStay2D(Collider2D collision)
    {
        otherAttractor = collision.GetComponent<Character>();

        if (otherAttractor != null && owner != null)
        {
            Attract(owner, otherAttractor);
        }
    }

    private void Attract(Character attractor, Character target)
    {
        if (attractor.characterType > CharacterType.Asteroid)
        {
            if (attractor.characterType >= target.characterType)
            {
                float massProduct = attractor.rb.mass * target.rb.mass;

                Vector3 direction = attractor.transform.position - target.transform.position;
                float distance = direction.magnitude;
                if (distance <= 0.1f)
                {
                    return;
                }

                float unScaledforceManguite = massProduct / Mathf.Pow(distance, 2);
                float forceMagnitude = G * GameManager.instance.status.GravitationalConstant * unScaledforceManguite;

                Vector3 force = direction.normalized * forceMagnitude;
                target.externalVelocity += (Vector2)force;

                //Debug.Log(force);
            }
        }

    }
}
