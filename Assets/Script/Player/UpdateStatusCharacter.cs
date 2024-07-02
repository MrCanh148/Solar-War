using DG.Tweening;
using TMPro;
using UnityEngine;

public class UpdateStatusCharacter : MonoBehaviour
{
    public Character owner;
    int currentMass;
    [SerializeField] private GameObject Minimap;
    private SpriteRenderer spriteRenderer;
    int requiredMass;
    int currentGenerateType;
    [SerializeField] TextMeshProUGUI NameTxt;

    private void Start()
    {
        OnInit();
        if (Minimap != null)
            spriteRenderer = Minimap.GetComponent<SpriteRenderer>();
        if (NameTxt != null)
        {
            NameTxt.text = SpawnPlanets.instance.CharacterInfos[(int)owner.characterType].namePlanet;
        }
        requiredMass = SpawnPlanets.instance.GetRequiredMass(owner.characterType);
        currentGenerateType = (int)owner.generalityType;

    }

    private void OnInit()
    {
        currentMass = (int)owner.rb.mass;
        if (owner.isPlayer)
            LogicUIPlayer.Instance.UpdateInfo();

        EvolutionCharacter(owner);

    }

    private void Update()
    {
        OnChangeMass((int)owner.rb.mass);
        OnChangeGenerateType((int)owner.generalityType);
        //EvolutionCharacter(owner);
    }

    public void UpdateInfoCharacter(Character character)
    {
        bool typeChanged;
        do
        {
            typeChanged = false;

            if (character.characterType == CharacterType.Asteroid)
                character.tf.DOScale(new Vector3(0.05f, 0.05f, 0.05f) + 0.0015f * new Vector3(character.rb.mass, character.rb.mass, character.rb.mass), 0f);

            foreach (var c in SpawnPlanets.instance.CharacterInfos)
            {


                if (character.characterType == c.characterType - 1) // Tăng CharacterType
                {
                    if (character.rb.mass >= c.requiredMass)
                    {
                        character.characterType = c.characterType;
                        character.spriteRenderer.sprite = c.sprite;
                        if (Minimap != null)
                            spriteRenderer.sprite = character.spriteRenderer.sprite;
                        character.tf.DOScale(c.scale, 0f);
                        typeChanged = true;
                        if (NameTxt != null)
                        {
                            NameTxt.text = SpawnPlanets.instance.CharacterInfos[(int)owner.characterType].namePlanet;
                        }
                        if (character.isPlayer && GameManager.instance.IsGameMode(GameMode.Normal))
                        {
                            SpawnPlanets.instance.AdjustSpawnRates(character.characterType);
                        }
                        break;
                    }
                }

                if (character.characterType == c.characterType + 1) // Giảm CharacterType
                {
                    if (character.rb.mass < requiredMass)
                    {
                        character.characterType = c.characterType;
                        character.spriteRenderer.sprite = c.sprite;
                        if (Minimap != null)
                            spriteRenderer.sprite = character.spriteRenderer.sprite;
                        character.tf.DOScale(c.scale, 0f);
                        typeChanged = true;
                        if (NameTxt != null)
                        {
                            NameTxt.text = SpawnPlanets.instance.CharacterInfos[(int)owner.characterType].namePlanet;
                        }
                        if (character.isPlayer)
                        {
                            if (GameManager.instance.IsGameMode(GameMode.Normal))
                                SpawnPlanets.instance.AdjustSpawnRates(character.characterType);
                            else if (GameManager.instance.IsGameMode(GameMode.Survival))
                                GameManager.instance.ChangeGameState(GameState.GameOver);
                        }
                        break;
                    }
                }

                if (character.characterType == c.characterType)
                {
                    requiredMass = c.requiredMass;
                }


            }
        } while (typeChanged); // Tiếp tục vòng lặp nếu loại đã thay đổi

    }


    public void EvolutionCharacter(Character character)
    {
        if (character.characterType == CharacterType.Asteroid)
        {
            character.generalityType = GeneralityType.Asteroid;
        }
        else if (character.characterType == CharacterType.SmallPlanet || character.characterType == CharacterType.LifePlanet || character.characterType == CharacterType.GasGiantPlanet)
        {
            character.generalityType = GeneralityType.Planet;
        }
        else if (character.characterType == CharacterType.SmallStar || character.characterType == CharacterType.MediumStar || character.characterType == CharacterType.NeutronStar || character.characterType == CharacterType.BigStar)
        {
            character.generalityType = GeneralityType.Star;
        }
        else if (character.characterType == CharacterType.BlackHole || character.characterType == CharacterType.BigCrunch || character.characterType == CharacterType.BigBang)
        {
            character.generalityType = GeneralityType.BlackHole;
        }
    }

    public void OnChangeMass(int newMass)
    {
        if (currentMass != newMass)
        {
            UpdateInfoCharacter(owner);
            EvolutionCharacter(owner);
            if (owner.isPlayer)
                LogicUIPlayer.Instance.UpdateInfo();
        }

        currentMass = newMass;
    }

    public void OnChangeGenerateType(int newType)
    {
        if (currentGenerateType > newType)  // tụt cấp generalityType
        {
            if (!owner.isBasicReSpawn)
            {
                AudioManager.instance.PlaySFX("Planet-destroy");
                owner.AllWhenDie();
                SpawnPlanets.instance.ActiveCharacter(owner, owner.characterType + 1);
            }
            else
            {
                owner.AllWhenDie();
                owner.isBasicReSpawn = false;
            }

        }
        else if (currentGenerateType < newType)  // lên cấp generalityType
        {
            if (!owner.isBasicReSpawn)
            {
                owner.AllWhenDie();
                owner.rb.mass += (SpawnPlanets.instance.GetRequiredMass(owner.characterType + 1) - SpawnPlanets.instance.GetRequiredMass(owner.characterType)) / 2;
            }
            else
            {
                owner.AllWhenDie();
                owner.isBasicReSpawn = false;
            }

        }
        currentGenerateType = newType;

    }

}