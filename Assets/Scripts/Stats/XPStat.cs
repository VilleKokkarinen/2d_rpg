using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XPStat : Stat
{

    [SerializeField]
    private Text text;

    public bool IsFull
    {
        get
        {
            return content.fillAmount == 1;
        }
    }


    public void ResetFill()
    {
        content.fillAmount = StatCurrentValue / MaxValue;
    }


    public override void Initialize(float c, float m)
    {
        base.Initialize(c,m);

        string current = c.ToString();
        string currentxpText = string.Empty;

        string max = m.ToString();
        string maxxpText = string.Empty;

        if (c > 1000000)
        {
            currentxpText = current[0] + "," + current[1] + "M";
        }
        else if (c > 10000)
        {
            currentxpText = current[0] + "," + current[1] + "K";
        }
        else
        {
            currentxpText = current;
        }

        if (m > 1000000)
        {
            maxxpText = max[0] + "," + max[1] + "M";
        }
        else if (m > 10000)
        {
            maxxpText = max[0] + "," + max[1] + "K";
        }
        else
        {
            maxxpText = max;
        }

        text.text = currentxpText + "/" + maxxpText;
    }

    void Update()
    {
        HandleBar();
    }

    public override void HandleBar()
    {
        base.HandleBar();

        string current = StatCurrentValue.ToString();
        string currentxpText = string.Empty;

        string max = MaxValue.ToString();
        string maxxpText = string.Empty;

        if (StatCurrentValue > 1000000)
        {
            currentxpText = current[0] + "," + current[1] + "M";
        }
        else if (StatCurrentValue > 10000)
        {
            currentxpText = current[0] + "," + current[1] + "K";
        }
        else
        {
            currentxpText = current;
        }

        if (MaxValue > 1000000)
        {
            maxxpText = max[0] + "," + max[1] + "M";
        }
        else if (MaxValue > 10000)
        {
            maxxpText = max[0] + "," + max[1] + "K";
        }
        else
        {
            maxxpText = max;
        }

        text.text = currentxpText + "/" + maxxpText;
    }
}
