using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestScript : MonoBehaviour
{
    public Quest quest { get; set; }

    private bool MarkedComplete = false;

    public void Select()
    {
        GetComponent<Text>().color = Color.red;
        QuestLog.Instance.ShowDescription(quest);


    }

    public void DeSelect()
    {
        GetComponent<Text>().color = Color.white;

    }
    public void IsComplete()
    {
        if (quest.IsComplete && MarkedComplete == false)
        {
            MarkedComplete = true;
            GetComponent<Text>().text += "lv: " + quest.Level + " | " + " (C)";

            MessageFeedManager.Instance.WriteMessage(string.Format("{0} (C)", quest.Title));
        }
        else if(!quest.IsComplete)
        {
            MarkedComplete = false;
            GetComponent<Text>().text = "lv: " + quest.Level + " | " + quest.Title;
        }
    }
}
