using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Item item;

    [SerializeField]
    private Image image;

    [SerializeField]
    private Text title;

    [SerializeField]
    private Text stack;

    private int count;

    public Item Item { get => item; set => item = value; }

    public void Initialize(Item item, int count)
    {
        this.item = item;
        this.count = count;
        image.sprite = item.Icon;
        title.text = string.Format("<color={0}>{1}</color>", QualityColor.Colors[item.Quality], item.Title);

        if(count > 1)
        {
            stack.enabled = true;
            stack.text = InventoryScript.Instance.GetItemCount(item.Title).ToString() + "/" + count.ToString();
        }

    }

    public void UpdateStackCount()
    {
        stack.text = InventoryScript.Instance.GetItemCount(item.Title) + "/" + count.ToString();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.Instance.ShowTooltip(transform.position, item);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.HideTooltip();
    }
}
