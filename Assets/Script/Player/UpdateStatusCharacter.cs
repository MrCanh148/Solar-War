using DG.Tweening;
using UnityEngine;

public class UpdateStatusCharacter : MonoBehaviour
{
    public Character owner;
    int currentMass;
    CharacterInfo characterInfo;

    private void Start()
    {
        OnInit();
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
            character.tf.DOScale(character.tf.localScale + 0.0002f * new Vector3(character.rb.mass, character.rb.mass, character.rb.mass), 0f);
        }
        foreach (var c in SpawnPlanets.instance.CharacterInfos)
        {
            if (character.characterType == c.characterType - 1)
            {
                if (character.rb.mass == c.requiredMass)
                {
                    character.characterType = c.characterType;
                    character.spriteRenderer.sprite = c.sprite;
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
