using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest1 : MonoBehaviour
{
    [SerializeField] private GameObject AlienPrefab;
    private bool isShowed = false;

    private void Start()
    {
        Debug.Log(" Quest 1 is coming");
    }

    private void Update()
    {
        if (!isShowed)
        {
            isShowed = true;
            QuestEventManager.Instance.NotifyQuest1Completed();
        }
   
    }

}
