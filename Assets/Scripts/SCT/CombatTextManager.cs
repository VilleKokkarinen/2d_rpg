using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SCTTYPE
{
    LoseHP,
    GainHP,
    GainMana,
    XPGain
}
public class CombatTextManager : MonoBehaviour
{
    private static CombatTextManager instance;
    public static CombatTextManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CombatTextManager>();
            }

            return instance;
        }
    }

   
    [SerializeField]
    private GameObject combatTextPrefab;

    public void CreateText(Vector2 position, string text, SCTTYPE type, bool crit)
    {
        Text sct = Instantiate(combatTextPrefab, transform).GetComponent<Text>();

        // hard coded offset, use a child object as parent instead?
        position.y += 0.6f;

        sct.transform.position = position;

        string before = string.Empty;
        string after = string.Empty;

        switch(type)
        {
            case SCTTYPE.LoseHP:
                before = "-";
                sct.color = Color.red;
                break;
            case SCTTYPE.GainHP:
                before = "+";
                after = " HP";
                sct.color = Color.green;
                break;
            case SCTTYPE.GainMana:
                before = "+";
                after = " Mana";
                sct.color = Color.cyan;
                break;
            case SCTTYPE.XPGain:
                before = "+";
                after = " XP";
                sct.color = Color.yellow;
                break;
        }

        sct.text = before + text + after;

        if (crit)
        {
            sct.GetComponent<Animator>().SetBool("Crit", crit);
        }

    }
}
