using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HandScript : MonoBehaviour
{
    private static HandScript instance;
    public static HandScript Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<HandScript>();
            }

            return instance;
        }
    }

    public IMoveable Moveable { get; set; }

    private Image icon;

    [SerializeField]
    private Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        icon = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        icon.transform.position = Input.mousePosition + offset;

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && Moveable != null)
        {
            DeleteItem();
        }
    }

    public void TakeMoveable(IMoveable moveable)
    {
        Moveable = moveable;

        icon.sprite = moveable.Icon;
        icon.color = Color.white;
    }
    public void Drop()
    {
        Moveable = null;
        icon.color = new Color(0, 0, 0, 0);

        InventoryScript.Instance.FromSlot = null;
    }

    public void DeleteItem()
    {
        if (Moveable is Item)
        {
            Item item = (Item)Moveable;

            if(item.Slot != null) 
            { 
                item.Slot.Clear();
            }
            else if(item.CharButton != null)
            {
                item.CharButton.DeEquipArmor();
            }

          
        }

        Drop();

        InventoryScript.Instance.FromSlot = null;
    }

    public IMoveable Put()
    {
        IMoveable tmp = Moveable;

        Moveable = null;

        icon.color = new Color(0, 0, 0, 0);

        return tmp;
    }
}
