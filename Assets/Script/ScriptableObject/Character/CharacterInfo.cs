using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Scriptable Objects/Characters")]
public class CharacterInfo : ScriptableObject
{
    public int id;
    public CharacterType characterType;
    public int requiredMass;
    public Sprite sprite;
    public Character characterPrefab;
    public float collisionCoefficient;
}
