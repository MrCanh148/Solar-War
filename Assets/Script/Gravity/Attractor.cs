using UnityEngine;

public class Attractor : MonoBehaviour
{
    public float G = 6.674f;
    private Character rb, otherAttractor;

    private void Start()
    {
        rb = GetComponent<Character>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        otherAttractor = collision.GetComponent<Character>();

        if (otherAttractor != null)
            Attract(rb, otherAttractor);
        
    }

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
