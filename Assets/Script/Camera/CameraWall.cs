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
                SpawnPlanets.instance.ActiveCharacter(character);
            }
        }
    }
}
