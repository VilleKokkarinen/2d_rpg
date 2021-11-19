using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Recipe : MonoBehaviour, ICastable
{
    [SerializeField]
    private CraftingMaterial[] materials;

    [SerializeField]
    private Item output;

    [SerializeField]
    private int outputCount;


    [SerializeField]
    private string description;

    [SerializeField]
    private Image highlight;

    [SerializeField]
    private float craftTime;

    [SerializeField]
    private Color barColor;

    public Item Output { get => output; }
    public int OutputCount { get => outputCount; }
    public string Description { get => description; }
    public CraftingMaterial[] Materials { get => materials; }

    public string Title => output.Title;

    public Sprite Icon => output.Icon;

    public float CastTime => craftTime;

    public Color BarColor => barColor;


    private void Start()
    {
        GetComponent<Text>().text = output.Title;
    }

    public void Select()
    {
        Color c = highlight.color;
        c.a = .3f;

        highlight.color = c;
    }
    public void DeSelect()
    {
        Color c = highlight.color;
        c.a = 0f;

        highlight.color = c;
    }
}
