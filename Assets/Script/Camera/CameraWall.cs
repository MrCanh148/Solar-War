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
                SpawnPlanets.instance.DeActiveCharacter(character);
                //SpawnPlanets.instance.ActiveCharacter(character);
            }
        }

        AsteroidGroup asteroidGroup = collision.GetComponent<AsteroidGroup>();
        if (asteroidGroup != null)
        {
            asteroidGroup.transform.localPosition = SpawnPlanets.instance.SpawnerCharacter();
            asteroidGroup.OnInit();

        }
    }
}
