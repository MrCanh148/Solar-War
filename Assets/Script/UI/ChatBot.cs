using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections;

public class ChatBot : MonoBehaviour, IQuest2Listener, IQuest1Listenner, IQuest3Listenner, IQuest4Listenner
{
    private Dictionary<string, List<BotChatText>> botChatTextsCache = new Dictionary<string, List<BotChatText>>();

    [SerializeField] private GameObject TextDisplay, PressEnterText, BotUI, StatePlayerUI;
    [SerializeField] TextMeshProUGUI ChatText;
    [SerializeField] private float TimeShowText = 0.01f;
    [SerializeField] private float TimeDelayShowGameObjectText = 1f;
    [SerializeField] private Button TaptoClose;

    private List<BotChatText> BotChatText;
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

        LoadBotChatTexts("BotChatText/BotStartGame");
        LoadBotChatTexts("BotChatText/BotSurvival");
        LoadBotChatTexts("BotChatText/BotQuest1");
        LoadBotChatTexts("BotChatText/BotQuest2");
        LoadBotChatTexts("BotChatText/BotQuest3");
        LoadBotChatTexts("BotChatText/BotQuest4");
    }

    private void LoadBotChatTexts(string label)
    {
        Addressables.LoadAssetsAsync<BotChatText>(label, null).Completed += (AsyncOperationHandle<IList<BotChatText>> handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                botChatTextsCache[label] = new List<BotChatText>(handle.Result);

                if (label == "BotChatText/BotStartGame" && GameManager.instance.currentGameMode == GameMode.Normal)
                {
                    BotChatText = botChatTextsCache[label];
                    isInitialBotChat = true;
                }
                else if (label == "BotChatText/BotSurvival" && GameManager.instance.currentGameMode == GameMode.Survival)
                {
                    BotChatText = botChatTextsCache[label];
                    isInitialBotChat = true;
                }
            }
        };
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && canPressEnter)
        {
            OnReturnKeyPressed();
        }

        if (currentIndex == 2 || !isInitialBotChat || GameManager.instance.currentGameMode == GameMode.Survival)
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
            yield return new WaitForSecondsRealtime(TimeShowText);
        }
        canPressEnter = true;
        isDisplayingText = false;
    }

    private IEnumerator GameObjectTextDisplayer()
    {
        TextDisplay.SetActive(false);
        yield return new WaitForSecondsRealtime(TimeDelayShowGameObjectText);
        TextDisplay.SetActive(true);
        PressEnterText.SetActive(true);

        if (BotChatText.Count > 0)
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
                if (currentIndex < BotChatText.Count - 1)
                {
                    currentIndex++;
                    StartDisplayTextOverTime(BotChatText[currentIndex].text);
                }
                else if (currentIndex == BotChatText.Count - 1)
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

    private void HandleQuestEvent(string questKey, int textIndex)
    {
        gameObject.SetActive(true);
        if (botChatTextsCache.TryGetValue(questKey, out List<BotChatText> botChatTexts))
        {
            BotChatText = botChatTexts;
            isInitialBotChat = false;
            StartDisplayTextOverTime(BotChatText[textIndex].text);
        }
    }

    // Implement IQuest2Listener
    public void OnQuest2Started()
    {
        HandleQuestEvent("BotChatText/BotQuest2/0", 0);
    }

    public void OnQuest2Completed()
    {
        HandleQuestEvent("BotChatText/BotQuest2/1", 1);
    }

    public void OnQuest2ProgressUpdated(int a) { }

    // Implement IQuest1Listener
    public void OnQuest1Started()
    {
        HandleQuestEvent("BotChatText/BotQuest1/0", 0);
    }

    public void OnQuest1Completed()
    {
        HandleQuestEvent("BotChatText/BotQuest1/1", 1);
    }

    // Implement IQuest3Listener
    public void OnQuest3Started()
    {
        HandleQuestEvent("BotChatText/BotQuest3/0", 0);
    }

    public void OnQuest3Completed()
    {
        HandleQuestEvent("BotChatText/BotQuest3/1", 1);
    }

    // Implement IQuest4Listener
    public void OnQuest4Started()
    {
        HandleQuestEvent("BotChatText/BotQuest4/0", 0);
    }

    public void OnQuest4Completed()
    {
        HandleQuestEvent("BotChatText/BotQuest4/1", 1);
    }
}
