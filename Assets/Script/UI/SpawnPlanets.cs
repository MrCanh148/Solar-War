using System.Collections.Generic;
using UnityEngine;

public class SpawnPlanets : FastSingleton<SpawnPlanets>
{
    public List<CharacterInfo> CharacterInfos;
    public AsteroidGroup asteroidGroupPrefab;
    [SerializeField] private Transform tfCharacterManager;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private float FarFromPlayerMin, FarFromPlayerMax;
    [SerializeField] private float destroyDistance;
    [SerializeField] private Player player1;
    private Character lastSpawnedPlanet;
    private GameObject currentArrow;

    public Camera _camera;
    float FarFromPlayerY;
    float FarFromPlayerX;
    public List<Character> lstCharacter;
    public List<AsteroidGroup> asteroidGroups;
    public SortedDictionary<CharacterType, int> spawnRates = new SortedDictionary<CharacterType, int>();



    private void Start()
    {
        _camera = Camera.main;
        FarFromPlayerY = _camera.orthographicSize;
        FarFromPlayerX = FarFromPlayerY * (float)_camera.pixelWidth / _camera.pixelHeight;
        AdjustSpawnRates(player1);
        foreach (var rate in spawnRates)
        {
            Debug.Log(rate.Key + " " + rate.Value);
        }
    }


    public void OnInit()
    {
        asteroidGroups.Clear();
        lstCharacter.Clear();
        for (int i = 1; i <= GameManager.instance.AmountPlanet.amountAsteroidGroup; i++)
        {
            AsteroidGroup asteroidGroup = Instantiate(asteroidGroupPrefab, tfCharacterManager);
            asteroidGroups.Add(asteroidGroup);
            asteroidGroup.transform.localPosition = SpawnerCharacter();
        }



        for (int i = 1; i <= GameManager.instance.AmountPlanet.amountSmallPlanet; i++)  //smallplanet
        {
            if (CharacterInfos[(int)CharacterType.SmallPlanet].characterPrefab != null)
            {
                Character character = Instantiate(CharacterInfos[(int)CharacterType.SmallPlanet].characterPrefab, tfCharacterManager);
                DeActiveCharacter(character);
                lstCharacter.Add(character);
            }
        }

        for (int i = 1; i <= GameManager.instance.AmountPlanet.amountLifePlanet; i++)  //lifeplanet
        {
            if (CharacterInfos[(int)CharacterType.LifePlanet].characterPrefab != null)
            {
                Character character = Instantiate(CharacterInfos[(int)CharacterType.LifePlanet].characterPrefab, tfCharacterManager);
                DeActiveCharacter(character);
                lstCharacter.Add(character);
            }
        }

        for (int i = 1; i <= GameManager.instance.AmountPlanet.amountGasGiantPlanet; i++) // gasgiantplanet
        {
            if (CharacterInfos[(int)CharacterType.GasGiantPlanet].characterPrefab != null)
            {
                Character character = Instantiate(CharacterInfos[(int)CharacterType.GasGiantPlanet].characterPrefab, tfCharacterManager);
                DeActiveCharacter(character);
                lstCharacter.Add(character);
            }
        }

        for (int i = 1; i <= GameManager.instance.AmountPlanet.amountSmallStar; i++) //smallstar
        {
            if (CharacterInfos[(int)CharacterType.SmallStar].characterPrefab != null)
            {
                Character character = Instantiate(CharacterInfos[(int)CharacterType.SmallStar].characterPrefab, tfCharacterManager);
                DeActiveCharacter(character);
                lstCharacter.Add(character);
            }
        }

        for (int i = 1; i <= GameManager.instance.AmountPlanet.amountMediumStar; i++)   //mediumstar
        {
            if (CharacterInfos[(int)CharacterType.MediumStar].characterPrefab != null)
            {
                Character character = Instantiate(CharacterInfos[(int)CharacterType.MediumStar].characterPrefab, tfCharacterManager);
                DeActiveCharacter(character);
                lstCharacter.Add(character);
            }
        }

        for (int i = 1; i <= GameManager.instance.AmountPlanet.amountBigStar; i++)  //bigstar
        {
            if (CharacterInfos[(int)CharacterType.BigStar].characterPrefab != null)
            {
                Character character = Instantiate(CharacterInfos[(int)CharacterType.BigStar].characterPrefab, tfCharacterManager);
                DeActiveCharacter(character);
                lstCharacter.Add(character);
            }
        }

        for (int i = 1; i <= GameManager.instance.AmountPlanet.amountNeutronStar; i++)  //neutronstar
        {
            if (CharacterInfos[(int)CharacterType.NeutronStar].characterPrefab != null)
            {
                Character character = Instantiate(CharacterInfos[(int)CharacterType.NeutronStar].characterPrefab, tfCharacterManager);
                DeActiveCharacter(character);
                lstCharacter.Add(character);
            }
        }

        for (int i = 1; i <= GameManager.instance.AmountPlanet.amountBlackHole; i++) //blackhole
        {
            if (CharacterInfos[(int)CharacterType.BlackHole].characterPrefab != null)
            {
                Character character = Instantiate(CharacterInfos[(int)CharacterType.BlackHole].characterPrefab, tfCharacterManager);
                DeActiveCharacter(character);
                lstCharacter.Add(character);
            }
        }

        UpgradePlayerGenerality(player1);
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
        float xPos = Random.Range(FarFromPlayerX * 1.5f, GameManager.instance.status.coefficientActiveGameObject * FarFromPlayerX);
        float yPos = Random.Range(FarFromPlayerY * 1.5f, GameManager.instance.status.coefficientActiveGameObject * FarFromPlayerY);
        xPos = RamdomValue(xPos);
        yPos = RamdomValue(yPos);
        return new Vector2(player1.tf.position.x + xPos, player1.tf.position.y + yPos);
    }

