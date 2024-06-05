using System.Collections;
using TMPro;
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
    [SerializeField] private float TimeEvolutionGO = 5f;

    private Character character;
    private Shield Shield;
    private bool isEvolutionInProgress = false;
    private Coroutine evolutionCoroutine;

    private void Start()
    {
        character = player.GetComponent<Character>();
        Shield = character.GetComponent<Shield>();
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
            evolutionCoroutine = StartCoroutine(EvolveOverTime(TimeEvolutionGO));
        }

        if (character.characterType != CharacterType.LifePlanet)
            isEvolutionInProgress = false;

        if (character.generalityType == GeneralityType.Star)
        {
            OffAllUI();
            UIinfo[3].SetActive(true);
            numberPlanet.text = character.NunmberOrbit.ToString() + " / " + character.MaxOrbit.ToString();
        }
        numberKill.text = character.Kill.ToString();

        ExpPlayer.value = character.Kill / MaxExp;
        ShieldPlayer.value = Shield.ShieldPlanet / Shield.MaxShield;
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
