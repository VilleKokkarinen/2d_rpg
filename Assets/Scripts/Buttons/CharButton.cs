using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private ArmorType armorType;

    private Armor equippedArmor;

    [SerializeField]
    private Image icon;

    [SerializeField]
    private GearSocket gearSocket;

    public Armor EquippedArmor { get => equippedArmor; }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            if(HandScript.Instance.Moveable is Armor)
            {
                Armor tmp = (Armor)HandScript.Instance.Moveable;

                if (tmp.Armortype == armorType)
                {
                    EquipArmor(tmp);
                }
                UIManager.Instance.RefreshToolTip(tmp);
            }
            else if(HandScript.Instance.Moveable == null && equippedArmor != null)
            {
                HandScript.Instance.TakeMoveable(equippedArmor);
                icon.color = Color.gray;

                CharacterPanel.Instance.SelectedButton = this;
            }
        }
    }


    public void EquipArmor(Armor armor)
    {
        armor.Remove();

        if(equippedArmor != null)
        {
            if(equippedArmor != armor)
            {
                armor.Slot.AddItem(equippedArmor);
            }
          
            UIManager.Instance.RefreshToolTip(equippedArmor);
        }
        else
        {
            UIManager.Instance.HideTooltip();
        }

        icon.enabled = true;
        icon.sprite = armor.Icon;
        icon.color = Color.white;

        equippedArmor = armor;

        equippedArmor.CharButton = this;

        if(HandScript.Instance.Moveable == (armor as IMoveable))
        {
            HandScript.Instance.Drop();
        }
     
        if(gearSocket != null && equippedArmor.AnimationClips != null)
        {
            gearSocket.Equip(equippedArmor.AnimationClips);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (equippedArmor != null)
        {
            UIManager.Instance.ShowTooltip(transform.position, equippedArmor);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.HideTooltip();
    }

    public void DeEquipArmor()
    {
        icon.color = Color.white;
        icon.enabled = false;


        if (gearSocket != null && equippedArmor.AnimationClips != null)
        {
            gearSocket.DeEquip();
        }

        equippedArmor.CharButton = null;
        equippedArmor = null;
    }
}
