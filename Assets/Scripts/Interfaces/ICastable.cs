using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICastable
{
    string Title
    {
        get;
    }

    Sprite Icon
    {
        get;
    }

    float CastTime
    {
        get;
    }

    Color BarColor
    {
        get;
    }
}
