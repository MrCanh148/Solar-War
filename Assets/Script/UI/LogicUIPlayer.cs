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
    [SerializeField] private Slider EvolutionSlider;
    [SerializeField] private TextMeshProUGUI numberKill;
    private Character character;
    private bool isEvolutionInProgress = false;

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
            StartCoroutine(EvolveOverTime(20f));
          
        }

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
    }

    private IEnumerator EvolveOverTime(float duration)
    {
        UIinfo[2].SetActive(true);
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
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

    private void OffAllUI()
    {
        foreach (GameObject go in UIinfo)
        {
            go.SetActive(false);
        }
        EvolutionSlider.value = 0f; 
    }
}
