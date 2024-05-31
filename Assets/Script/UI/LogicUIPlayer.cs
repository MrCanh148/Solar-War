using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LogicUIPlayer : MonoBehaviour
{
    [Header("0:Shield - 1:Exp - 2:Envolution - 3:Planet - 4:Kill")]
    [SerializeField] private GameObject[] UIinfo;
    [SerializeField] private GameObject player;

    [SerializeField] private TextMeshProUGUI numberPlanet;
    [SerializeField] private TextMeshProUGUI numberKill;

    [SerializeField] private Slider EvolutionSlider;
    [SerializeField] private Slider ShieldPlayer;
    [SerializeField] private Slider ExpPlayer;
    [SerializeField] private float MaxExp = 36;

    private Character character;
    private bool isEvolutionInProgress = false;
    private Coroutine evolutionCoroutine;

    private void Start()
    {
        character = player.GetComponent<Character>();
    }

    private void Update()
    {
        if (character.characterType == CharacterType.Asteroid
            || character.characterType == CharacterType.GasGiantPlanet
            || character.characterType == CharacterType.BlackHole
            || character.characterType == CharacterType.SmallPlanet)
        {
            OffAllUI();
        }

        if (character.characterType == CharacterType.LifePlanet && !isEvolutionInProgress)
        {
            OffAllUI();
            evolutionCoroutine = StartCoroutine(EvolveOverTime(20f));
        }

        if (character.characterType != CharacterType.LifePlanet)
            isEvolutionInProgress = false;

        if (character.generalityType == GeneralityType.Star)
        {
            OffAllUI();
            UIinfo[3].SetActive(true);
            if (character.characterType == CharacterType.SmallStar)
                numberPlanet.text = "0 / 5";
            if (character.characterType == CharacterType.BigStar)
                numberPlanet.text = "0 / 8";
            if (character.characterType == CharacterType.NeutronStar)
                numberPlanet.text = "0 / 4";
        }
        numberKill.text = character.Kill.ToString();

        ExpPlayer.value = character.Kill / MaxExp;
        ShieldPlayer.value = character.Shield / character.MaxShield;
    }

    private IEnumerator EvolveOverTime(float duration)
    {
        UIinfo[2].SetActive(true);
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            if (character.characterType != CharacterType.LifePlanet)
            {
                ResetEvolutionUI();
                yield break;
            }

            elapsedTime += Time.deltaTime;
            EvolutionSlider.value = Mathf.Clamp01(elapsedTime / duration);
            yield return null;
        }

        EvolutionSlider.value = 1f;
        UIinfo[0].SetActive(true);
        UIinfo[1].SetActive(true);
        UIinfo[2].SetActive(false);
        UIinfo[4].SetActive(true);

        isEvolutionInProgress = true;
    }

    private void ResetEvolutionUI()
    {
        UIinfo[2].SetActive(false);
        EvolutionSlider.value = 0f;
        isEvolutionInProgress = false;
    }

    private void OffAllUI()
    {
        foreach (GameObject go in UIinfo)
        {
            go.SetActive(false);
        }
        EvolutionSlider.value = 0f;
    }
}
