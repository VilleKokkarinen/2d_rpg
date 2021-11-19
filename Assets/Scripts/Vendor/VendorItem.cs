using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VendorItem
{
    [SerializeField]
    private Item item;

    [SerializeField]
    private int quantity;

    [SerializeField]
    private int stackSize;

    [SerializeField]
    private bool unlimited;

    public Item Item { get => item; }
    public int Quantity { get => quantity; set => quantity = value; }
    public int StackSize { get => stackSize; }
    public bool Unlimited { get => unlimited; }
}
