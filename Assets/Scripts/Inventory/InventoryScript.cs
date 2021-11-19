using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ItemCountChanged(Item item);

public class InventoryScript : MonoBehaviour
{
    public event ItemCountChanged itemCountChangedEvent;

    private static InventoryScript instance;
    public static InventoryScript Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<InventoryScript>();
            }

            return instance;
        }
    }

    public int EmptySlotCount
    {
        get
        {
            return bag.bagScript.EmptySlotCount;
        }
    }

    [SerializeField]
    private BagButton bagButton;

    //for debugging
    [SerializeField]
    private Item[] items;

    private Bag bag;

    public int TotalSlotCount
    {
        get
        {
            return bag.bagScript.Slots.Count;
        }
    }
    public int FullSlotCount
    {
        get
        {
            return TotalSlotCount - EmptySlotCount;
        }
    }
    private SlotScript fromSlot;

    public bool CanAddBag
    {
        get
        {
            return bag == null;
        }
    }

    public SlotScript FromSlot
    {
        get => fromSlot;
        set
        {
            fromSlot = value;
            if(value != null)
            {
                fromSlot.Icon.color = Color.grey;
            }
        }
    }

    public Bag Bag { get => bag;}

    public List<SlotScript> GetSlots()
    {
        List<SlotScript> slots = new List<SlotScript>();

        foreach (var slot in Bag.bagScript.Slots)
        {
            if (!slot.IsEmpty)
            {
                slots.Add(slot);
            }
        }
        return slots;
    }



    public Stack<Item> GetItems(string type, int count)
    {
        Stack<Item> items = new Stack<Item>();

        foreach (SlotScript slot in bag.bagScript.Slots)
        {
            if (!slot.IsEmpty && slot.item.GetType() == type.GetType())
            {
                foreach (Item item in slot.Items)
                {
                    items.Push(item);

                    if(items.Count == count)
                    {
                        return items;
                    }
                }
            }
        }

        return items;
    }

    public IUseable GetUseable(string type)
    {
        foreach (SlotScript slot in bag.bagScript.Slots)
        {
            if (!slot.IsEmpty && slot.item.GetType() == type.GetType())
            {
                return slot.item as IUseable;
            }
        }
        return null;
    }

    public Stack<IUseable> GetUseables(IUseable type)
    {
        Stack<IUseable> useables = new Stack<IUseable>();

        foreach(SlotScript slot in bag.bagScript.Slots)
        {
            if (!slot.IsEmpty && slot.item.GetType() == type.GetType())
            {
                foreach(Item item in slot.Items)
                {
                    useables.Push(item as IUseable);
                }
            }
        }
        return useables;
    }

    private void Awake()
    {
        Bag bag = (Bag)Instantiate(items[0]);
        bag.Initialize(28);
        bag.Use();

        ManaPotion pot = (ManaPotion)Instantiate(items[1]);
        AddItem(pot);

        Armor debug_helmet = (Armor)Instantiate(items[2]);
        AddItem(debug_helmet);

        Armor debug_pants = (Armor)Instantiate(items[3]);
        AddItem(debug_pants);

        Armor debug_boots = (Armor)Instantiate(items[4]);
        AddItem(debug_boots);

        Armor debug_body = (Armor)Instantiate(items[5]);
        AddItem(debug_body);

        Armor debug_weapon = (Armor)Instantiate(items[6]);
        AddItem(debug_weapon);

        HealthPotion pothp = (HealthPotion)Instantiate(items[8]);
        AddItem(pothp);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Bag bag = (Bag)Instantiate(items[0]);
            bag.Initialize(28);
            bag.Use();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            Bag bag = (Bag)Instantiate(items[0]);
            bag.Initialize(28);
            AddItem(bag);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            ManaPotion pot = (ManaPotion)Instantiate(items[1]);
            AddItem(pot);
        }
        if (Input.GetKey(KeyCode.B))
        {
            GoldNugget nugget = (GoldNugget)Instantiate(items[7]);
            AddItem(nugget);
        }
    }

    public bool AddItem(Item item)
    {
        if(item.StackSize > 0)
        {
            if (PlaceInStack(item))
            {
                return true;
            }
        }

        return PlaceInEmpty(item);
    }

    public void RemoveItem(Item item)
    {
        foreach (var slot in bag.bagScript.Slots)
        {
            if (!slot.IsEmpty && slot.item.Title == item.Title)
            {
                slot.RemoveItem(item);
                return;
            }
        }
    }

    private bool PlaceInEmpty(Item item)
    {
        if (bag.bagScript.AddItem(item))
        {
            OnItemCountchanged(item);
            return true;
        }
        return false;
    }

    private bool PlaceInStack(Item item)
    {
        foreach(SlotScript slot in bag.bagScript.Slots)
        {
            if (slot.StackItem(item)){
                OnItemCountchanged(item);
                return true;
            }
        }
        return false;
    }

    public void PlaceInIndex(Item item, int slotIndex)
    {
        bag.bagScript.Slots[slotIndex].AddItem(item);
    }

    public void AddBag(Bag bag)
    {
        if (bagButton.Bag == null)
        {
            bagButton.Bag = bag;
            bag.bagButton = bagButton;

            this.bag = bag;
        }       
    }

    public void RemoveBag()
    {
        bag.Remove();
        Destroy(bag.bagScript.gameObject);

    }

    public void OpenClose()
    {
        bag.bagScript.OpenClose();
    }

    public int GetItemCount(string type)
    {
        int count = 0;

        foreach (var slot in bag.bagScript.Slots)
        {
            if (!slot.IsEmpty && slot.item.Title == type)
            {
                count += slot.Items.Count;
            }
        }
        return count;
    }
    public void OnItemCountchanged(Item item)
    {
        if(itemCountChangedEvent != null)
        {
            itemCountChangedEvent.Invoke(item);
        }
    }
}
