using UnityEngine;

public class DestroyTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject parentObject = transform.parent.gameObject;

        if (collision.gameObject.tag == "AirSpace1")
        {
            Destroy(collision.gameObject); 
            Destroy(parentObject);
        }
    }
}
