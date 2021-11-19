using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using UnityEngine;

enum ArmorType
{
    Head,
    Amulet,
    Ammo,
    Body,
    Cape,
    Legs,
    Boots,
    Gloves,
    Ring,
    Bracelet,
    MainHand,
    OffHand,
    TwoHand
}

[CreateAssetMenu(fileName = "Armor", menuName = "Items/Armor", order = 2)]
public class Armor : Item
{
    [SerializeField]
    private ArmorType armortype;

    [SerializeField]
    private ItemStat[] itemstats;

    [SerializeField]
    private AnimationClip[] animationClips;

    internal ArmorType Armortype { get => armortype; }

    public AnimationClip[] AnimationClips { get => animationClips; }

   

    public override string GetDescription()
    {
        string stats = string.Empty;

        foreach(ItemStat stat in itemstats)
        {
            stats += string.Format("\n <color={0}>{1}{2}</color> {3}", stat.Value >= 0 ? "#3fe07f" : "#e36439", stat.Value >= 0 ? "+" : "", stat.Value, stat.Name);

        }


        return base.GetDescription() + stats;
    }

    public void Equip()
    {
        CharacterPanel.Instance.EquipArmor(this);
    }
}
