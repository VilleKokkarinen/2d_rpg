using System.Collections;
using System;
using UnityEngine;

[Serializable]
public class Spell: IUseable, IMoveable, IDescribable, ICastable
{
    [SerializeField]
    private string name;

    [SerializeField]
    private int damage;

    [SerializeField]
    private Sprite icon;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float castTime;

    [SerializeField]
    private GameObject spellPrefab;

    [SerializeField]
    private Color barColor;

    public string Name { get => name; }
    public int Damage { get => damage;}
    public Sprite Icon { get => icon; }
    public float Speed { get => speed; }
    public float CastTime { get => castTime;}
    public GameObject SpellPrefab { get => spellPrefab; }
    public Color BarColor { get => barColor; }

    public string Title => Name;

    [SerializeField]
    private string description;

    public string GetDescription()
    {
        return string.Format("{0}\nCast time: {1} second(s)\n<color=#ffd111>{2}</color>\nDamage: {3}", name, castTime,description, damage);
    }

    public void Use()
    {
        Player.Instance.CastSpell(this);
    }
}