using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatBot : MonoBehaviour, IQuest2Listener, IQuest1Listenner, IQuest3Listenner, IQuest4Listenner
{
    private BotChatText[] BotChatText;
    [SerializeField] private GameObject TextDisplay, PressEnterText, BotUI, StatePlayerUI;
    [SerializeField] TextMeshProUGUI ChatText;
    [SerializeField] private float TimeShowText = 0.01f;
    [SerializeField] private float TimeDelayShowGameObjectText = 1f;
    [SerializeField] private Button TaptoClose;

    private int currentIndex = 0;
    private bool isDisplayingText = false;
    private bool displayFullTextImmediately = false;
    private bool canPressEnter = false;
    private bool isInitialBotChat = true;

    private Coroutine displayCoroutine;

    private void Awake()
    {
        QuestEventManager.Instance.RegisterListener(this);
        QuestEventManager.Instance.Register1Listener(this);
        QuestEventManager.Instance.Register3Listener(this);
        QuestEventManager.Instance.Register4Listener(this);
    }

    private void OnDestroy()
    {
        QuestEventManager.Instance.UnregisterListener(this);
        QuestEventManager.Instance.Unregister1Listener(this);
        QuestEventManager.Instance.Unregister3Listener(this);
        QuestEventManager.Instance.Unregister4Listener(this);
    }

    private void Start()
    {
        TaptoClose.onClick.AddListener(OnReturnKeyPressed);
        StartCoroutine(GameObjectTextDisplayer());
        BotChatText = Resources.LoadAll<BotChatText>("BotChatText/BotStartGame");
        isInitialBotChat = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && canPressEnter)
        {
            OnReturnKeyPressed();
        }

        if (currentIndex == 2 || !isInitialBotChat)
        {
            StatePlayerUI.SetActive(true);
        }
    }

    private IEnumerator DisplayTextOverTime(string fullText)
    {
        AudioManager.instance.PlaySFX("Robot");
        isDisplayingText = true;
        displayFullTextImmediately = false;
        ChatText.text = "";

        foreach (char c in fullText)
        {
            if (displayFullTextImmediately)
            {
                ChatText.text = fullText;
                break;
            }

            ChatText.text += c;
            yield return new WaitForSeconds(TimeShowText);
        }
        canPressEnter = true;
        isDisplayingText = false;
    }

    private IEnumerator GameObjectTextDisplayer()
    {
        TextDisplay.SetActive(false);
        yield return new WaitForSeconds(TimeDelayShowGameObjectText);
        TextDisplay.SetActive(true);
        PressEnterText.SetActive(true);

        if (BotChatText.Length > 0)
        {
            StartDisplayTextOverTime(BotChatText[currentIndex].text);
        }
    }

    private void StartDisplayTextOverTime(string text)
    {
        if (displayCoroutine != null)
        {
            StopCoroutine(displayCoroutine);
        }
        displayCoroutine = StartCoroutine(DisplayTextOverTime(text));
    }

    private void OnReturnKeyPressed()
    {
        if (isDisplayingText)
        {
            displayFullTextImmediately = true;
        }
        else
        {
            if (isInitialBotChat)
            {
                if (currentIndex < BotChatText.Length - 1)
                {
                    currentIndex++;
                    StartDisplayTextOverTime(BotChatText[currentIndex].text);
                }
                else if (currentIndex == BotChatText.Length - 1)
                {
                    BotUI.SetActive(false);
                    gameObject.SetActive(false);
                }
            }
            else
            {
                BotUI.SetActive(false);
                gameObject.SetActive(false);
            }
        }
    }

    // ========================================Implement IQuest2Listener
    public void OnQuest2Started()
    {
        gameObject.SetActive(true);
        BotChatText = Resources.LoadAll<BotChatText>("BotChatText/BotQuest2");
        isInitialBotChat = false;
        StartDisplayTextOverTime(BotChatText[0].text);
    }

    public void OnQuest2Completed()
    {
        gameObject.SetActive(true);
        BotChatText = Resources.LoadAll<BotChatText>("BotChatText/BotQuest2");
        isInitialBotChat = false;
        StartDisplayTextOverTime(BotChatText[1].text);
    }

    public void OnQuest2ProgressUpdated(int a) { }

    // ========================================Implement IQuest1Listener
    public void OnQuest1Started()
    {
        gameObject.SetActive(true);
        BotChatText = Resources.LoadAll<BotChatText>("BotChatText/BotQuest1");
        isInitialBotChat = false;
        StartDisplayTextOverTime(BotChatText[0].text);
    }

    public void OnQuest1Completed()
    {
        gameObject.SetActive(true);
        BotChatText = Resources.LoadAll<BotChatText>("BotChatText/BotQuest1");
        isInitialBotChat = false;
        StartDisplayTextOverTime(BotChatText[1].text);
    }

    // ========================================Implement IQuest3Listener
    public void OnQuest3Started()
    {
        gameObject.SetActive(true);
        BotChatText = Resources.LoadAll<BotChatText>("BotChatText/BotQuest3");
        isInitialBotChat = false;
        StartDisplayTextOverTime(BotChatText[0].text);
    }

    public void OnQuest3Completed()
    {
        gameObject.SetActive(true);
        BotChatText = Resources.LoadAll<BotChatText>("BotChatText/BotQuest3");
        isInitialBotChat = false;
        StartDisplayTextOverTime(BotChatText[1].text);
    }

    // ========================================Implement IQuest4Listener
    public void OnQuest4Started()
    {
        gameObject.SetActive(true);
        BotChatText = Resources.LoadAll<BotChatText>("BotChatText/BotQuest4");
        isInitialBotChat = false;
        StartDisplayTextOverTime(BotChatText[0].text);
    }

    public void OnQuest4Completed()
    {
        gameObject.SetActive(true);
        BotChatText = Resources.LoadAll<BotChatText>("BotChatText/BotQuest4");
        isInitialBotChat = false;
        StartDisplayTextOverTime(BotChatText[1].text);
    }
}
