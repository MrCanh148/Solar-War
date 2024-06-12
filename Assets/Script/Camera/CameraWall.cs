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
            asteroidGroup.transform.localPosition = SpawnPlanets.instance.ReSpawnerAsterrooidGroup();
            asteroidGroup.OnInit();

        }
    }
}
