using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Lootbutton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField]
    private Image icon;

    [SerializeField]
    private Text title;

    private LootWindow lootWindow;

    public Image Icon { get => icon; }
    public Text Title { get => title; }

    public Item Item { get; set; }

    private void Awake()
    {
        lootWindow = GetComponentInParent<LootWindow>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (InventoryScript.Instance.AddItem(Item))
        {
            gameObject.SetActive(false);
            lootWindow.TakeLoot(Item);
            UIManager.Instance.HideTooltip();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.Instance.ShowTooltip(transform.position, Item);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.HideTooltip();
    }
}
