using System.Collections;
using TMPro;
using UnityEngine;

public class ChatBot : MonoBehaviour
{
    private BotChatText[] BotChatText;
    [SerializeField] GameObject BotUI;
    [SerializeField] TextMeshProUGUI ChatText;

    private void Start()
    {
        BotUI.SetActive(true);
        BotChatText = Resources.LoadAll<BotChatText>("BotChatText");
        StartCoroutine(DisplayTextOverTime(BotChatText[1].text));
    }

    private void Update()
    {
        for (int i = 0; i < BotChatText.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                i++;
                StartCoroutine(DisplayTextOverTime(BotChatText[i].text));
            }
        }
    }


    private IEnumerator DisplayTextOverTime(string fullText)
    {
        ChatText.text = "";
        foreach (char c in fullText)
        {
            ChatText.text += c;
            yield return new WaitForSeconds(0.001f);
        }
    }
}
