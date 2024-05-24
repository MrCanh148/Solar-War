using System.Collections.Generic;
using UnityEngine;

public class SpawnPlanets : FastSingleton<SpawnPlanets>
{
    public List<CharacterInfo> CharacterInfos;
    [SerializeField] private List<Character> Planets; //Cho spawn bằng nút
    [SerializeField] private Transform player;
    [SerializeField] private Transform tfCharacterManager;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private float FarFromPlayerMin, FarFromPlayerMax;
    [SerializeField] private float destroyDistance;
    private Character lastSpawnedPlanet;
    private GameObject currentArrow;
    public int quantityAsteroid = 20;
    public int quantityPlanet = 0;


    private void Start()
    {
        for (int i = 0; i < Planets.Count; i++)
        {
            if (Planets[i].characterType == CharacterType.Asteroid)
            {
                for (int j = 1; j <= quantityAsteroid; j++)
                {
                    Character character = Instantiate(Planets[i], tfCharacterManager);
                    SpawnerCharacter(character);
                    character.name = CharacterType.Asteroid.ToString() + i;
                    ActiveCharacter(character);
                    //activeCharacterList.Add(character);
                }
                break;
            }

        }
    }


    private void Update()
    {
        for (int i = 1; i <= 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i))
                SpawnPlanet(i - 1);
        }

        if (currentArrow != null)
            UpdateArrow();
    }

    private void SpawnPlanet(int index)
    {
        float xPos = Random.Range(player.position.x + FarFromPlayerMin, player.position.x + FarFromPlayerMax);
        float yPos = Random.Range(player.position.y + FarFromPlayerMin, player.position.y + FarFromPlayerMax);
        Vector3 placeSpawn = new Vector3(xPos, yPos, 0);

        if (index >= 0 && index < Planets.Count)
        {
            Character newPlanet = Instantiate(Planets[index], placeSpawn, Quaternion.identity);
            lastSpawnedPlanet = newPlanet;
            UpdateArrow();
        }
    }

    private void UpdateArrow()
    {
        if (currentArrow != null)
            Destroy(currentArrow);

        if (lastSpawnedPlanet != null)
        {
            Vector3 playerPosition = player.position;
            Vector3 targetPosition = lastSpawnedPlanet.transform.position;
            Vector3 direction = (targetPosition - playerPosition).normalized;

            currentArrow = Instantiate(arrowPrefab, playerPosition, Quaternion.identity);

            currentArrow.transform.up = direction;

            float distance = Vector3.Distance(playerPosition, targetPosition);
            if (distance < destroyDistance)
                Destroy(currentArrow);
        }
    }


    public void SpawnerCharacter(Character character)
    {
        float xPos = Random.Range(FarFromPlayerMin, GameManager.instance.status.coefficientActiveGameObject * FarFromPlayerMin);
        float yPos = Random.Range(FarFromPlayerMin, GameManager.instance.status.coefficientActiveGameObject * FarFromPlayerMin);
        xPos = RamdomValue(xPos);
        yPos = RamdomValue(yPos);
        character.tf.localPosition = new Vector2(player.position.x + xPos, player.position.y + yPos);
    }

    public float RamdomValue(float n)
    {
        float number = n;
        int randomValue = Random.Range(0, 2);
        if (randomValue == 0)
        {
            number = -n;
        }
        else if (randomValue == 1)
        {
            number = n;
        }
        return number;
    }

    public void DeActiveCharacter(Character character)
    {
        character.gameObject.SetActive(false);

    }

    public void ActiveCharacter(Character character)
    {
        character.gameObject.SetActive(true);
        SpawnerCharacter(character);
        character.velocity = RandomInitialVelocity();
    }

    public Vector2 RandomInitialVelocity()
    {
        float randomX = Random.Range(-3, 3);
        float randomY = Random.Range(-3, 3);
        return new Vector2(randomX, randomY);
    }

    public int GetRequiredMass(CharacterType characterType)
    {
        int mass = 0;
        foreach (var c in CharacterInfos)
        {
            if (c.characterType == characterType)
            {
                mass = c.requiredMass;
            }
        }
        return mass;
    }

    public string GetNamePlanet(CharacterType characterType)
    {
        string name = "";
        foreach (var c in CharacterInfos)
        {
            if (c.characterType == characterType)
                name = c.namePlanet;
        }
        return name;
    }

}