    public Vector3 ReSpawnerAsterrooidGroup()
    {
        float distance = FarFromPlayerX > FarFromPlayerY ? FarFromPlayerX : FarFromPlayerY;
        //float random = Random.Range(FarFromPlayerY * 1.5f, (GameManager.instance.status.coefficientActiveGameObject - .5f) * FarFromPlayerY);
        return player1.tf.position + ((Vector3)player1.mainVelocity.normalized * distance * (GameManager.instance.status.coefficientActiveGameObject + 1) / 2);
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

    public void ActiveCharacter(Character character, CharacterType type)
    {
        if (character.isPlayer)
            ReSpawnPlayer.Instance.ResPlayer();
        else
        {
            character.gameObject.SetActive(true);
            character.tf.localPosition = SpawnerCharacter();
            character.velocity = RandomInitialVelocity(2f);
        }

        if (character.characterType == CharacterType.Asteroid)
        {
            character.rb.mass = (int)Random.Range(1, 3);
          
        }
        else
        {
            character.rb.mass = GetRequiredMass(type) + GetRequiredMass(type + 1) / 10;
        }

    }

    public Vector2 RandomInitialVelocity(float limit)
    {
        float randomX = Random.Range(-limit, limit);
        float randomY = Random.Range(-limit, limit);
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

            if (character.characterType == CharacterType.Asteroid)        //asteroid
            {
                foreach (Character c in lstCharacter)
                {
                    if (!c.gameObject.activeSelf && c != null)
                    {
                        if (c.characterType > CharacterType.SmallPlanet)
                        {
                            DeActiveCharacter(c);
                        }
                        else
                        {
                            ActiveCharacter(c, c.characterType);
                        }
                    }
                }
            }
            else if (character.characterType == CharacterType.SmallPlanet)  //small planet
            {
                foreach (Character c in lstCharacter)
                {
                    if (!c.gameObject.activeSelf && c != null)
                    {

                        if (c.characterType > CharacterType.SmallStar)
                        {
                            DeActiveCharacter(c);
                        }
                        else
                        {
                            ActiveCharacter(c, c.characterType);
                        }
                    }
                }
            }
            else if (character.characterType == CharacterType.LifePlanet)  //life planet
            {
                foreach (Character c in lstCharacter)
                {
                    if (!c.gameObject.activeSelf && c != null)
                    {

                        if (c.characterType > CharacterType.BigStar)
                        {
                            DeActiveCharacter(c);
                        }
                        else
                        {
                            ActiveCharacter(c, c.characterType);
                        }
                    }
                }
            }
            else if (character.characterType == CharacterType.GasGiantPlanet)  //gas giant planet
            {
                foreach (Character c in lstCharacter)
                {
                    if (!c.gameObject.activeSelf && c != null)
                    {

                        if (c.characterType > CharacterType.NeutronStar)
                        {
                            DeActiveCharacter(c);
                        }
                        else
                        {
                            ActiveCharacter(c, c.characterType);
                        }
                    }
                }
            }
            else if (character.characterType == CharacterType.SmallStar)  //small star
            {
                foreach (Character c in lstCharacter)
                {
                    if (!c.gameObject.activeSelf && c != null)
                    {

                        if (c.characterType > CharacterType.NeutronStar)
                        {
                            DeActiveCharacter(c);
                        }
                        else
                        {
                            ActiveCharacter(c, c.characterType);
                        }
                    }
                }
            }
            else if (character.characterType == CharacterType.MediumStar)  //medium star
            {
                foreach (Character c in lstCharacter)
                {
                    if (!c.gameObject.activeSelf && c != null)
                    {

                        if (c.characterType > CharacterType.NeutronStar)
                        {
                            DeActiveCharacter(c);
                        }
                        else
                        {
                            ActiveCharacter(c, c.characterType);
                        }
                    }
                }
            }
            else if (character.characterType == CharacterType.BigStar)  //big star
            {
                foreach (Character c in lstCharacter)
                {
                    if (!c.gameObject.activeSelf && c != null)
                    {

                        if (c.characterType > CharacterType.NeutronStar)
                        {
                            DeActiveCharacter(c);
                        }
                        else
                        {
                            ActiveCharacter(c, c.characterType);
                        }
                    }
                }
            }
            else if (character.characterType == CharacterType.NeutronStar)
            {
                foreach (Character c in lstCharacter)
                {
                    if (!c.gameObject.activeSelf && c != null)
                    {
                        ActiveCharacter(c, c.characterType);
                    }
                }
            }
            else if (character.characterType == CharacterType.BlackHole)
            {
                foreach (Character c in lstCharacter)
                {
                    if (!c.gameObject.activeSelf && c != null)
                    {
                        ActiveCharacter(c, c.characterType);
                    }
                }
            }
            else if (character.characterType == CharacterType.BigCrunch)
            {
                foreach (Character c in lstCharacter)
                {
                    if (!c.gameObject.activeSelf && c != null)
                    {
                        ActiveCharacter(c, c.characterType);
                    }
                }
            }
            else if (character.characterType == CharacterType.BigCrunch)
            {
                foreach (Character c in lstCharacter)
                {
                    if (!c.gameObject.activeSelf && c != null)
                    {
                        ActiveCharacter(c, c.characterType);
                    }
                }
            }
        }
    }


