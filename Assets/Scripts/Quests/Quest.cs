using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Quest
{
    [SerializeField]
    private string title;

    [SerializeField]
    private string description;

    [SerializeField]
    private CollectObjective[] collectObjectives;

    [SerializeField]
    private KillObjective[] killObjectives;

    public QuestScript questScript { get; set; }

    public QuestGiver questGiver { get; set; }

    public string Title { get => title; }
    public string Description { get => description; set => description = value; }
  

    [SerializeField]
    private int level, xp;

    public int Level { get => level; }
    public int Xp { get => xp; }

    public CollectObjective[] CollectObjectives { get => collectObjectives; }
    public KillObjective[] KillObjectives { get => killObjectives; set => killObjectives = value; }

    public bool IsComplete
    {
        get
        {
            foreach (var o in collectObjectives)
            {
                if (!o.IsComplete)
                {
                    return false;
                }
            }
            foreach (var o in killObjectives)
            {
                if (!o.IsComplete)
                {
                    return false;
                }
            }
            return true;
        }

    }

}

[Serializable]
public abstract class Objective
{
    [SerializeField]
    private int amount;
    
    private int currentAmount;

    [SerializeField]
    private string type;

    public int Amount { get => amount; }
    public int CurrentAmount { get => currentAmount; set => currentAmount = value; }
    public string Type { get => type; }

    public bool IsComplete
    {
        get
        {
            return CurrentAmount >= Amount;
        }
    }
}

[Serializable]
public class CollectObjective: Objective
{
    public void UpdateItemCount(Item item)
    {
        if (Type.ToLower() == item.Title.ToLower())
        {
            CurrentAmount = InventoryScript.Instance.GetItemCount(item.Title);

            if(CurrentAmount <= Amount)
            {
                MessageFeedManager.Instance.WriteMessage(string.Format("{0}: {1}/{2}", item.Title, CurrentAmount, Amount));
                QuestLog.Instance.UpdateSelected();
                QuestLog.Instance.CheckCompletion();
            }

        
        }
    }

    public void UpdateItemCount()
    {
        CurrentAmount = InventoryScript.Instance.GetItemCount(Type);
        QuestLog.Instance.UpdateSelected();
        QuestLog.Instance.CheckCompletion();
    }

    public void Complete()
    {
        Stack<Item> items = InventoryScript.Instance.GetItems(Type, CurrentAmount);

        foreach(Item item in items)
        {
            item.Remove();
        }
    }
}

[Serializable]
public class KillObjective : Objective
{
    public void UpdateKillCount(Character character)
    {
        if (Type == character.Type)
        {
            CurrentAmount++;

            if (CurrentAmount <= Amount)
            {
                MessageFeedManager.Instance.WriteMessage(string.Format("{0}: {1}/{2}", character.Type, CurrentAmount, Amount));
                QuestLog.Instance.UpdateSelected();
                QuestLog.Instance.CheckCompletion();
            }

        
        }
    }
}