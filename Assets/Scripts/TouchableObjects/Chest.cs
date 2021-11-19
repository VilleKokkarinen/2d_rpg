using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Sprite open, closed;

    private bool isOpen;

    [SerializeField]
    private CanvasGroup canvasGroup;

    private List<Item> items;

    [SerializeField]
    private BagScript bag;

    public List<Item> Items { get => items; }
    public BagScript Bag { get => bag; }

    private void Awake()
    {
        items = new List<Item>();
    }

    public void Interact()
    {
        if(isOpen)
        {
            StopInteract();
        }
        else
        {
            AddItems();
            isOpen = true;
            spriteRenderer.sprite = open;

            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
        }
    }

    public void StopInteract()
    {
        if (isOpen)
        {
            StoreItems();

            bag.Clear();

            isOpen = false;
            spriteRenderer.sprite = closed;

            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
        }       
    }

    public void AddItems()
    {
        if(items != null)
        {
            foreach (Item item in items)
            {
                item.Slot.AddItem(item);
            }
        }
    }

    public void StoreItems()
    {
        items = bag.GetItems();
    }
}