using UnityEngine;

public class DestroyTrigger : MonoBehaviour
{
    private ShootTarget target;
    private Bullet bullet;
    private Missile missile;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("AirSpace1"))
        {
            target = collision.gameObject.GetComponent<ShootTarget>();
            bullet = GetComponent<Bullet>();
            missile = GetComponent<Missile>();

            if (bullet != null)
            {
                target.heart -= bullet.damage;
                if (target.heart <= 0 )
                {
                    if (target.hostAlien != bullet.characterOwner && bullet.characterOwner != null)
                        bullet.characterOwner.Kill++;

                    AudioManager.instance.PlaySFX("Alien-Destroy");
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

                    AudioManager.instance.PlaySFX("Alien-Destroy");
                    Destroy(target.gameObject);
                }
            }

            Destroy(gameObject);
        }
    }
}
