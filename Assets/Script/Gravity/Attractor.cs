using UnityEngine;

public class Attractor : MonoBehaviour
{
    public float G = 6.674f;
    private Character rb, otherAttractor;
    //private bool isAttraced;

    private void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        rb = GetComponent<Character>();
        otherAttractor = collision.GetComponent<Character>();

        if (otherAttractor != null)
        {   
            Attract(rb, otherAttractor);
        }
    }
    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    Attractor otherAttractor = collision.GetComponent<Attractor>();
    //    if (otherAttractor != null && isAttraced)
    //    {
    //        isAttraced = false;
    //    }
    //}

    private void Attract(Character attractor, Character target)
    {
        float massProduct = attractor.rb.mass * target.rb.mass;

        Vector3 direction = attractor.transform.position - target.transform.position;
        float distance = direction.magnitude;

        float unScaledforceManguite = massProduct / Mathf.Pow(distance, 2);
        float forceMagnitude = G * unScaledforceManguite;

        Vector3 force = direction.normalized * forceMagnitude;
        target.externalVelocity = force;
    }
}
