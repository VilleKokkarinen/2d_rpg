using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class WeaponSocket : GearSocket
{
    private float currentY;

    [SerializeField]
    private SpriteRenderer parentRenderer;

    public override void SetXY(float x, float y)
    {
        base.SetXY(x, y);

        if(currentY != y)
        {
          
            if(y == 1)
            {
                transform.localPosition = new Vector3(0, 0.9999999f, 0);
            }
            else
            {
                transform.localPosition = new Vector3(0, 0.9990001f, 0);
            }
        }
    }
}
