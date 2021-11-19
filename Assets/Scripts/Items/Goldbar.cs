using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Goldbar", menuName = "Items/Goldbar", order = 3)]
public class Goldbar : Item
{  
    public override string GetDescription()
    {
        return base.GetDescription() + string.Format("\n<color=#00ff00ff>Ooh, shiny!</color>");
    }
}
