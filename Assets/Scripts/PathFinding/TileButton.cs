using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileButton : MonoBehaviour
{
    [SerializeField]
    private TileType tileType;

    public TileType TileType { get => tileType; }
}
