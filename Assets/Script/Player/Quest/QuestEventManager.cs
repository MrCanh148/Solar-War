using System.Collections.Generic;
using UnityEngine;

public class QuestEventManager : MonoBehaviour
{
    private static QuestEventManager _instance;
    public static QuestEventManager Instance => _instance;

    private List<IQuestEvent> questEventListeners = new List<IQuestEvent>();
    private List<IQuest2Listener> quest2Listeners = new List<IQuest2Listener>();

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

    // For IQuestEvent
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

    // For IQuest2Listener
    public void RegisterListener(IQuest2Listener listener)
    {
        if (!quest2Listeners.Contains(listener))
        {
            quest2Listeners.Add(listener);
        }
    }

    public void UnregisterListener(IQuest2Listener listener)
    {
        if (quest2Listeners.Contains(listener))
        {
            quest2Listeners.Remove(listener);
        }
    }

    public void NotifyQuestStarted()
    {
        foreach (var listener in quest2Listeners)
        {
            listener.OnQuest2Started();
        }
    }

    public void NotifyQuestCompleted()
    {
        foreach (var listener in quest2Listeners)
        {
            listener.OnQuest2Completed();
        }
    }

    public void NotifyQuestProgressUpdated(int percentage)
    {
        foreach (var listener in quest2Listeners)
        {
            listener.OnQuest2ProgressUpdated(percentage);
        }
    }
}
