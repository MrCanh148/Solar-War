using UnityEngine;

public class SpawnRates
{
    public CharacterType CharacterType;
    public float SpawnRate;

    public SpawnRates(CharacterType characterType, float spawnRate)
    {
        CharacterType = characterType;
        SpawnRate = spawnRate;
    }
}
