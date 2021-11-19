using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stat : MonoBehaviour
{

    protected Image content;

    [SerializeField]
    private float lerpSpeed;

    private float overflow;


    public float MaxValue { get; set; }

    public float StatCurrentValue { 
        get => CurrentValue;

        set {
            if (value > MaxValue)
            {
                overflow = value - MaxValue;
                CurrentValue = MaxValue;
            }
            else if (value < 0)
                CurrentValue = 0;
            else
            {
                CurrentValue = value;
            }
            currentFill = CurrentValue / MaxValue;
        }
    }

    public float Overflow { get
        {
            float tmp = overflow;
            overflow = 0;
            return tmp;
        }
     }

    private float CurrentValue;

    private float currentFill;

    public virtual void Initialize(float currentValue, float maxValue)
    {
        if(content == null)
        {
            content = GetComponent<Image>();
        }

        content.fillAmount = currentValue / maxValue;

        MaxValue = maxValue;
        StatCurrentValue = currentValue;
    }

    // Start is called before the first frame update
    void Start()
    {
        content = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleBar();
    }

    public virtual void HandleBar()
    {
        if (currentFill != content.fillAmount)
        {
            content.fillAmount = Mathf.MoveTowards(content.fillAmount, currentFill, Time.deltaTime * lerpSpeed);
        }
    }
}
