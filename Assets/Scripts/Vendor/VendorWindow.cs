using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VendorWindow : Window
{
   

    [SerializeField]
    private VendorButton[] buttons;

    private List<List<VendorItem>> pages = new List<List<VendorItem>>();

    private int pageIndex = 0;

    [SerializeField]
    private Text pageNumber;

    [SerializeField]
    private GameObject nextbtn, previousbtn;

    private Vendor vendor;

    private void Awake()
    {
        ClearButtons();
    }

    public void CreatePages(VendorItem[] items)
    {
        List<VendorItem> page = new List<VendorItem>();

        for (int i = 0; i < items.Length; i++)
        {
            page.Add(items[i]);

            if (page.Count == 10 || i == items.Length - 1)
            {
                pages.Add(page);
                page = new List<VendorItem>();
            }
        }

        AddItems();

    }

    public void AddItems()
    {
        if (pages.Count > 0)
        {

            if (pages.Count - 1 < pageIndex)
            {
                pageIndex = pages.Count - 1;
            }

            pageNumber.text = pageIndex + 1 + "/" + pages.Count;

            previousbtn.SetActive(pageIndex > 0);

            nextbtn.SetActive(pages.Count > 1 && pageIndex < pages.Count - 1);


            for (int i = 0; i < pages[pageIndex].Count; i++)
            {

                if (pages[pageIndex][i] != null)
                {
                    buttons[i].AddItem(pages[pageIndex][i]);
                }
            }
        }
    }

  
    public void ClearButtons()
    {
        foreach (VendorButton btn in buttons)
        {
            btn.gameObject.SetActive(false);
        }
    }

    public void NextPage()
    {
        if (pageIndex < pages.Count - 1)
        {
            pageIndex++;

            ClearButtons();
            AddItems();
        }
    }
    public void PreviousPage()
    {
        if (pageIndex > 0)
        {
            pageIndex--;

            ClearButtons();
            AddItems();
        }
    }

    public override void Open(NPC npc)
    {
        CreatePages((npc as Vendor).Items);
        base.Open(npc);
    }
}
