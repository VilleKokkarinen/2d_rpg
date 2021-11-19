using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SavedGame : MonoBehaviour
{
    [SerializeField]
    private Text dateTime;

    [SerializeField]
    private Image health;

    [SerializeField]
    private Image mana;

    [SerializeField]
    private Image xp;

    [SerializeField]
    private Text healthText;

    [SerializeField]
    private Text manaText;

    [SerializeField]
    private Text xpText;

    [SerializeField]
    private Text levelText;

    [SerializeField]
    private GameObject visuals;

    [SerializeField]
    private int index;

    public int Index { get => index; }

    private void Awake()
    {
        visuals.SetActive(false);
    }

    public void ShowInfo(SaveData data)
    {
        visuals.SetActive(true);

        dateTime.text = "Date: " + data.Timestamp.ToString("dd/MM/yyyy") + " - " + data.Timestamp.ToString("H:mm");

        health.fillAmount = data.playerData.Health / data.playerData.MaxHealth;
        healthText.text = data.playerData.Health + "/" + data.playerData.MaxHealth;

        mana.fillAmount = data.playerData.Mana / data.playerData.MaxMana;
        manaText.text = data.playerData.Mana + "/" + data.playerData.MaxMana;

        xp.fillAmount = data.playerData.XP / data.playerData.MaxXP;
        xpText.text = data.playerData.XP + "/" + data.playerData.MaxXP;

        levelText.text = data.playerData.Level.ToString();
    }
    public void HideVisuals()
    {
        visuals.SetActive(false);
    }


}
