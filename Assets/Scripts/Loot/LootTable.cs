using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LootTable : MonoBehaviour
{
    [SerializeField]
    protected Loot[] loot;

    public List<Drop> DroppedItems { get; set; }

    private bool rolled = false;

    public List<Drop> GetLoot()
    {
        if (!rolled)
        {
            DroppedItems = new List<Drop>();
            RollLoot();         
        }
        return DroppedItems;
    }
    protected virtual void RollLoot()
    {
        foreach (Loot lootItem in loot)
        {
            int roll = RandomNumberGenerator.NumberBetween(1, 100);

            if (roll <= lootItem.DropChance)
            {
                DroppedItems.Add(new Drop(lootItem.Item, this));
            }           
        }
        rolled = true;
    }
}
