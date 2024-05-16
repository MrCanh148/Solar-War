using System.Collections;
using TMPro;
using UnityEngine;

public class ChatBot : MonoBehaviour
{
    private BotChatText[] BotChatText;
    [SerializeField] GameObject BotUI;
    [SerializeField] TextMeshProUGUI ChatText;
    [SerializeField] private float TimeShowText = 0.01f;

    private int currentIndex = 0; 
    private bool isDisplayingText = false; 


    private void Start()
    {
        BotUI.SetActive(true);
        BotChatText = Resources.LoadAll<BotChatText>("BotChatText");
        if (BotChatText.Length > 0)
        {
            StartCoroutine(DisplayTextOverTime(BotChatText[currentIndex].text));
        }
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
}
