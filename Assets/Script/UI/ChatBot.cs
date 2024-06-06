using System.Collections;
using TMPro;
using UnityEngine;

public class ChatBot : MonoBehaviour
{
    private BotChatText[] BotChatText;
    [SerializeField] private GameObject TextDisplay, PressEnterText, BotUI, InGameUI;
    [SerializeField] TextMeshProUGUI ChatText;
    [SerializeField] private float TimeShowText = 0.01f;
    [SerializeField] private float TimeDelayShowGameObjectText = 1f;

    private int currentIndex = 0; 
    private bool isDisplayingText = false; 

    private void Start()
    {
        StartCoroutine(GameObjectTextDisPlayer());
        BotChatText = Resources.LoadAll<BotChatText>("BotChatText");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (!isDisplayingText)
            {
                if (currentIndex < BotChatText.Length - 1)
                {
                    currentIndex++;
                    StartCoroutine(DisplayTextOverTime(BotChatText[currentIndex].text));
                }
                else if (currentIndex == BotChatText.Length - 1)
                    BotUI.SetActive(false);
            }
        }

        if (currentIndex == 2)
            InGameUI.SetActive(true);
    }

    private IEnumerator DisplayTextOverTime(string fullText)
    {
        isDisplayingText = true;
        ChatText.text = "";
        foreach (char c in fullText)
        {
            ChatText.text += c;
            yield return new WaitForSeconds(TimeShowText);
        }
        isDisplayingText = false;
    }

    private IEnumerator GameObjectTextDisPlayer()
    {
        TextDisplay.SetActive(false);
        yield return new WaitForSeconds(TimeDelayShowGameObjectText);
        TextDisplay.SetActive(true);
        PressEnterText.SetActive(true);
        if (BotChatText.Length >= 0)
            StartCoroutine(DisplayTextOverTime(BotChatText[currentIndex].text));
    }
}
