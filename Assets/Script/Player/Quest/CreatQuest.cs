using UnityEngine;
using System.Collections.Generic;

public class CreateQuest : MonoBehaviour, IQuestEvent
{
    [SerializeField] private LogicQuestPointer _quest;
    [SerializeField] private GameObject[] questPrefabs;
    [SerializeField] private GameObject Player;

    private List<GameObject> activeQuests = new List<GameObject>();

    private void Start()
    {
        CreateUniquePoints();
        RegisterToEvent();
    }

    private void CreateUniquePoints()
    {
        List<GameObject> usedPrefabs = new List<GameObject>();
        for (int i = 0; i < 4; i++)
        {
            GameObject selectedPrefab = GetUniquePrefab(usedPrefabs);

            usedPrefabs.Add(selectedPrefab);
            Vector3 position = GetPosition(i);
            GameObject pointerObject = Instantiate(selectedPrefab, position, Quaternion.identity);
            _quest.CreatePoint(position);
            activeQuests.Add(pointerObject);
        }
    }

    private GameObject GetUniquePrefab(List<GameObject> usedPrefabs)
    {
        List<GameObject> unusedPrefabs = new List<GameObject>();

        foreach (GameObject prefab in questPrefabs)
        {
            if (!usedPrefabs.Contains(prefab))
            {
                unusedPrefabs.Add(prefab);
            }
        }

        if (unusedPrefabs.Count > 0)
        {
            int randomIndex = Random.Range(0, unusedPrefabs.Count);
            return unusedPrefabs[randomIndex];
        }
        else
        {
            return null;
        }
    }

    private Vector3 GetPosition(int index)
    {
        float x = Random.Range(50, 70);
        Vector3[] positions = new Vector3[]
        {
            new Vector3(x, x) + Player.transform.position,
            new Vector3(-x, x) + Player.transform.position,
            new Vector3(x, -x) + Player.transform.position,
            new Vector3(-x, -x) + Player.transform.position
        };

        return positions[index];
    }

    private void RegisterToEvent()
    {
        var questEvent = FindObjectOfType<QuestEventManager>();
        if (questEvent != null)
        {
            questEvent.RegisterQuestEvent(this);
        }
    }

    public void OnQuestEnter(Quest quest)
    {
        foreach (var questObject in activeQuests)
        {
            if (questObject != quest.gameObject)
            {
                Destroy(questObject); 
            }
        }

        activeQuests.Clear();
        _quest.RemoveAllPoints();
    }

}
