using DG.Tweening;
using UnityEngine;

public class UpdateStatusCharacter : MonoBehaviour
{
    public Character owner;
    int currentMass;
    CharacterInfo characterInfo;
    [SerializeField] private GameObject Minimap;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        OnInit();
        spriteRenderer = Minimap.GetComponent<SpriteRenderer>();
    }

    private void OnInit()
    {
        currentMass = (int)owner.rb.mass;
        if (owner.isPlayer)
        {
            ShowUI.instance.UpdateInfo();
        }

        //UpdateInfoCharacter(owner);
        EvolutionCharacter(owner);

    }

    private void Update()
    {
        OnChangeMass((int)owner.rb.mass);
    }
    public void UpdateInfoCharacter(Character character)
    {
        if (character.characterType == CharacterType.Asteroid)
        {
            character.tf.DOScale(new Vector3(0.05f, 0.05f, 0.05f) + 0.0015f * new Vector3(character.rb.mass, character.rb.mass, character.rb.mass), 0f);
        }
        foreach (var c in SpawnPlanets.instance.CharacterInfos)
        {
            if (character.characterType == c.characterType - 1)
            {
                if (character.rb.mass >= c.requiredMass)
                {
                    character.characterType = c.characterType;
                    character.spriteRenderer.sprite = c.sprite;
                    spriteRenderer.sprite = character.spriteRenderer.sprite;
                    character.tf.DOScale(c.scale, 0f);
                }
            }
        }
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
        else if (character.characterType == CharacterType.SmallStar || character.characterType == CharacterType.MediumStar || character.characterType == CharacterType.NeutronStar)
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
            {
                ShowUI.instance.UpdateInfo();
            }


        }

        currentMass = newMass;
    }


}
