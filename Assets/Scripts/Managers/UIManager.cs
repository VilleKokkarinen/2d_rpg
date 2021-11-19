using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<UIManager>();
            }

            return instance;
        }
    }

    [SerializeField]
    private ActionButton[] actionButtons;

    [SerializeField]
    private GameObject targetPortraitFrame;

    [SerializeField]
    private GameObject tooltip;

    private Stat healthStat;

    [SerializeField]
    private Image targetPortraitImage;

    [SerializeField]
    private Text targetLevelText;

    [SerializeField]
    private CanvasGroup[] menus;

    [SerializeField]
    private CanvasGroup keybindMenu;

    [SerializeField]
    private CanvasGroup spellBook;

    private GameObject[] keybindButtons;

    private Text tooltipText;

    [SerializeField]
    private CharacterPanel characterPanel;

    private void Awake()
    {
        keybindButtons = GameObject.FindGameObjectsWithTag("Keybind");
        tooltipText = tooltip.GetComponentInChildren<Text>();
    }

    // Start is called before the first frame update
    void Start()
    {
        healthStat = targetPortraitFrame.GetComponentInChildren<Stat>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenSingle(menus[0]); // main menu
        }
        if (Input.GetKeyDown(KeyCode.F5))
        {
            OpenSingle(menus[1]); // savegame
        }
        if (Input.GetKeyDown(KeyCode.F6))
        {
            OpenSingle(menus[2]); // keybinds
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            OpenSingle(menus[3]); // spellbook
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            OpenSingle(menus[4]); // craftmenu
        }
        if (Input.GetKeyDown(KeyCode.I)) 
        {
            InventoryScript.Instance.OpenClose(); // inventory
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            OpenSingle(menus[5]); // gear / char panel
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            OpenSingle(menus[6]); // quest log
        }
    }

    public void ClickActionButton(string btnName)
    {
        Array.Find(actionButtons, x => x.gameObject.name == btnName).button.onClick.Invoke();
    }

    public void ShowTargetFrame(Enemy target)
    {
        targetPortraitFrame.SetActive(true);

        healthStat.Initialize(target.Health.StatCurrentValue, target.Health.MaxValue);

        targetPortraitImage.sprite = target.Portrait;

        targetLevelText.text = target.Level.ToString();

        target.healthChanged += new HealthChanged(UpdateTargetFrame);

        target.characterRemoved += new CharacterRemoved(HideTargetFrame);

        if (target.Level >= Player.Instance.Level +5)
        {
            targetLevelText.color = Color.red;

        }
        else if (target.Level == Player.Instance.Level +3 || target.Level == Player.Instance.Level +4)
        {
            targetLevelText.color = new Color32(255, 124, 0, 255);
        }
        else if (target.Level >= Player.Instance.Level -2 && target.Level <= Player.Instance.Level +2)
        {
            targetLevelText.color = Color.yellow;
        }
        else if ( target.Level <= Player.Instance.Level -3 && target.Level > XPmanager.CalculateGrayMobLevel())
        {
            targetLevelText.color = Color.green;
        }
        else
        {
            targetLevelText.color = Color.grey;
        }
    }

    public void HideTargetFrame()
    {
        targetPortraitFrame.SetActive(false);
    }

    public void UpdateTargetFrame(float value)
    {
        healthStat.StatCurrentValue = value;
    }

    public void UpdateKeyText(string key, KeyCode code)
    {
        Text tmp = Array.Find(keybindButtons, x => x.name == key).GetComponentInChildren<Text>();
        tmp.text = code.ToString();

    }

    public void OpenClose(CanvasGroup canvasgroup)
    {
        canvasgroup.alpha = canvasgroup.alpha > 0 ? 0 : 1;

        canvasgroup.blocksRaycasts = canvasgroup.blocksRaycasts == true ? false : true;
    }
    public void OpenSingle(CanvasGroup canvasgroup)
    {
        bool shouldClose = canvasgroup.blocksRaycasts == true ? false : true;

        foreach (var cg in menus)
        {
            CloseSingle(cg);
        }

        if (shouldClose)
        {
            canvasgroup.alpha = 1;
            canvasgroup.blocksRaycasts = true;
        }
        else
        {
            canvasgroup.alpha = 0;
            canvasgroup.blocksRaycasts = false;
        }
        
    }
    public void CloseSingle(CanvasGroup canvasgroup)
    {
        canvasgroup.alpha = 0;

        canvasgroup.blocksRaycasts = false;
    }
    public void UpdateStackSize(IClickable clickable)
    {
        if(clickable.Count > 1)
        {
            clickable.StackText.text = clickable.Count.ToString();
            clickable.StackText.color = Color.white;
            clickable.Icon.color = Color.white;
        }
        else
        {
            clickable.StackText.color = new Color(0, 0, 0, 0);
            clickable.Icon.color = Color.white;
        }

        if(clickable.Count == 0)
        {
            clickable.Icon.color = new Color(0, 0, 0, 0);
            clickable.StackText.color = new Color(0, 0, 0, 0);
        }
    }

    public void ClearStackSize(IClickable clickable)
    {
        clickable.StackText.color = new Color(0, 0, 0, 0);
        clickable.Icon.color = Color.white;
    }

    public void ShowTooltip(Vector3 position, IDescribable description)
    {
        tooltip.SetActive(true);
        tooltipText.text = description.GetDescription();

        Canvas.ForceUpdateCanvases();

        RectTransform ToolTipRect = tooltip.GetComponent<RectTransform>();
        RectTransform BGrect = GameObject.Find("UI").GetComponent<RectTransform>();      


        Vector2 anchoredPosition = new Vector2(position.x, position.y);
        Vector2 ToolTipCanvasAnchoredPosition = new Vector2(ToolTipRect.anchoredPosition.x, ToolTipRect.anchoredPosition.y);

        bool xoverlap = false;
        bool yoverlap = false;

        float BGHeight = BGrect.rect.height / BGrect.localScale.y;


        if (((position.x / BGrect.localScale.x) - ToolTipRect.rect.width) < 0)
        {
            anchoredPosition.x = ToolTipRect.rect.width + ToolTipRect.rect.width/2;
            xoverlap = true;
        }       
        if((anchoredPosition.y + ToolTipCanvasAnchoredPosition.y + ToolTipRect.rect.height) > BGHeight)
        {
            anchoredPosition.y = BGrect.rect.height - ToolTipRect.rect.height / BGrect.localScale.y;
            yoverlap = true;
        }

        if (yoverlap && xoverlap) // tooltip would be overlapping on topleft corner and placed on top of the object
        {
            anchoredPosition.x = 40 + position.x + ToolTipRect.rect.width + ToolTipRect.rect.width / 2;
        }      

        tooltip.transform.position = anchoredPosition;
        tooltip.SetActive(false);
        tooltip.SetActive(true);


    }
    public void HideTooltip()
    {
        tooltip.SetActive(false);
        tooltipText.text = null;
    }

    public void RefreshToolTip(IDescribable description)
    {
        tooltipText.text = description.GetDescription();

    }


}
