using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotScript : MonoBehaviour, IPointerClickHandler, IClickable, IPointerEnterHandler, IPointerExitHandler
{
    private ObservableStack<Item> items = new ObservableStack<Item>();

    [SerializeField]
    private Image icon;

    [SerializeField]
    private Text stackSize;

    public Item item
    {
        get
        {
            if (!IsEmpty)
            {
                return Items.Peek();
            }
            return null;
        }
    }
    public bool IsEmpty
    {
        get
        {
            return Items.Count == 0;
        }
    }
    public bool IsFull
    {
        get
        {
            if (IsEmpty || Count < item.StackSize)
            {
                return false;
            }
            return true;
        }
    }
    public Image Icon { 
        get
        {
            return icon;
        } 
        set 
        {
            icon = value;
        }
    }

    public int Count
    {
        get
        {
            return Items.Count;
        }
    }

    public BagScript MyBag { get; set; }

    public int Index { get; set; }

    public Text StackText => stackSize;

    public ObservableStack<Item> Items { get => items; }

    private void Awake()
    {
        Items.OnPop += new UpdateStackEvent(UpdateSlot);
        Items.OnPush += new UpdateStackEvent(UpdateSlot);
        Items.OnClear += new UpdateStackEvent(UpdateSlot);
    }
    public bool AddItem(Item item)
    {
        Items.Push(item);

        icon.sprite = item.Icon;
        icon.color = Color.white;

        item.Slot = this;
        return true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            if(InventoryScript.Instance.FromSlot == null && !IsEmpty)
            {
                if (HandScript.Instance.Moveable != null)
                {
                    if (HandScript.Instance.Moveable is Armor)
                    {
                        if(item is Armor && (item as Armor).Armortype == (HandScript.Instance.Moveable as Armor).Armortype)
                        {
                            (item as Armor).Equip();
                            HandScript.Instance.Drop();
                        }
                    }
                  
                }
                else
                {
                    HandScript.Instance.TakeMoveable(item as IMoveable);
                    InventoryScript.Instance.FromSlot = this;
                }      
              
            }
            else if (InventoryScript.Instance.FromSlot == null && IsEmpty )
            {
                if(HandScript.Instance.Moveable is Bag){
                    Bag bag = (Bag)HandScript.Instance.Moveable;
                    if (bag.bagScript != MyBag && InventoryScript.Instance.EmptySlotCount - bag.Slots > 0)
                    {
                        AddItem(bag);
                        bag.bagButton.RemoveBag();
                        HandScript.Instance.Drop();
                    }
                }
                else if (HandScript.Instance.Moveable is Armor)
                {
                    Armor armor = (Armor)HandScript.Instance.Moveable;
                    UIManager.Instance.ShowTooltip(transform.position, armor);

                    CharacterPanel.Instance.SelectedButton.DeEquipArmor();
                    AddItem(armor);
                    HandScript.Instance.Drop();
                }
            
            }
            else if(InventoryScript.Instance.FromSlot != null)
            {
                if (PutItemBack() || MergeItems(InventoryScript.Instance.FromSlot) || SwapItems(InventoryScript.Instance.FromSlot) || AddItems(InventoryScript.Instance.FromSlot.Items))
                {
                    Item item = (Item)HandScript.Instance.Moveable;
                    UIManager.Instance.ShowTooltip(transform.position, item);
                    
                    HandScript.Instance.Drop();
                    InventoryScript.Instance.FromSlot = null;
                }
            }
           
        }
        if(eventData.button == PointerEventData.InputButton.Right && HandScript.Instance.Moveable == null)
        {
            UseItem();
        }
    }

    private bool PutItemBack()
    {
        if(InventoryScript.Instance.FromSlot == this)
        {
            InventoryScript.Instance.FromSlot.icon.color = Color.white;
            return true;
        }
        return false;
    }
    private bool SwapItems(SlotScript from)
    {
        if (IsEmpty)
        {
            return false;
        }
        if(from.item.GetType() != item.GetType() || from.Count + Count > item.StackSize)
        {
            ObservableStack<Item> tmpFrom = new ObservableStack<Item>(from.Items);
            from.Items.Clear();

            from.AddItems(Items);
            Items.Clear();

            AddItems(tmpFrom);

            UIManager.Instance.RefreshToolTip(item);
            return true;
        }
        return false;
    }
    private bool MergeItems(SlotScript from)
    {
        if (IsEmpty)
        {
            return false;
        }
        if (from.item.GetType() == item.GetType() && !IsFull)
        {
            int free = item.StackSize - Count;

            for(int i = 0; i < free; i++)
            {
                AddItem(from.Items.Pop());
            }

            return true;
        }

        return false;
    }
    public bool AddItems(ObservableStack<Item> newItems)
    {
        if (IsEmpty || newItems.Peek().GetType() == item.GetType())
        {
            int count = newItems.Count;

            for(int i = 0; i < count; i++)
            {
                if (IsFull)
                {
                    return false;
                }

                AddItem(newItems.Pop());
            }
            return true;
        }
        return false;
    }

    public void UseItem()
    {
        if(item is IUseable)
        {
            (item as IUseable).Use();
        }
        else if(item is Armor)
        {
            (item as Armor).Equip();
        }
    }
    public void RemoveItem(Item item)
    {
        if(!IsEmpty)
        {
            InventoryScript.Instance.OnItemCountchanged(Items.Pop());
        }
    }
    public void Clear()
    {
        int initialCount = Items.Count;

        if(initialCount > 0)
        {
            for (int i = 0; i < initialCount; i++)
            {
                InventoryScript.Instance.OnItemCountchanged(Items.Pop());
            }
           
        }
    }
    public bool StackItem(Item item)
    {
        if (!IsEmpty && item.name == this.item.name && Items.Count < this.item.StackSize)
        {
            Items.Push(item);
            item.Slot = this;
            return true;
        }
        return false;
    }

    private void UpdateSlot()
    {
        UIManager.Instance.UpdateStackSize(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!IsEmpty)
        {
            UIManager.Instance.ShowTooltip(transform.position, item);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.HideTooltip();
    }
}
