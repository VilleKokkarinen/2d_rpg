using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : NPC
{
    [SerializeField]
    private Quest[] quests;

    [SerializeField]
    private Sprite question, questionSilver, exclamation;

    public Quest[] Quests { get => quests; }
    public int ID { get => Id; }
    public List<string> CompletedQuests { get => completedQuests;
        set
        {
            completedQuests = value;
            foreach (var title in completedQuests)
            {
                for(int i = 0; i < quests.Length; i++)
                {
                    if (quests[i] != null && quests[i].Title == title)
                    {
                        quests[i] = null;
                    }
                }
            }
        }
    }

    [SerializeField]
    private SpriteRenderer statusRenderer;

    [SerializeField]
    private int Id;

    private List<string> completedQuests = new List<string>();

    [SerializeField]
    private SpriteRenderer minimapRenderer;

    private void Start()
    {
        foreach (var quest in quests)
        {
            quest.questGiver = this;
        }
    }

    public void UpdateQuestStatus()
    {
        int count = 0;

        foreach (var quest in quests)
        {
            if(quest!= null)
            {
                if (quest.IsComplete && QuestLog.Instance.HasQuest(quest))
                {
                    statusRenderer.sprite = question;
                    minimapRenderer.sprite = question;
                    break;
                }
                else if (!QuestLog.Instance.HasQuest(quest)){
                    statusRenderer.sprite = exclamation;
                    minimapRenderer.sprite = exclamation;
                    break;
                }
                else if (!quest.IsComplete && QuestLog.Instance.HasQuest(quest))
                {
                    statusRenderer.sprite = questionSilver;
                    minimapRenderer.sprite = questionSilver;
                    break;
                }
            }
            else
            {
                count++;

                if(count == quests.Length)
                {
                    statusRenderer.enabled = false;
                    minimapRenderer.enabled = false;
                }
            }
        }
    }
}
