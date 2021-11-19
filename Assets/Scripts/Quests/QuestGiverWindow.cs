using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestGiverWindow : Window
{
    private static QuestGiverWindow instance;
    public static QuestGiverWindow Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<QuestGiverWindow>();
            }

            return instance;
        }
    }

    private QuestGiver questGiver;

    [SerializeField]
    private GameObject questPrefab;

    [SerializeField]
    private Transform questArea;

    [SerializeField]
    private GameObject backBtn, AcceptBtn, completeBtn, questDescription;

    private List<GameObject> questgos = new List<GameObject>();

    private Quest selectedQuest;

    public void ShowQuests(QuestGiver questGiver)
    {
        this.questGiver = questGiver;

        foreach (var go in questgos)
        {
            Destroy(go);
        }

        questArea.gameObject.SetActive(true);
        questDescription.SetActive(false);

        foreach (var quest in questGiver.Quests)
        {
            if(quest != null && quest.Title != null)
            {
                GameObject go = Instantiate(questPrefab, questArea);
                go.GetComponent<Text>().text = "lv: " + quest.Level + " | " + quest.Title + "<color=#ffbb04>!</color>";

                go.GetComponent<QGQuestScript>().quest = quest;

                questgos.Add(go);

                if (QuestLog.Instance.HasQuest(quest) && quest.IsComplete)
                {
                    go.GetComponent<Text>().text = quest.Title + "<color=#ffbb04>?</color>";
                }
                else if (QuestLog.Instance.HasQuest(quest))
                {
                    Color c = go.GetComponent<Text>().color;
                    c.a = 0.5f;

                    go.GetComponent<Text>().color = c;
                    go.GetComponent<Text>().text = quest.Title + "<color=#c0c0c0ff>?</color>";
                }
            }
        }
    }

    public override void Open(NPC npc)
    {
        base.Open(npc);
        ShowQuests((npc as QuestGiver));
    }

    public void ShowQuestInfo(Quest quest)
    {
        selectedQuest = quest;

        if (QuestLog.Instance.HasQuest(quest) && quest.IsComplete)
        {
            AcceptBtn.SetActive(false);

            completeBtn.SetActive(true);
        }
        else if (!QuestLog.Instance.HasQuest(quest))
        {
            AcceptBtn.SetActive(true);
        }

        backBtn.SetActive(true);       

        questArea.gameObject.SetActive(false);
        questDescription.SetActive(true);

        string objectives = string.Empty;

        if (quest.CollectObjectives.Length > 0)
        {
            objectives += "\nCollect Objectives\n";
            foreach (Objective obj in quest.CollectObjectives)
            {
                objectives += obj.Type + ": " + obj.CurrentAmount + " / " + obj.Amount + "\n";
            }
        }

        if (quest.KillObjectives.Length > 0)
        {
            objectives += "\nKill Objectives\n";

            foreach (Objective obj in quest.KillObjectives)
            {
                objectives += obj.Type + ": " + obj.CurrentAmount + " / " + obj.Amount + "\n";
            }

        }

        string title = quest.Title;

        string description = quest.Description;

        questDescription.GetComponent<Text>().text = string.Format("<b>{0}</b>\n\n{1}\n{2}", title, description, objectives);

    }

    public void Back()
    {
        backBtn.SetActive(false);
        AcceptBtn.SetActive(false);

        completeBtn.SetActive(false);

        ShowQuests(questGiver);
    }

    public void Accept()
    {
        QuestLog.Instance.AcceptQuest(selectedQuest);
        Back();
    }

    public override void Close()
    {
        completeBtn.SetActive(false);
        base.Close();
    }


    public void CompleteQuest()
    {
        if (selectedQuest.IsComplete)
        {
            for (int i = 0; i < questGiver.Quests.Length; i++)
            {
                if(selectedQuest == questGiver.Quests[i])
                {
                    questGiver.CompletedQuests.Add(selectedQuest.Title);
                    questGiver.Quests[i] = null;
                    selectedQuest.questGiver.UpdateQuestStatus();
                }
            }

            foreach (var objective in selectedQuest.CollectObjectives)
            {
                InventoryScript.Instance.itemCountChangedEvent -= new ItemCountChanged(objective.UpdateItemCount);
                objective.Complete();
            }
            foreach (var objective in selectedQuest.KillObjectives)
            {
                GameManager.Instance.killConfirmedEvent -= new KillConfirmed(objective.UpdateKillCount);
            }

            Player.Instance.GainXP(XPmanager.CalculateXP(selectedQuest));

            QuestLog.Instance.RemoveQuest(selectedQuest.questScript);
            Back();
        }
    }
}
