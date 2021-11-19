using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour, IPointerClickHandler, IClickable, IPointerEnterHandler, IPointerExitHandler
{
    public IUseable Useable { get; set; }

    private Stack<IUseable> useables = new Stack<IUseable>();
    private int useableCount;

    [SerializeField]
    private Text stackSize;

    public Button button { get; private set; }
    public Image Icon { get => icon; set => icon = value; }

    public int Count => useableCount;

    public Text StackText => stackSize;

    public Stack<IUseable> Useables {
        get => useables;
        set
        {
            if(value.Count > 0)
            {
                Useable = value.Peek();
            }
            else
            {
                Useable = null;
            }
          
            useables = value;
        }
    }

    [SerializeField]
    private Image icon;


    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);

        InventoryScript.Instance.itemCountChangedEvent += new ItemCountChanged(UpdateItemCount);
    }

    public void OnClick()
    {
        if(HandScript.Instance.Moveable == null)
        {
            if (Useable != null)
            {
                Useable.Use();
            }
            else if(Useables != null && Useables.Count > 0)
            {
                Useables.Peek().Use();
            }
        } 
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            if(HandScript.Instance.Moveable != null && HandScript.Instance.Moveable is IUseable)
            {
                SetUseable(HandScript.Instance.Moveable as IUseable);
            }
        }
    }
    public void SetUseable(IUseable useable)
    {
        if (useable is Item)
        {
            Useables = InventoryScript.Instance.GetUseables(useable);
            useableCount = Useables.Count;

            if (InventoryScript.Instance.FromSlot != null)
            {
                InventoryScript.Instance.FromSlot.Icon.color = Color.white;
                InventoryScript.Instance.FromSlot = null;
            }
          
        }
        else
        {
            Useables.Clear();
            Useable = useable;
        }

        useableCount = Useables.Count;

        UpdateVisual(useable as IMoveable);
        UIManager.Instance.RefreshToolTip(Useable as IDescribable);
    }

    public void UpdateVisual(IMoveable moveable)
    {
        if(HandScript.Instance.Moveable != null)
        {
            HandScript.Instance.Drop();
        }

        icon.sprite = moveable.Icon;
        icon.color = Color.white;
        
        if(useableCount > 1)
        {
            UIManager.Instance.UpdateStackSize(this);
        }
        else if(Useable is Spell)
        {
            UIManager.Instance.ClearStackSize(this);
        }
    }

    public void UpdateItemCount(Item item)
    {
        if(item is IUseable && Useables.Count > 0)
        {
            if(Useables.Peek().GetType() == item.GetType())
            {
                Useables = InventoryScript.Instance.GetUseables(item as IUseable);

                useableCount = Useables.Count;

                UIManager.Instance.UpdateStackSize(this);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        IDescribable tmp = null;

        if(Useable != null && Useable is IDescribable)
        {
            tmp = (IDescribable)Useable;
            //UIManager.Instance.ShowTooltip(transform.position, );
        }
        else if (Useables.Count > 0)
        {
            //UIManager.Instance.ShowTooltip(transform.position);
        }
        if(tmp != null)
        {
            UIManager.Instance.ShowTooltip(transform.position, tmp);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.HideTooltip();
    }
}
