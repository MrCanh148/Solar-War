using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest4 : MonoBehaviour
{
    private bool isShowed = false;

    private void Start()
    {
        Debug.Log(" Quest 4 is coming");
    }

    private void Update()
    {
        if (!isShowed)
        {
            isShowed = true;
            QuestEventManager.Instance.NotifyQuest4Completed();
        }
     
    }
}
