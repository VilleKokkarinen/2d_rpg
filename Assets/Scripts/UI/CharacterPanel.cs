using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPanel : MonoBehaviour
{
    private static CharacterPanel instance;
    public static CharacterPanel Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CharacterPanel>();
            }

            return instance;
        }

    }

    [SerializeField]
    private CanvasGroup canvasgroup;

    [SerializeField]
    private CharButton head, amulet, body, cape, ammo, legs, main, off, gloves, boots, ring, bracelet;

    public CharButton SelectedButton { get; set; }

    public void OpenClose()
    {
        canvasgroup.alpha = canvasgroup.alpha > 0 ? 0 : 1;

        canvasgroup.blocksRaycasts = canvasgroup.blocksRaycasts == true ? false : true;

    }

    public void EquipArmor(Armor armor)
    {
        switch (armor.Armortype)
        {
            case ArmorType.Head:
                head.EquipArmor(armor);
                break;
            case ArmorType.Amulet:
                amulet.EquipArmor(armor);
                break;
            case ArmorType.Ammo:
                ammo.EquipArmor(armor);
                break;
            case ArmorType.Body:
                body.EquipArmor(armor);
                break;
            case ArmorType.Cape:
                cape.EquipArmor(armor);
                break;
            case ArmorType.Legs:
                legs.EquipArmor(armor);
                break;
            case ArmorType.Boots:
                boots.EquipArmor(armor);
                break;
            case ArmorType.Gloves:
                gloves.EquipArmor(armor);
                break;
            case ArmorType.Ring:
                ring.EquipArmor(armor);
                break;
            case ArmorType.MainHand:
                main.EquipArmor(armor);
                break;
            case ArmorType.OffHand:
                off.EquipArmor(armor);
                break;
            /*case ArmorType.TwoHand:
                break;*/
        }
    }
}