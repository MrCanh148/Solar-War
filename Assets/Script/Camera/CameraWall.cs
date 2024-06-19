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
                    SpawnPlanets.instance.ActiveCharacter2(character);
                else
                {
                    SpawnPlanets.instance.DeActiveCharacter(character);
                }

            }
        }

        AsteroidGroup asteroidGroup = collision.GetComponent<AsteroidGroup>();
        if (asteroidGroup != null)
        {
            asteroidGroup.OnInit();
            asteroidGroup.transform.localPosition = SpawnPlanets.instance.ReSpawnerAsterrooidGroup();
        }

        ShootTarget target = collision.GetComponent<ShootTarget>();
        if (target != null)
        {
            Destroy(target.gameObject);
        }
    }
}
