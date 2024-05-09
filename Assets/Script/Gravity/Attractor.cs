using UnityEngine;

public class Attractor : MonoBehaviour
{
    const float G = 6.674f;
    private Rigidbody2D rb;
    private bool isAttraced, isOnCollider;
    private float TimeConnect, TimeLimit;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (isAttraced)
            TimeConnect += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Attractor otherAttractor = collision.GetComponent<Attractor>();
        if (otherAttractor != null && !isAttraced)
        {
            if (collision.gameObject.tag == "Asteroid")
            {

            }
            else
            {   
                Attract(otherAttractor);
                isAttraced = true;
            }
            isOnCollider = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Attractor otherAttractor = collision.GetComponent<Attractor>();
        if (otherAttractor != null && isAttraced)
        {
            isAttraced = false;
        }
        isOnCollider = false;
        TimeConnect = 0;
    }

    private void Attract(Attractor objToAttract)
    {
        Rigidbody2D rbToAttract = objToAttract.rb;

        Vector3 direction = rb.position - rbToAttract.position;
        float distance = direction.magnitude;

        if (distance == 0f)
            return;

        float forceMagnitude = G * (rb.mass * rbToAttract.mass) / Mathf.Pow(distance, 2);
        Vector3 force = direction.normalized * forceMagnitude;

        rbToAttract.AddForce(force);
    }
}
