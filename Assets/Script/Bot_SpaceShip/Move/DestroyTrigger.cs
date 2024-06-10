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
                target.heart -= bullet.damage;
                if (target.heart <= 0 )
                {
                    if (target.hostAlien != bullet.characterOwner && bullet.characterOwner != null)
                        bullet.characterOwner.Kill++;

                    Destroy(target.gameObject);
                }
            }

            if (missile != null)
            {
                target.heart -= missile.damage;
                if (target.heart <= 0 )
                {
                    if (target.hostAlien != missile.characterOwner && missile.characterOwner != null)
                        missile.characterOwner.Kill++;

                    Destroy(target.gameObject);
                }
            }

            Destroy(parentObject);
        }
    }
}
