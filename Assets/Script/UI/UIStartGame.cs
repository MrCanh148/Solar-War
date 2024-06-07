using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIStartGame : MonoBehaviour
{
    [SerializeField] private GameObject[] AllUI;
    [SerializeField] private GameObject UIStart, AllInOne, Hole, OneInAll, Player;
    [SerializeField] private Button StartBt;
    [SerializeField] Player player;
    private Animator StartAnimator;
    private bool StartExplore = false;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        player.canWASD = false;
        DisAbleAllUI();
        UIStart.SetActive(true);
        spriteRenderer = Player.GetComponent<SpriteRenderer>();
        StartAnimator = UIStart.GetComponent<Animator>();
        StartAnimator.enabled = false;
        StartBt.onClick.AddListener(LogicAfterClickBt);
    }

    private void Update()
    {
        if (player.rb.mass >= 1000000)
        {
            StartExplore = true;
            DisAbleAllUI();
        }
         

        if (StartExplore)
        {
            
            StartCoroutine(ChaChaBoomBoom());
        }
  
    } 

    private void LogicAfterClickBt()
    {
        StartAnimator.enabled = true;
        StartCoroutine(RunAnimator());
    }

    private void DisAbleAllUI ()
    {
        foreach (GameObject go in AllUI)
        {
            go.SetActive(false);
        }
    }

    private IEnumerator RunAnimator()
    {
        yield return new WaitForSeconds(1f);
        StartExplore = true;
        UIStart.SetActive(false);
    }

    private IEnumerator ChaChaBoomBoom()
    {
        StartExplore = false;
        AllInOne.SetActive(true);
        yield return new WaitForSeconds(2f);
        spriteRenderer.enabled = false;
        Hole.SetActive(true);
        yield return new WaitForSeconds(3f);
        AllInOne.SetActive(false);
        player.characterType = CharacterType.NeutronStar;
        OneInAll.SetActive(true);
        yield return new WaitForSeconds(1f);
        Hole.SetActive(false);
        player.rb.mass = 2f;
        yield return new WaitForSeconds(5f);
        spriteRenderer.enabled = true;
        player.canWASD = true;
        AllUI[3].SetActive(true);
        OneInAll.SetActive(false);
        player.characterType = CharacterType.Asteroid;
        player.generalityType = GeneralityType.Asteroid;
    }
}
