using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagScript : MonoBehaviour
{

    [SerializeField]
    private GameObject slotPrefab;

    private List<SlotScript> slots = new List<SlotScript>();

    private CanvasGroup canvasGroup;

    public bool IsOpen
    {
        get
        {
            return canvasGroup.alpha > 0;
        }
    }

    public List<SlotScript> Slots { get => slots; }

    public int EmptySlotCount
    {
        get
        {
            int count = 0;

            foreach(SlotScript slot in Slots)
            {
                if (slot.IsEmpty)
                {
                    count++;
                }
            }

            return count;
        }
    }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void AddSlots(int slotCount)
    {
        for(int i = 0; i < slotCount; i++)
        {
            SlotScript slot = Instantiate(slotPrefab, transform).GetComponent<SlotScript>();
            slot.Index = i;
            slot.MyBag = this;
            Slots.Add(slot);
        }
    }

    public void OpenClose()
    {
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;

        canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true;
    }

    public List<Item> GetItems()
    {
        List<Item> items = new List<Item>();

        foreach(SlotScript slot in slots)
        {
            if (!slot.IsEmpty)
            {
                foreach(Item item in slot.Items)
                {
                    items.Add(item);
                }
            }
        }
        return items;
    }

    public bool AddItem(Item item)
    {
        foreach(SlotScript slot in Slots)
        {
            if (slot.IsEmpty)
            {
                slot.AddItem(item);
                return true;
            }
        }

        return false;
    }

    public void Clear()
    {
        foreach (SlotScript slot in Slots)
        {
            slot.Clear();
        }
    }
}
