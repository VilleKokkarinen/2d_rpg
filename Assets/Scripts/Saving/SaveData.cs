using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SaveData
{
    public PlayerData playerData { get; set; }
    public List<ChestData> chestData { get; set; }
    public InventoryData inventoryData { get; set; }
    public List<EquipmentData> equipmentData { get; set; }    
    public List<QuestData> questData { get; set; }
    public List<QuestGiverData> questGiverData { get; set; }
    public List<ActionButtonData> actionButtonData { get; set; }

    public string Scene { get; set; }
    public DateTime Timestamp { get; set; }
    public SaveData()
    {
        chestData = new List<ChestData>();
        equipmentData = new List<EquipmentData>();
        questData = new List<QuestData>();
        questGiverData = new List<QuestGiverData>();
        actionButtonData = new List<ActionButtonData>();

        inventoryData = new InventoryData(0);
        Timestamp = DateTime.Now;
    }


}

[Serializable]
public class PlayerData
{
    public int Level { get; set; }
    public float XP { get; set; }
    public float MaxXP { get; set; }
    public float Health { get; set; }
    public float MaxHealth { get; set; }
    public float Mana { get; set; }
    public float MaxMana { get; set; }
    public float PositionX { get; set; }
    public float PositionY { get; set; }

    public PlayerData(int level, float xp, float maxXP, float health, float maxHealth, float mana, float maxMana, Vector2 position )
    {
        Level = level;
        XP = xp;
        MaxXP = maxXP;
        Health = health;
        MaxHealth = maxHealth;
        Mana = mana;
        MaxMana = maxMana;
        PositionX = position.x;
        PositionY = position.y;
    }   
}

[Serializable]
public class ItemData
{
    public string Title { get; set; }
    public int StackCount { get; set; }
    public int SlotIndex { get; set; }


    public ItemData(string title, int stackcount = 0, int slotindex = 0)
    {
        Title = title;
        StackCount = stackcount;
        SlotIndex = slotindex;
    }
}

[Serializable]
public class ChestData
{
    public string Name { get; set; }
    public List<ItemData> Items { get; set; }

    public ChestData(string name)
    {
        Name = name;
        Items = new List<ItemData>();
    }
}

[Serializable]
public class InventoryData
{
    public BagData Inventory { get; set; }
    public List<ItemData> Items { get; set; }

    public InventoryData(int count)
    {
        Inventory = new BagData(count);
        Items = new List<ItemData>();
    }
}

[Serializable]
public class BagData
{
    public int SlotCount { get; set; }

    public BagData(int count)
    {
        SlotCount = count;
    }
}

[Serializable]
public class EquipmentData
{
    public string Title { get; set; }
    public string Type { get; set; }

    public EquipmentData(string title, string type)
    {
        Title = title;
        Type = type;
    }
}

[Serializable]
public class ActionButtonData
{
    public string Action { get; set; }

    public bool IsItem { get; set; }

    public int Index { get; set; }
    public ActionButtonData(string action, bool isItem, int index)
    {
        Action = action;
        IsItem = isItem;
        Index = index;
    }
}

[Serializable]
public class QuestData
{
    public string Title { get; set; }
    public string Description { get; set; }
    public CollectObjective[] CollectObjectives { get; set; }
    public KillObjective[] KillObjectives { get; set; }
    public int QuestGiverId { get; set; }
    public QuestData(string title, string description, CollectObjective[] collectObjectives, KillObjective[] killObjectives, int questGiverId)
    {
        Title = title;
        Description = description;
        CollectObjectives = collectObjectives;
        KillObjectives = killObjectives;
        QuestGiverId = questGiverId;
    }
}
[Serializable]
public class QuestGiverData
{
    public List<string> CompletedQuests { get; set; }
    public int QuestGiverId { get; set; }
    public QuestGiverData(List<string> completedQuests, int questGiverId)
    {
        CompletedQuests = completedQuests;
        QuestGiverId = questGiverId;
    }
}
