using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellBook : MonoBehaviour
{
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

    public Spell CastSpell(int index)
    {
        if (fadeOutRoutine != null)
        {
            StopCoroutine(fadeOutRoutine);
            fadeOutRoutine = null;
        }

        castingBar.color = spells[index].BarColor;
        spellName.text = spells[index].Name;
        spellIcon.sprite = spells[index].Icon;

        castingBar.fillAmount = 0;
        castTime.text = "0,0";

        spellRoutine = StartCoroutine(Progress(index));
        fadeInRoutine = StartCoroutine(FadeBarIn());

        return spells[index];
    }

    private IEnumerator Progress(int index)
    {
        float timeLeft = Time.deltaTime;

        float rate = 1.0f / spells[index].CastTime;

        float progress = 0.0f;

        while (progress <= 1.0)
        {
            castingBar.fillAmount = Mathf.Lerp(0, 1, progress);

            progress += rate * Time.deltaTime;

            timeLeft += Time.deltaTime;

            castTime.text = (spells[index].CastTime - timeLeft).ToString("F1");

            if (spells[index].CastTime - timeLeft < 0)
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
}
