using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BagButton : MonoBehaviour, IPointerClickHandler
{
    private Bag bag;

    [SerializeField]
    private Sprite full, empty;

    public Bag Bag 
    {
        get
        {          
            return bag;
        }
        set
        {
            if(value != null)
            {
                GetComponent<Image>().sprite = full;
            }
            else
            {
                GetComponent<Image>().sprite = empty;
            }
            bag = value;

        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                HandScript.Instance.TakeMoveable(bag);
            }
            else if (bag != null)
            {
                bag.bagScript.OpenClose();
            }
        }      
    }

    public void RemoveBag()
    {
        InventoryScript.Instance.RemoveBag();

        bag.bagButton = null;

        foreach (Item item in bag.bagScript.GetItems() )
        {
            InventoryScript.Instance.AddItem(item);
        }

        bag = null;
    }
}
