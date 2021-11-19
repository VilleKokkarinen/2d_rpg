using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : ScriptableObject, IMoveable, IDescribable
{
    [SerializeField]
    private Sprite icon;

    [SerializeField]
    private int stackSize;

    private SlotScript slot;

    public Sprite Icon { get => icon; }

    public int StackSize { get => stackSize; }

    [SerializeField]
    private string title;

    [SerializeField]
    private Quality quality;

    private CharButton charButton;

    [SerializeField]
    private int price;

    public SlotScript Slot { get => slot; set => slot = value; }
    public Quality Quality { get => quality; }
    public string Title { get => title; }

    public CharButton CharButton { get => charButton;
        set
        {
            Slot = null;
            charButton = value;
        }
    }

    public int Price { get => price; set => price = value; }

    public virtual string GetDescription()
    {
        return $"<color={QualityColor.Colors[Quality]}> {Title}</color>";
    }

    public void Remove()
    {
        if(slot != null)
        {
            slot.RemoveItem(this);
        }
    }
}
