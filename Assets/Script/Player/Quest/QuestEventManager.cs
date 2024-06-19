using System.Collections.Generic;
using UnityEngine;

public class QuestEventManager : MonoBehaviour
{
    private static QuestEventManager _instance;
    public static QuestEventManager Instance => _instance;

    private List<IQuestEvent> questEventListeners = new List<IQuestEvent>();

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void RegisterQuestEvent(IQuestEvent questEvent)
    {
        if (!questEventListeners.Contains(questEvent))
        {
            questEventListeners.Add(questEvent);
        }
    }

    public void UnregisterQuestEvent(IQuestEvent questEvent)
    {
        if (questEventListeners.Contains(questEvent))
        {
            questEventListeners.Remove(questEvent);
        }
    }

    public void NotifyQuestEnter(Quest quest)
    {
        foreach (var listener in questEventListeners)
        {
            listener.OnQuestEnter(quest);
        }
    }
}
