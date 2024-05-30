using TMPro;
using UnityEngine;

public class LogicUIPlayer : MonoBehaviour
{
    [Header("0:Shield - 1:Exp - 2:Envolution - 3:Planet - 4:Kill")]
    [SerializeField] private GameObject[] UIinfo;
    [SerializeField] private GameObject player;
    [SerializeField] private TextMeshProUGUI numberPlanet;
    private Character character;
    private Rigidbody2D rb;

    private void Start()
    {
        character = player.GetComponent<Character>();
        rb = player.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (character.characterType == CharacterType.Asteroid || character.characterType == CharacterType.GasGiantPlanet || character.characterType == CharacterType.BlackHole)
            OffAllUI();

        if (character.characterType == CharacterType.SmallPlanet)
        {
            OffAllUI();
            if (rb.mass >= 40)
                UIinfo[2].SetActive(true);
        }

        if (character.characterType == CharacterType.LifePlanet)
        {
            OffAllUI();
            UIinfo[0].SetActive(true);
            UIinfo[1].SetActive(true);
            UIinfo[4].SetActive(true);
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
    }

    private void OffAllUI()
    {
        foreach (GameObject go in UIinfo)
        {
            go.SetActive(false);
        }
    }
}
