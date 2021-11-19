using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ManaPotion", menuName = "Items/ManaPotion", order = 1)]
public class ManaPotion : Item, IUseable
{
    [SerializeField]
    private int mana;
   
    public void Use()
    {
        if (Player.Instance.Mana.StatCurrentValue < Player.Instance.Mana.MaxValue)
        {
            Remove();

            Player.Instance.GetMana(mana);
        }
      
      
    }
}
