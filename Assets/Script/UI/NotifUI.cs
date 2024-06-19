using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotifUI : MonoBehaviour, IQuest2Listener
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI Name, Info;
    [SerializeField] private GameObject Bg;

    private CanvasGroup bgCanvasGroup;

    private void Awake()
    {
        QuestEventManager.Instance.RegisterListener(this);
        bgCanvasGroup = Bg.GetComponent<CanvasGroup>();
        if (bgCanvasGroup == null)
        {
            bgCanvasGroup = Bg.AddComponent<CanvasGroup>();
        }
    }

    private void OnDestroy()
    {
        QuestEventManager.Instance.UnregisterListener(this);
    }

    // Implement IQuest2Listener
    public void OnQuest2Started() { }

    public void OnQuest2Completed()
    {
        BgFadeOut();
    }

    public void OnQuest2ProgressUpdated(int percentage)
    {       
        BgFadeIn();
        Name.text = "Distance Travelled:";
        Info.text = $"{percentage}%";
    }

    private void BgFadeIn()
    {
        Bg.SetActive(true);
        bgCanvasGroup.DOFade(1, 2f); 
    }

    private void BgFadeOut()
    {
        bgCanvasGroup.DOFade(0, 2f).OnComplete(() => Bg.SetActive(false)); 
    }
}
