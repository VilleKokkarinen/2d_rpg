using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootWindow : MonoBehaviour
{
    private static LootWindow instance;
    public static LootWindow Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<LootWindow>();
            }

            return instance;
        }
    }

    [SerializeField]
    private Lootbutton[] lootButtons;

    private List<List<Drop>> pages = new List<List<Drop>>();

    private List<Drop> droppedLoot = new List<Drop>();

    private int pageIndex = 0;

    [SerializeField]
    private Item[] items;


    [SerializeField]
    private Text pageNumber;

    [SerializeField]
    private GameObject nextbtn, previousbtn;

    private CanvasGroup canvasGroup;

    public IInteractable Interactable { get; set; }

    public bool IsOpen
    {
        get
        {
            return canvasGroup.alpha > 0;
        }
    }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        ClearButtons();
    }


    public void CreatePages(List<Drop> items)
    {
        if (!IsOpen)
        {
            List<Drop> page = new List<Drop>();

            droppedLoot = items;

            for (int i = 0; i < items.Count; i++)
            {
                page.Add(items[i]);

                if (page.Count == 4 || i == items.Count - 1)
                {
                    pages.Add(page);
                    page = new List<Drop>();
                }
            }

            AddLoot();
            Open();
        }
      
    }

    private void AddLoot()
    {
        if(pages.Count > 0)
        {
            if (pages.Count - 1 < pageIndex)
            {
                pageIndex = pages.Count - 1;
            }

            pageNumber.text = pageIndex + 1 + "/" + pages.Count;

            previousbtn.SetActive(pageIndex > 0);

            nextbtn.SetActive(pages.Count > 1 && pageIndex < pages.Count -1);


            for (int i = 0; i < pages[pageIndex].Count; i++)
            {

                if(pages[pageIndex][i] != null)
                {
                    lootButtons[i].Icon.sprite = pages[pageIndex][i].Item.Icon;
                    lootButtons[i].gameObject.SetActive(true);
                    lootButtons[i].Item = pages[pageIndex][i].Item;

                    string title = string.Format("<color={0}>{1}</color>", QualityColor.Colors[pages[pageIndex][i].Item.Quality], pages[pageIndex][i].Item.Title);

                    lootButtons[i].Title.text = title;
                }             
            }
        }
    }

    public void TakeLoot(Item loot)
    {
        Drop drop = pages[pageIndex].Find(x => x.Item == loot);

        droppedLoot.Remove(drop);

        drop.Remove();
        
        pages[pageIndex].Remove(drop);

        if (pages[pageIndex].Count == 0)
        {
            pages.Remove(pages[pageIndex]);

            if(pageIndex == pages.Count && pageIndex > 0)
            {
                pageIndex--;
            }
            AddLoot();
        }

    }

    public void ClearButtons()
    {
        foreach(Lootbutton btn in lootButtons)
        {
            btn.gameObject.SetActive(false);
        }
    }
    public void NextPage()
    {
        if(pageIndex < pages.Count -1)
        {
            pageIndex++;

            ClearButtons();
            AddLoot();
        }
    }
    public void PreviousPage()
    {
        if (pageIndex > 0)
        {
            pageIndex--;

            ClearButtons();
            AddLoot();
        }
    }

    public void Open()
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
    }
    public void Close()
    {
        pageIndex = 0;
        pages.Clear();

        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;

        ClearButtons();

        if(Interactable != null)
        {
            Interactable.StopInteract();
        }

        Interactable = null;
    }

}
