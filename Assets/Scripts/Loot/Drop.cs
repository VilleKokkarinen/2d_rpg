using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop
{
    public Item Item { get; set; }

    public LootTable LootTable { get; set; }

    public Drop(Item item, LootTable table)
    {
        Item = item;
        LootTable = table;
    }

    public void Remove()
    {
        LootTable.DroppedItems.Remove(this);
    }
}
