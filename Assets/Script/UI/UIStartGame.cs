using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIStartGame : MonoBehaviour
{

    [SerializeField] private GameObject[] AllUI;
    [SerializeField] private GameObject UIStart;
    [SerializeField] private Button StartBt;
    [SerializeField] Player player;
    private Animator StartAnimator;

    private void Start()
    {
        player.canWASD = false;
        DisAbleAllUI();
        UIStart.SetActive(true);
        StartAnimator = UIStart.GetComponent<Animator>();
        StartAnimator.enabled = false;
        StartBt.onClick.AddListener(LogicAfterClickBt);
    }


    private void LogicAfterClickBt()
    {
        StartAnimator.enabled = true;
        player.canWASD = true;
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
        AllUI[3].SetActive(true);
    }
}
