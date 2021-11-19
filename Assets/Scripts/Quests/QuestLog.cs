using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestLog : MonoBehaviour
{
    private static QuestLog instance;
    public static QuestLog Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<QuestLog>();
            }

            return instance;
        }
    }

    public List<Quest> Quests { get => quests; }

    [SerializeField]
    private GameObject questPrefab;

    [SerializeField]
    private Transform questList;

    private Quest selected;

    [SerializeField]
    private Text selectedDescription;

    private List<QuestScript> questScripts = new List<QuestScript>();

    private List<Quest> quests = new List<Quest>();

    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    private Text questCountText;

    [SerializeField]
    private int maxCount;

    private int currentCount;

    private void Start()
    {
        questCountText.text = currentCount + "/" + maxCount;
    }
    
    public void AcceptQuest(Quest quest)
    {
        if (currentCount < maxCount)
        {
            currentCount++;
            questCountText.text = currentCount + "/" + maxCount;

            foreach (var o in quest.CollectObjectives)
            {
                InventoryScript.Instance.itemCountChangedEvent += new ItemCountChanged(o.UpdateItemCount);
                o.UpdateItemCount(); // check if quest is completed already
            }
            foreach (var o in quest.KillObjectives)
            {
                GameManager.Instance.killConfirmedEvent += new KillConfirmed(o.UpdateKillCount);
            }

            quests.Add(quest);

            GameObject go = Instantiate(questPrefab, questList);

            QuestScript qs = go.GetComponent<QuestScript>();

            quest.questScript = qs;
            qs.quest = quest;

            questScripts.Add(qs);

            go.GetComponent<Text>().text = quest.Title;


            CheckCompletion();
        }
    }

    public void UpdateSelected()
    {
        ShowDescription(selected);
    }

    public void ShowDescription(Quest quest)
    {
        if(quest != null)
        {
            if (selected != null && selected != quest)
            {
                selected.questScript.DeSelect();
            }

            string objectives = string.Empty;

            if(quest.CollectObjectives.Length > 0)
            {
                objectives += "\nCollect Objectives\n";
                foreach (Objective obj in quest.CollectObjectives)
                {
                    objectives += obj.Type + ": " + obj.CurrentAmount + " / " + obj.Amount + "\n";
                }
            }

            if(quest.KillObjectives.Length > 0)
            {
                objectives += "\nKill Objectives\n";

                foreach (Objective obj in quest.KillObjectives)
                {
                    objectives += obj.Type + ": " + obj.CurrentAmount + " / " + obj.Amount + "\n";
                }

            }

            selected = quest;

            string title = quest.Title;

            string description = quest.Description;

            selectedDescription.text = string.Format("<b>{0}</b>\n\n{1}\n{2}", title, description, objectives);

        }
    }

    public void CheckCompletion()
    {
        foreach (var qs in questScripts)
        {
            qs.quest.questGiver.UpdateQuestStatus();
            qs.IsComplete();
        }
    }

    public void RemoveQuest(QuestScript qs)
    {
        questScripts.Remove(qs);
        Destroy(qs.gameObject);
        quests.Remove(qs.quest);
        selectedDescription.text = string.Empty;
        selected = null;
        qs.quest.questGiver.UpdateQuestStatus();
        qs = null;

        currentCount--;
        questCountText.text = currentCount + "/" + maxCount;
    }

    public void AbandonQuest()
    {
        foreach (var objective in selected.CollectObjectives)
        {
            InventoryScript.Instance.itemCountChangedEvent -= new ItemCountChanged(objective.UpdateItemCount);
        }
        foreach (var objective in selected.KillObjectives)
        {
            GameManager.Instance.killConfirmedEvent -= new KillConfirmed(objective.UpdateKillCount);
        }

        RemoveQuest(selected.questScript);
    }

    public bool HasQuest(Quest quest)
    {
        return quests.Exists(x => x.Title == quest.Title);
    }

    public void OpenClose()
    {
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;

        canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true;
    }

    public void Close()
    {
        canvasGroup.alpha = 0;

        canvasGroup.blocksRaycasts = false;
    }


}