    public void AdjustSpawnRates(Character character)
    {
        // Thay đổi tỷ lệ xuất hiện dựa trên CharacterType của người chơi
        switch (character.characterType)
        {
            case CharacterType.Asteroid:
                spawnRates[CharacterType.Asteroid] = 30;
                spawnRates[CharacterType.SmallPlanet] = 40;
                spawnRates[CharacterType.LifePlanet] = 0;
                spawnRates[CharacterType.GasGiantPlanet] = 0;
                spawnRates[CharacterType.SmallStar] = 0;
                spawnRates[CharacterType.MediumStar] = 0;
                spawnRates[CharacterType.BigStar] = 0;
                spawnRates[CharacterType.NeutronStar] = 0;
                spawnRates[CharacterType.BlackHole] = 0;
                spawnRates[CharacterType.BigCrunch] = 0;
                spawnRates[CharacterType.BigBang] = 0;
                break;
            case CharacterType.SmallPlanet:
                spawnRates[CharacterType.Asteroid] = 100;
                spawnRates[CharacterType.SmallPlanet] = 50;
                spawnRates[CharacterType.LifePlanet] = 40;
                spawnRates[CharacterType.GasGiantPlanet] = 30;
                spawnRates[CharacterType.SmallStar] = 20;
                spawnRates[CharacterType.MediumStar] = 0;
                spawnRates[CharacterType.BigStar] = 0;
                spawnRates[CharacterType.NeutronStar] = 0;
                spawnRates[CharacterType.BlackHole] = 0;
                spawnRates[CharacterType.BigCrunch] = 0;
                spawnRates[CharacterType.BigBang] = 0;
                break;
            case CharacterType.LifePlanet:
                spawnRates[CharacterType.Asteroid] = 100;
                spawnRates[CharacterType.SmallPlanet] = 0;
                spawnRates[CharacterType.LifePlanet] = 0;
                spawnRates[CharacterType.GasGiantPlanet] = 0;
                spawnRates[CharacterType.SmallStar] = 0;
                spawnRates[CharacterType.MediumStar] = 0;
                spawnRates[CharacterType.BigStar] = 0;
                spawnRates[CharacterType.NeutronStar] = 0;
                spawnRates[CharacterType.BlackHole] = 0;
                spawnRates[CharacterType.BigCrunch] = 0;
                spawnRates[CharacterType.BigBang] = 0;
                break;
            case CharacterType.GasGiantPlanet:
                spawnRates[CharacterType.Asteroid] = 100;
                spawnRates[CharacterType.SmallPlanet] = 0;
                spawnRates[CharacterType.LifePlanet] = 0;
                spawnRates[CharacterType.GasGiantPlanet] = 0;
                spawnRates[CharacterType.SmallStar] = 0;
                spawnRates[CharacterType.MediumStar] = 0;
                spawnRates[CharacterType.BigStar] = 0;
                spawnRates[CharacterType.NeutronStar] = 0;
                spawnRates[CharacterType.BlackHole] = 0;
                spawnRates[CharacterType.BigCrunch] = 0;
                spawnRates[CharacterType.BigBang] = 0;
                break;
            case CharacterType.SmallStar:
                spawnRates[CharacterType.Asteroid] = 100;
                spawnRates[CharacterType.SmallPlanet] = 0;
                spawnRates[CharacterType.LifePlanet] = 0;
                spawnRates[CharacterType.GasGiantPlanet] = 0;
                spawnRates[CharacterType.SmallStar] = 0;
                spawnRates[CharacterType.MediumStar] = 0;
                spawnRates[CharacterType.BigStar] = 0;
                spawnRates[CharacterType.NeutronStar] = 0;
                spawnRates[CharacterType.BlackHole] = 0;
                spawnRates[CharacterType.BigCrunch] = 0;
                spawnRates[CharacterType.BigBang] = 0;
                break;
            case CharacterType.MediumStar:
                spawnRates[CharacterType.Asteroid] = 100;
                spawnRates[CharacterType.SmallPlanet] = 0;
                spawnRates[CharacterType.LifePlanet] = 0;
                spawnRates[CharacterType.GasGiantPlanet] = 0;
                spawnRates[CharacterType.SmallStar] = 0;
                spawnRates[CharacterType.MediumStar] = 0;
                spawnRates[CharacterType.BigStar] = 0;
                spawnRates[CharacterType.NeutronStar] = 0;
                spawnRates[CharacterType.BlackHole] = 0;
                spawnRates[CharacterType.BigCrunch] = 0;
                spawnRates[CharacterType.BigBang] = 0;
                break;
            case CharacterType.BigStar:
                spawnRates[CharacterType.Asteroid] = 100;
                spawnRates[CharacterType.SmallPlanet] = 0;
                spawnRates[CharacterType.LifePlanet] = 0;
                spawnRates[CharacterType.GasGiantPlanet] = 0;
                spawnRates[CharacterType.SmallStar] = 0;
                spawnRates[CharacterType.MediumStar] = 0;
                spawnRates[CharacterType.BigStar] = 0;
                spawnRates[CharacterType.NeutronStar] = 0;
                spawnRates[CharacterType.BlackHole] = 0;
                spawnRates[CharacterType.BigCrunch] = 0;
                spawnRates[CharacterType.BigBang] = 0;
                break;
            case CharacterType.NeutronStar:
                spawnRates[CharacterType.Asteroid] = 100;
                spawnRates[CharacterType.SmallPlanet] = 0;
                spawnRates[CharacterType.LifePlanet] = 0;
                spawnRates[CharacterType.GasGiantPlanet] = 0;
                spawnRates[CharacterType.SmallStar] = 0;
                spawnRates[CharacterType.MediumStar] = 0;
                spawnRates[CharacterType.BigStar] = 0;
                spawnRates[CharacterType.NeutronStar] = 0;
                spawnRates[CharacterType.BlackHole] = 0;
                spawnRates[CharacterType.BigCrunch] = 0;
                spawnRates[CharacterType.BigBang] = 0;
                break;
            case CharacterType.BlackHole:
                spawnRates[CharacterType.Asteroid] = 100;
                spawnRates[CharacterType.SmallPlanet] = 0;
                spawnRates[CharacterType.LifePlanet] = 0;
                spawnRates[CharacterType.GasGiantPlanet] = 0;
                spawnRates[CharacterType.SmallStar] = 0;
                spawnRates[CharacterType.MediumStar] = 0;
                spawnRates[CharacterType.BigStar] = 0;
                spawnRates[CharacterType.NeutronStar] = 0;
                spawnRates[CharacterType.BlackHole] = 0;
                spawnRates[CharacterType.BigCrunch] = 0;
                spawnRates[CharacterType.BigBang] = 0;
                break;
            case CharacterType.BigCrunch:
                spawnRates[CharacterType.Asteroid] = 100;
                spawnRates[CharacterType.SmallPlanet] = 0;
                spawnRates[CharacterType.LifePlanet] = 0;
                spawnRates[CharacterType.GasGiantPlanet] = 0;
                spawnRates[CharacterType.SmallStar] = 0;
                spawnRates[CharacterType.MediumStar] = 0;
                spawnRates[CharacterType.BigStar] = 0;
                spawnRates[CharacterType.NeutronStar] = 0;
                spawnRates[CharacterType.BlackHole] = 0;
                spawnRates[CharacterType.BigCrunch] = 0;
                spawnRates[CharacterType.BigBang] = 0;
                break;
            case CharacterType.BigBang:
                spawnRates[CharacterType.Asteroid] = 100;
                spawnRates[CharacterType.SmallPlanet] = 0;
                spawnRates[CharacterType.LifePlanet] = 0;
                spawnRates[CharacterType.GasGiantPlanet] = 0;
                spawnRates[CharacterType.SmallStar] = 0;
                spawnRates[CharacterType.MediumStar] = 0;
                spawnRates[CharacterType.BigStar] = 0;
                spawnRates[CharacterType.NeutronStar] = 0;
                spawnRates[CharacterType.BlackHole] = 0;
                spawnRates[CharacterType.BigCrunch] = 0;
                spawnRates[CharacterType.BigBang] = 0;
                break;
            default:
                break;
        }

        Debug.Log(spawnRates[CharacterType.Asteroid]);
        Debug.Log(spawnRates[CharacterType.SmallPlanet]);

    }

    public CharacterType RandomCharacterType()
    {
        CharacterType characterType = CharacterType.Asteroid;
        int random = Random.Range(1, 101);
        foreach (var rate in spawnRates)
        {
            if (random <= rate.Value)
            {
                characterType = rate.Key;
                break;
            }
        }
        return characterType;
    }

}
