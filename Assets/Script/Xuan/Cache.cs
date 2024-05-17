using System.Collections.Generic;
using UnityEngine;

public class Cache : MonoBehaviour
{
    private static readonly Dictionary<Collider2D, Character> colliderCharacters = new();

    public static Character GetCharacterCollider(Collider2D collider)
    {
        if (!colliderCharacters.ContainsKey(collider))
        {
            colliderCharacters.Add(collider, collider.GetComponent<Character>());
        }
        return colliderCharacters[collider];
    }


}
