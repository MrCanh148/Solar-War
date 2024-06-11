using System.Collections.Generic;
using UnityEngine;

public class SpawnPlanets : FastSingleton<SpawnPlanets>
{
    public List<CharacterInfo> CharacterInfos;
    //[SerializeField] private List<Character> Planets; //Cho spawn bằng nút
    //[SerializeField] private Transform player;
    public AsteroidGroup asteroidGroupPrefab;
    [SerializeField] private Transform tfCharacterManager;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private float FarFromPlayerMin, FarFromPlayerMax;
    [SerializeField] private float destroyDistance;
    [SerializeField] private Player player1;
    private Character lastSpawnedPlanet;
    private GameObject currentArrow;
    public int quantityAsteroid = 20;
    public int quantityPlanet = 0;
    public Camera _camera;
    float FarFromPlayer;

    private void Start()
    {
        _camera = Camera.main;
        FarFromPlayer = _camera.orthographicSize;
    }


    public void OnInit()
    {
        for (int i = 1; i <= quantityAsteroid; i++)
        {
            if (CharacterInfos[(int)CharacterType.Asteroid].characterPrefab != null)
            {
                Character character = Instantiate(CharacterInfos[(int)CharacterType.Asteroid].characterPrefab, tfCharacterManager);
                character.tf.localPosition = SpawnerCharacter();
                character.name = CharacterType.Asteroid.ToString() + i;
                ActiveCharacter(character);
                //activeCharacterList.Add(character);
            }
        }

        for (int i = 1; i <= GameManager.instance.AmountPlanet.amountAsteroidGroup; i++)
        {
            AsteroidGroup asteroidGroup = Instantiate(asteroidGroupPrefab, tfCharacterManager);
            asteroidGroup.transform.localPosition = SpawnerCharacter();
        }
    }

    private void Update()
    {
        for (int i = 1; i <= 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i) && player1.canWASD)
                SpawnPlanet(i - 1);
        }

        if (currentArrow != null)
            UpdateArrow();
    }

    private void SpawnPlanet(int index)
    {
        float xPos = Random.Range(player1.tf.position.x + FarFromPlayerMin, player1.tf.position.x + FarFromPlayerMax);
        float yPos = Random.Range(player1.tf.position.y + FarFromPlayerMin, player1.tf.position.y + FarFromPlayerMax);
        Vector3 placeSpawn = new Vector3(xPos, yPos, 0);

        if (index >= 0 && index < CharacterInfos.Count)
        {
            if (CharacterInfos[index].characterPrefab != null)
            {
                Character newPlanet = Instantiate(CharacterInfos[index].characterPrefab, placeSpawn, Quaternion.identity);
                lastSpawnedPlanet = newPlanet;
                UpdateArrow();
            }
        }
    }

    private void UpdateArrow()
    {
        if (currentArrow != null)
            Destroy(currentArrow);

        if (lastSpawnedPlanet != null)
        {
            Vector3 playerPosition = player1.tf.position;
            Vector3 targetPosition = lastSpawnedPlanet.transform.position;
            Vector3 direction = (targetPosition - playerPosition).normalized;

            currentArrow = Instantiate(arrowPrefab, playerPosition, Quaternion.identity);

            currentArrow.transform.up = direction;

            float distance = Vector3.Distance(playerPosition, targetPosition);
            if (distance < destroyDistance)
                Destroy(currentArrow);
        }
    }


    public Vector3 SpawnerCharacter()
    {
        float xPos = Random.Range(FarFromPlayer, GameManager.instance.status.coefficientActiveGameObject * FarFromPlayer);
        float yPos = Random.Range(FarFromPlayer, GameManager.instance.status.coefficientActiveGameObject * FarFromPlayer);
        xPos = RamdomValue(xPos);
        yPos = RamdomValue(yPos);
        return new Vector2(player1.tf.position.x + xPos, player1.tf.position.y + yPos);
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
        character.tf.localPosition = SpawnerCharacter();
        character.velocity = RandomInitialVelocity();
        if (character.characterType == CharacterType.Asteroid)
        {
            character.rb.mass = (int)Random.Range(1, 3);
        }
        else
        {
            character.rb.mass = GetRequiredMass(character.characterType) + GetRequiredMass(character.characterType + 1) / 10;
        }

    }

    public Vector2 RandomInitialVelocity()
    {
        float randomX = Random.Range(-3, 3);
        float randomY = Random.Range(-3, 3);
        return new Vector2(randomX, randomY);
    }

    public int GetRequiredMass(CharacterType characterType)
    {
        int mass = CharacterInfos[(int)characterType].requiredMass;
        return mass;
    }

    public string GetNamePlanet(CharacterType characterType)
    {
        string name = CharacterInfos[(int)characterType].namePlanet;
        return name;
    }


    public float GetScalePlanet(CharacterType characterType)
    {
        float scale;
        if (characterType == CharacterType.Asteroid)
        {
            scale = 0.06f;
        }
        else
            scale = CharacterInfos[(int)characterType].scale.x;

        return scale;
    }

    public void UpgradePlayerGenerality(Character character)
    {
        if (character.isPlayer)
        {

        }
    }
}
