using UnityEngine;

public class DestroyTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject parentObject = transform.parent.gameObject;

        if (collision.gameObject.tag == "AirSpace1")
        {
            ShootTarget target = collision.gameObject.GetComponent<ShootTarget>();
            Bullet bullet = parentObject.GetComponent<Bullet>();
            Missile missile = parentObject.GetComponent<Missile>();

            if (bullet != null)
            {
                if (target.host != bullet.characterOwner)
                    bullet.characterOwner.Kill++;
            }

            if (missile != null)
            {
                if (target.host != missile.characterOwner)
                    missile.characterOwner.Kill++;
            }

            Destroy(collision.gameObject); 
            Destroy(parentObject);
        }
    }
}
