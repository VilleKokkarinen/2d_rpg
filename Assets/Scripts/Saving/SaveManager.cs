using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{
    private Chest[] chests;

    [SerializeField]
    private Item[] items;
    
    private CharButton[] equipmentSlots;

    [SerializeField]
    public ActionButton[] actionButtons;

    [SerializeField]
    private SavedGame[] saveSlots;

    private string action;

    [SerializeField]
    private GameObject dialogue;

    [SerializeField]
    private Text dialogueText;

    private SavedGame current;

    private void Awake()
    {
        chests = FindObjectsOfType<Chest>();
        equipmentSlots = FindObjectsOfType<CharButton>();

      
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("Load"))
        {
            Load(saveSlots[PlayerPrefs.GetInt("Load")]);
            PlayerPrefs.DeleteKey("Load");
        }
        else
        {
            Player.Instance.SetDefaultValues();
        }
        foreach (var savegame in saveSlots)
        {
            ShowSavedFiles(savegame);
        }
    }

    public void ShowDialogue(GameObject go)
    {
        action = go.name;

        switch(action)
        {
            case "Load":
                dialogueText.text = "Load game?";
                break;
            case "Save":
                dialogueText.text = "Save game?";
                break;
            case "Delete":
                dialogueText.text = "Delete savefile?";
                break;
        }

        current = go.GetComponentInParent<SavedGame>();

        dialogue.SetActive(true);
    }

    public void ExecuteAction()
    {
        switch (action)
        {
            case "Load":
                LoadScene(current);
                CloseDialogue();
                break;
            case "Save":
                Save(current);
                CloseDialogue();
                break;
            case "Delete":
                Delete(current);
                CloseDialogue();
                break;
        }
      
    }

    private void LoadScene(SavedGame save)
    {
        if (File.Exists(Application.persistentDataPath + "/" + save.gameObject.name + ".dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();

            FileStream file = File.Open(Application.persistentDataPath + "/" + save.gameObject.name + ".dat", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();

            PlayerPrefs.SetInt("Load", save.Index);

            SceneManager.LoadScene(data.Scene);

        }
    }

    public void CloseDialogue()
    {
        dialogue.SetActive(false);
    }
    private void ShowSavedFiles(SavedGame save)
    {
        if(File.Exists(Application.persistentDataPath + "/" + save.gameObject.name + ".dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();

            FileStream file = File.Open(Application.persistentDataPath + "/" + save.gameObject.name + ".dat", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();

            save.ShowInfo(data);
        }
    }

    public void Save(SavedGame savedGame)
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();

            FileStream file = File.Open(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat", FileMode.Create);

            SaveData data = new SaveData();

            data.Scene = SceneManager.GetActiveScene().name;

            SaveEquipment(data);

            SaveBag(data);

            SaveInventory(data);

            SavePlayer(data);
            SaveChests(data);

            SaveActionButtons(data);

            SaveQuests(data);

            SaveQuestGivers(data);

            bf.Serialize(file, data);

            file.Close();

            ShowSavedFiles(savedGame);
        }
        catch (Exception e)
        {
            Debug.Log(e);
            Delete(savedGame);
            PlayerPrefs.DeleteKey("Load");
        }
    }
    public void Load(SavedGame savedGame)
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();

            FileStream file = File.Open(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat", FileMode.Open);

            SaveData data = (SaveData)bf.Deserialize(file);

            file.Close();

            LoadEquipment(data);

            LoadBag(data);

            LoadInventory(data);

            LoadPlayer(data);
            LoadChests(data);

            LoadActionButtons(data);

            LoadQuests(data);

            LoadQuestGivers(data);
        }
        catch (Exception e)
        {
            Debug.Log(e);

            Delete(savedGame);
            PlayerPrefs.DeleteKey("Load");
            SceneManager.LoadScene(0);
        }
    }

    private void Delete(SavedGame savedGame)
    {
        try
        {
            File.Delete(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat");
            savedGame.HideVisuals();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
    private void SavePlayer(SaveData data)
    {
        Player p = Player.Instance;
        data.playerData = new PlayerData(
            p.Level,
            p.Xp.StatCurrentValue,
            p.Xp.MaxValue,
            p.Health.StatCurrentValue,
            p.Health.MaxValue,
            p.Mana.StatCurrentValue,
            p.Mana.MaxValue,
            p.transform.position
            );
        
    }

    private void LoadPlayer(SaveData data)
    {
        Player p = Player.Instance;

        p.Level = data.playerData.Level;
        p.UpdateLevel();

        p.Health.Initialize(data.playerData.Health, data.playerData.MaxHealth);
        p.Mana.Initialize(data.playerData.Mana, data.playerData.MaxMana);

        p.Xp.Initialize(data.playerData.XP, data.playerData.MaxXP);

        p.transform.position = new Vector2(data.playerData.PositionX, data.playerData.PositionY);
    }

    private void SaveChests(SaveData data)
    {
        for(int i = 0; i < chests.Length; i++)
        {
            data.chestData.Add(new ChestData(chests[i].name));

            foreach(Item item in chests[i].Items)
            {
                if(chests[i].Items.Count > 0)
                {
                    data.chestData[i].Items.Add(new ItemData(item.Title, item.Slot.Items.Count, item.Slot.Index));
                }
            }
        } 
    }

    private void LoadChests(SaveData data)
    {
        foreach (var chest in data.chestData)
        {
            Chest c = Array.Find(chests, x => x.name == chest.Name);

            foreach (var itemData in chest.Items)
            {
                Item item = Instantiate(Array.Find(items, x => x.Title == itemData.Title));
                item.Slot = c.Bag.Slots.Find(x => x.Index == itemData.SlotIndex);
                c.Items.Add(item);

            }
        }
    }

    private void SaveBag(SaveData data)
    {
        data.inventoryData.Inventory = new BagData(InventoryScript.Instance.Bag.Slots);
    }
    private void LoadBag(SaveData data)
    {
        Bag newBag = (Bag)Instantiate(items[0]);
        newBag.SetupScript();
        newBag.Initialize(data.inventoryData.Inventory.SlotCount);

        InventoryScript.Instance.AddBag(newBag);
    }

    private void SaveInventory(SaveData data)
    {
        List<SlotScript> slots = InventoryScript.Instance.GetSlots();

        foreach (var slot in slots)
        {
            data.inventoryData.Items.Add(new ItemData(slot.item.Title, slot.Items.Count, slot.Index));

        }
    }
    private void LoadInventory(SaveData data)
    {
        foreach (var itemData in data.inventoryData.Items)
        {
            Item item = Instantiate(Array.Find(items, x => x.Title == itemData.Title));

            for (int i = 0; i < itemData.StackCount; i++)
            {
                InventoryScript.Instance.PlaceInIndex(item, itemData.SlotIndex);
            }
        }
    }

    private void SaveEquipment(SaveData data)
    {
        foreach (CharButton btn in equipmentSlots)
        {
            if(btn.EquippedArmor != null)
            {
                data.equipmentData.Add(new EquipmentData(btn.EquippedArmor.Title, btn.name));
            }
        }
    }
    private void LoadEquipment(SaveData data)
    {
        foreach (EquipmentData eq in data.equipmentData)
        {
            CharButton c = Array.Find(equipmentSlots, x => x.name == eq.Type);
            c.EquipArmor(Array.Find(items, x => x.Title == eq.Title) as Armor);
        }
    }


    private void SaveActionButtons(SaveData data)
    {
        for (int i= 0; i < actionButtons.Length; i++)
        {
            if(actionButtons[i].Useable != null)
            {
                ActionButtonData actbtnData;
                if (actionButtons[i].Useable is Spell)
                {
                    actbtnData = new ActionButtonData((actionButtons[i].Useable as Spell).Name, false, i);
                }
                else
                {
                    actbtnData = new ActionButtonData((actionButtons[i].Useable as Item).Title, true, i);
                }
                data.actionButtonData.Add(actbtnData);
            }
        }
    }
    private void LoadActionButtons(SaveData data)
    {
        foreach (var btn in data.actionButtonData)
        {
            if (btn.IsItem)
            {
                actionButtons[btn.Index].SetUseable(InventoryScript.Instance.GetUseable(btn.Action));
            }
            else
            {
                actionButtons[btn.Index].SetUseable(SpellBook.Instance.GetSpell(btn.Action));
            }
        }
    }

    private void SaveQuests(SaveData data)
    {
        foreach (var quest in QuestLog.Instance.Quests)
        {
            data.questData.Add(new QuestData(quest.Title, quest.Description, quest.CollectObjectives, quest.KillObjectives, quest.questGiver.ID));
        }
    }

    private void LoadQuests(SaveData data)
    {
        QuestGiver[] qgs = FindObjectsOfType<QuestGiver>();

        foreach (var quest in data.questData)
        {
            QuestGiver qg = Array.Find(qgs, x => x.ID == quest.QuestGiverId);
            Quest q = Array.Find(qg.Quests, x => x.Title == quest.Title);

            q.questGiver = qg;

            q.KillObjectives = quest.KillObjectives;

            QuestLog.Instance.AcceptQuest(q);
        }
    }

    private void SaveQuestGivers(SaveData data)
    {
        QuestGiver[] qgs = FindObjectsOfType<QuestGiver>();
        foreach (var questGiver in qgs)
        {
            data.questGiverData.Add(new QuestGiverData(questGiver.CompletedQuests, questGiver.ID));
        }

    }
    private void LoadQuestGivers(SaveData data)
    {
        QuestGiver[] qgs = FindObjectsOfType<QuestGiver>();

        foreach (var questGiverData in data.questGiverData)
        {
            QuestGiver questGiver = Array.Find(qgs, x => x.ID == questGiverData.QuestGiverId);

            questGiver.CompletedQuests = questGiverData.CompletedQuests;
            questGiver.UpdateQuestStatus();

        }
    }

}
