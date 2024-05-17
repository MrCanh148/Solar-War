using DG.Tweening;
using UnityEngine;

public class UpdateStatusCharacter : MonoBehaviour
{
    public Character owner;
    int currentMass;

    private void Start()
    {
        currentMass = (int)owner.rb.mass;
        if (owner.isPlayer)
        {
            ShowUI.instance.UpdateInfo();
        }

        UpdateInfoCharacter(owner);
        EvolutionCharacter(owner);
    }

    private void Update()
    {
        OnChangeData((int)owner.rb.mass);
    }
    public void UpdateInfoCharacter(Character character)
    {
        if (character.characterType == CharacterType.Asteroid)
        {
            character.tf.DOScale(character.tf.localScale + 0.01f * new Vector3(character.rb.mass, character.rb.mass, character.rb.mass), 0f);
        }

    }

    public void EvolutionCharacter(Character character)
    {
        if (character.characterType == CharacterType.SmallPlanet)
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

    public void OnChangeData(int newMass)
    {
        if (currentMass != newMass)
        {
            if (owner.isPlayer)
            {
                ShowUI.instance.UpdateInfo();
            }

            UpdateInfoCharacter(owner);
            EvolutionCharacter(owner);
        }

        currentMass = newMass;
    }
}
