using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VendorButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField]
    private Image icon;

    [SerializeField]
    private Text title;

    [SerializeField]
    private Text price;

    [SerializeField]
    private Text quantity;

    public VendorItem Item { get; set; }

    public Image Icon { get => icon; set => icon = value; }
    public Text Title { get => title; set => title = value; }

    public void AddItem(VendorItem vendorItem)
    {
        Item = vendorItem;

        if (vendorItem.Quantity > 0 || (vendorItem.Quantity == 0 && vendorItem.Unlimited) )
        {
            Icon.sprite = vendorItem.Item.Icon;
            Title.text = string.Format("<color={0}>{1}</color>", QualityColor.Colors[vendorItem.Item.Quality], vendorItem.Item.Title);
            price.text = string.Format("Price: {0} coins", vendorItem.Item.Price.ToString());

            if (!vendorItem.Unlimited)
            {
                quantity.text = vendorItem.Quantity.ToString();
            }
            else
            {
                quantity.text = string.Empty;
            }

            gameObject.SetActive(true);
        }
        
      
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if ((Player.Instance.Coins >= Item.Item.Price) && InventoryScript.Instance.AddItem(Instantiate(Item.Item)))
        {
            SellItem();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.Instance.ShowTooltip(transform.position, Item.Item);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.HideTooltip();
    }

    public void SellItem()
    {
        Player.Instance.Coins -= Item.Item.Price;

        if (!Item.Unlimited)
        {
            Item.Quantity--;
            quantity.text = Item.Quantity.ToString();

            if (Item.Quantity == 0)
            {
                gameObject.SetActive(false); // instead gray item out?
            }
        }
    }
}
