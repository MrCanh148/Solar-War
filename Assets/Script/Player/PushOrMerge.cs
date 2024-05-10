using UnityEngine;

public class PushOrMerge : MonoBehaviour
{
    private Character character;
    private Attractor attractor;

    private void Start()
    {
        character = GetComponent<Character>();
        attractor = GetComponent<Attractor>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Character character2 = collision.gameObject.GetComponent<Character>();
        //attractor.stopAttracted = true;
        if (character2.characterType == character.characterType)
        {
            //Debug.Log(character2.name + ": " + character2.velocity.magnitude + " " + character.name + ": " + character.velocity.magnitude);
            if (character2.GetInstanceID() > character.GetInstanceID())
                HandleCollision(character2, character);
        }
    }

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    attractor.stopAttracted = false;
    //}

    public void HandleCollision(Character c1, Character c2)
    {
        float gravitational = (c1.velocity * c1.rb.mass - c2.velocity * c2.rb.mass).magnitude;
        if (gravitational <= GameManager.instance.status.minimumMergeForce)
        {
            Vector2 velocityC1 = (2 * c2.rb.mass * c2.velocity + (c1.rb.mass - c2.rb.mass) * c1.velocity) / (c1.rb.mass + c2.rb.mass);
            Vector2 velocityC2 = (2 * c1.rb.mass * c1.velocity + (c2.rb.mass - c1.rb.mass) * c2.velocity) / (c1.rb.mass + c2.rb.mass);

            c1.velocity = new Vector2(velocityC1.x, velocityC1.y);
            //c1.ResetExternalVelocity();

            c2.velocity = new Vector2(velocityC2.x, velocityC2.y);
            //c2.ResetExternalVelocity();
        }
        else
        {
            MergeCharacter(c1, c2);
            Vector2 velocityS = (c2.rb.mass * c2.velocity + c1.rb.mass * c1.velocity) / (c1.rb.mass + c2.rb.mass);
            c1.velocity = new Vector2(velocityS.x, velocityS.y);
        }
    }

    public void MergeCharacter(Character c1, Character c2)
    {
        c1.rb.mass++;
        c2.gameObject.SetActive(false);
    }
}
