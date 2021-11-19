using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellBook : MonoBehaviour
{
    private static SpellBook instance;
    public static SpellBook Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SpellBook>();
            }

            return instance;
        }
    }

    [SerializeField]
    private Spell[] spells;

    [SerializeField]
    private Text spellName;

    [SerializeField]
    private Image spellIcon;

    [SerializeField]
    private Text castTime;

    [SerializeField]
    private Image castingBar;

    [SerializeField]
    private CanvasGroup canvasGroup;

    private Coroutine spellRoutine;

    private Coroutine fadeInRoutine;
    private Coroutine fadeOutRoutine;

    public void Cast(ICastable castable)
    {
        if (fadeOutRoutine != null)
        {
            StopCoroutine(fadeOutRoutine);
            fadeOutRoutine = null;
        }

        castingBar.color = castable.BarColor;
        spellName.text = castable.Title;
        spellIcon.sprite = castable.Icon;

        castingBar.fillAmount = 0;
        castTime.text = "0,0";

        spellRoutine = StartCoroutine(Progress(castable));
        fadeInRoutine = StartCoroutine(FadeBarIn());

    }

    private IEnumerator Progress(ICastable castable)
    {
        float timeLeft = Time.deltaTime;

        float rate = 1.0f / castable.CastTime;

        float progress = 0.0f;

        while (progress <= 1.0)
        {
            castingBar.fillAmount = Mathf.Lerp(0, 1, progress);

            progress += rate * Time.deltaTime;

            timeLeft += Time.deltaTime;

            castTime.text = (castable.CastTime - timeLeft).ToString("F1");

            if (castable.CastTime - timeLeft < 0)
            {
                castTime.text = "0,0";
            }

            yield return null;
        }

        StopCasting();
    }

    public void StopCasting()
    {
        if(fadeInRoutine != null)
        {
            StopCoroutine(fadeInRoutine);
            fadeInRoutine = null;
            Fadeout();
        }
        if (spellRoutine != null)
        {
            StopCoroutine(spellRoutine);
            spellRoutine = null;
        }

    }

    private void Fadeout()
    {
        fadeOutRoutine = StartCoroutine(FadeBarOut());
    }

    private IEnumerator FadeBarIn()
    {
        float rate = 1.0f / 0.25f;

        float progress = 0.0f;

        while (progress <= 1.0)
        {
            canvasGroup.alpha = Mathf.Lerp(0, 1, progress);

            progress += rate * Time.deltaTime;

            yield return null;
        }
    }

    private IEnumerator FadeBarOut()
    {
        float rate = 1.0f / 0.25f;

        float progress = 1.0f;

        while (progress >= 0.0)
        {
            canvasGroup.alpha = Mathf.Lerp(0, 1, progress);

            progress -= rate * Time.deltaTime;

            yield return null;
        }
    }

    public Spell GetSpell(string spellName)
    {
        Spell spell = Array.Find(spells, x => x.Name == spellName);

        return spell;
    }
}
