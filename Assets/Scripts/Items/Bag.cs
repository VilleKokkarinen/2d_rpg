using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Bag", menuName ="Items/Bag", order = 1)]
public class Bag : Item, IUseable
{

    [SerializeField]
    private int slots;

    [SerializeField]
    protected GameObject bagPrefab;

    public BagScript bagScript { get; set; }

    public int Slots { get => slots; }

    public BagButton bagButton { get; set; }

    public void Initialize(int slots)
    {
        this.slots = slots;
    }
   
    public void Use()
    {
        if(InventoryScript.Instance.CanAddBag)
        {
            Remove();
            bagScript = Instantiate(bagPrefab, InventoryScript.Instance.transform).GetComponent<BagScript>();
            bagScript.AddSlots(slots);

            InventoryScript.Instance.AddBag(this);
        }
      
    }

    public void SetupScript()
    {
        bagScript = Instantiate(bagPrefab, InventoryScript.Instance.transform).GetComponent<BagScript>();
        bagScript.AddSlots(slots);
    }

    public override string GetDescription()
    {
        return base.GetDescription() + string.Format("\n<color=#00ff00ff>{0} slot bag</color>", slots);
    }
}
