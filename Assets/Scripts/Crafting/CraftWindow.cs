using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftWindow : Window
{
    [SerializeField]
    private Text title;

    [SerializeField]
    private Text description;

    [SerializeField]
    private GameObject materialPrefab;

    [SerializeField]
    private Transform parent;

    private List<GameObject> materials = new List<GameObject>();

    [SerializeField]
    private Recipe selectedRecipe;

    [SerializeField]
    private ItemInfo craftItemInfo;

    [SerializeField]
    private Text countText;

    private int maxAmount;

    private int amount;

    private List<int> amounts = new List<int>();
    
    private int Amount
    {
        set
        {
            countText.text = value.ToString();
            amount = value;
        }
        get => amount;
    }

    private void Start()
    {
        InventoryScript.Instance.itemCountChangedEvent += new ItemCountChanged(UpdateMaterialCount);
        ShowDescription(selectedRecipe);
    }

    public void ShowDescription(Recipe recipe)
    {
        if(selectedRecipe != null)
        {
            selectedRecipe.DeSelect();
        }
        selectedRecipe = recipe;
        selectedRecipe.Select();

        foreach(GameObject go in materials)
        {
            Destroy(go);
        }

        materials.Clear();

        title.text = recipe.Output.Title;
        description.text = recipe.Description;

        craftItemInfo.Initialize(recipe.Output, recipe.OutputCount);

        foreach(CraftingMaterial material in recipe.Materials)
        {
            GameObject tmp = Instantiate(materialPrefab, parent);

            tmp.GetComponent<ItemInfo>().Initialize(material.Item, material.Count);

            materials.Add(tmp);
        }
        UpdateMaterialCount(null);
    }

    private void UpdateMaterialCount(Item item)
    {
        amounts.Sort();

        foreach (GameObject material in materials)
        {
            ItemInfo tmp = material.GetComponent<ItemInfo>();
            tmp.UpdateStackCount();
        }
        if (CanCraft())
        {
            maxAmount = amounts[0];

            if(countText.text == "0")
            {
                Amount = 1;
            }
            else if(int.Parse(countText.text) > maxAmount)
            {
                Amount = maxAmount;
            }
        }
        else
        {
            Amount = 0;
            maxAmount = 0;
        }
    }

    public void Craft(bool All)
    {       
        if (CanCraft() && !Player.Instance.IsAttacking)
        {
            if (All)
            {
                amounts.Sort();
                countText.text = maxAmount.ToString();
                StartCoroutine(CraftRoutine(amounts[0]));
            }
            else{
                StartCoroutine(CraftRoutine(amount));
            }

          
        }
       
    }

    private bool CanCraft()
    {
        bool canCraft = true;

        amounts = new List<int>();

        foreach (var material in selectedRecipe.Materials)
        {
            int count = InventoryScript.Instance.GetItemCount(material.Item.Title);
            if (count >= material.Count)
            {
                amounts.Add(count/material.Count);
                continue;
            }
            else
            {
                canCraft = false;
                break;
            }
        }


        return canCraft;
    }

    private IEnumerator CraftRoutine(int count)
    {
        for (int i = 0; i < count; i++)
        {
            yield return Player.Instance.InitRoutine = StartCoroutine(Player.Instance.CraftRoutine(selectedRecipe));
        }
       
    }

    public void AddItemsToInventory()
    {
        if (InventoryScript.Instance.AddItem(craftItemInfo.Item))
        {
            foreach (var material in selectedRecipe.Materials)
            {
                for (int i = 0; i < material.Count; i++)
                {
                    InventoryScript.Instance.RemoveItem(material.Item);
                }
            }
        }
        
    }

    public void ChangeAmount(int i)
    {
        if((amount + i) > 0 && amount + i <= maxAmount)
        {
            Amount += i;
        }
    }

    public override void Close()
    {
        base.Close();
    }

}
