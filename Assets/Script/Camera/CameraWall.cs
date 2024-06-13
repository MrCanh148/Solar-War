using UnityEngine;

public class CameraWall : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Character character = Cache.GetCharacterCollider(collision);
        if (character != null)
        {
            if (!character.isPlayer)
            {
                if (character.characterType > CharacterType.Asteroid)
                    SpawnPlanets.instance.ActiveCharacter(character);
                else
                    SpawnPlanets.instance.DeActiveCharacter(character);
            }
        }

        AsteroidGroup asteroidGroup = collision.GetComponent<AsteroidGroup>();
        if (asteroidGroup != null)
        {

            if (asteroidGroup.AllChildrenDeActive())
            {

                asteroidGroup.transform.localPosition = SpawnPlanets.instance.ReSpawnerAsterrooidGroup();
                asteroidGroup.OnInit();
            }
            else
            {
                foreach (AsteroidGroup AG in SpawnPlanets.instance.asteroidGroups)
                {
                    if (AG.gameObject.activeSelf)
                    {
                        if (AG.AllChildrenDeActive())
                        {
                            AG.gameObject.SetActive(false);
                        }
                    }

                }

                foreach (AsteroidGroup AG in SpawnPlanets.instance.asteroidGroups)
                {

                    if (!AG.gameObject.activeSelf)
                    {
                        AG.gameObject.SetActive(true);
                        asteroidGroup.transform.localPosition = SpawnPlanets.instance.ReSpawnerAsterrooidGroup();
                        AG.OnInit();
                        break;
                    }

                }

            }
        }

        ShootTarget target = collision.GetComponent<ShootTarget>();
        if (target != null)
        {
            Destroy(target.gameObject);
        }
    }
}
