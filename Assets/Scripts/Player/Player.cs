using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character
{
    private static Player instance;
    public static Player Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Player>();
            }

            return instance;
        }
    }
    private Vector2 initPosition;



    [SerializeField]
    private Stat mana;

    public Stat Mana
    {
        get { return mana; }
    }

    [SerializeField]
    private float maxMana;

    [SerializeField]
    private float manaValue;


    [SerializeField]
    private XPStat xp;


    [SerializeField]
    private Transform[] exitPoints;

    private int exitIndex;

    Vector2 lastFacingDirection = Vector2.right;

    private Vector3 min, max;

    [SerializeField]
    private GearSocket[] gearSockets;

    private List<IInteractable> interactables = new List<IInteractable>();

    public int Coins { get; set; }
    public XPStat Xp { get => xp; }
    public List<IInteractable> Interactables { get => interactables; set => interactables = value; }
    public List<Enemy> Attackers { get => attackers; set => attackers = value; }
    public Coroutine InitRoutine { get => initRoutine; set => initRoutine = value; }

    [SerializeField]
    private Text levelText;

    [SerializeField]
    private Animator lvlUpAnimator;

    private List<Enemy> attackers = new List<Enemy>();

    [SerializeField]
    private Transform minimapIcon;

    private Coroutine initRoutine;

    [SerializeField]
    private CraftWindow profession;

    #region pathfinding
    private Vector3 destination;

    private Vector3 current;

    private Vector3 goal;

    [SerializeField]
    private AStar astar;
    #endregion

    [SerializeField]
    private SpriteRenderer[] gearRenderers;

    public void SetDefaultValues()
    {
        mana.Initialize(manaValue, maxMana);
        health.Initialize(healthValue, maxHealth);
        Xp.Initialize(0, Mathf.Floor(100 * Level * Mathf.Pow(Level, 0.5f)));
        Coins = 10;
        levelText.text = Level.ToString();
        initPosition = transform.parent.position;
    }

    // Update is called once per frame
    protected override void Update()
    {
        GetInput();
        ClickToMove();

        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, min.x, max.x),
            Mathf.Clamp(transform.position.y, min.y, max.y),
            transform.position.z);


        base.Update();
    }

    private void GetInput()
    {        
        Direction = Vector2.zero;

        //DEBUG ONLY
        if (Input.GetKeyDown(KeyCode.I))
        {
            mana.StatCurrentValue -= 6;
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            mana.StatCurrentValue += 6;
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            GainXP(20);
        }
        //DEBUG ONLY END

        if (Input.GetKey(KeybindManager.Instance.Keybinds["UP"]))
        {
            exitIndex = 0;
            Direction += Vector2.up;

            lastFacingDirection = Vector2.up;
            minimapIcon.eulerAngles = new Vector3(0, 0, 0);
        }
        if (Input.GetKey(KeybindManager.Instance.Keybinds["LEFT"]))
        {
            exitIndex = 3;
            Direction += Vector2.left;

            lastFacingDirection = Vector2.left;

            if(Direction.y == 0)
            {
                minimapIcon.eulerAngles = new Vector3(0, 0, 90);
            }
            else if (Direction.y == 1)
            {
                minimapIcon.eulerAngles = new Vector3(0, 0, 45);
            }
          
        }
        if (Input.GetKey(KeybindManager.Instance.Keybinds["DOWN"]))
        {
            exitIndex = 2;
            Direction += Vector2.down;

            lastFacingDirection = Vector2.down;

            if (Direction.x == -1)
            {
                minimapIcon.eulerAngles = new Vector3(0, 0, 135);
            }
            else
            {
                minimapIcon.eulerAngles = new Vector3(0, 0, 180);
            }
        }
        if (Input.GetKey(KeybindManager.Instance.Keybinds["RIGHT"]))
        {
            exitIndex = 1;
            Direction += Vector2.right;

            lastFacingDirection = Vector2.right;

            if (Direction.y == 0)
            {
                minimapIcon.eulerAngles = new Vector3(0, 0, 270);
            }
            else if (Direction.y == 1)
            {
                minimapIcon.eulerAngles = new Vector3(0, 0, 315);
            }
            else if (Direction.y == -1)
            {
                minimapIcon.eulerAngles = new Vector3(0, 0, 225);
            }
        }

        if (IsMoving)
        {
            StopAction();
            StopInit();
        }

        foreach(string action in KeybindManager.Instance.ActionBinds.Keys)
        {
            if (Input.GetKeyDown(KeybindManager.Instance.ActionBinds[action]))
            {
                UIManager.Instance.ClickActionButton(action);
            }
        }
    }

    public void SetLimits(Vector3 min, Vector3 max)
    {
        this.min = min;
        this.max = max;
    }

    private IEnumerator AttackRoutine(ICastable castable)
    {
        Transform currentTarget = Target.HitBox;

        yield return actionRoutine = StartCoroutine(ActionRoutine(castable));

        if(currentTarget != null && InLineOfSight())
        {
            Spell s = SpellBook.Instance.GetSpell(castable.Title);

            SpellScript ss = Instantiate(s.SpellPrefab, exitPoints[exitIndex].position, Quaternion.identity).GetComponent<SpellScript>();

            ss.Initialize(currentTarget, s.Damage, this);
        }

        StopAction();     
    }

    private IEnumerator GatherRoutine(ICastable castable, List<Drop> drops)
    {
        yield return actionRoutine = StartCoroutine(ActionRoutine(castable));

        StopAction();

        LootWindow.Instance.CreatePages(drops);
    }

    public IEnumerator CraftRoutine(ICastable castable)
    {
        yield return actionRoutine = StartCoroutine(ActionRoutine(castable));

        profession.AddItemsToInventory();

        StopAction();
    }

    private IEnumerator ActionRoutine(ICastable castable)
    {
        SpellBook.Instance.Cast(castable);

        IsAttacking = true;

        MyAnimator.SetBool("Attack", true); // change to gather

        foreach (GearSocket g in gearSockets)
        {
            g.animator.SetBool("Attack", true); // change to gather
        }

        yield return new WaitForSeconds(castable.CastTime);

        StopAction();

    }

    public void CastSpell(ICastable castable)
    {
        if (Target != null && Target.GetComponentInParent<Character>().IsAlive && !IsAttacking && !IsMoving && InLineOfSight())
        {
            initRoutine = StartCoroutine(AttackRoutine(castable));
        }
    }

    public void Gather(ICastable castable, List<Drop> drops)
    {
        if (!IsAttacking)
        {
            initRoutine = StartCoroutine(GatherRoutine(castable, drops));
        }
    }

    private bool InLineOfSight()
    {
        if (Target != null)
        {
            Vector2 targetDirection = (Target.transform.position - transform.position).normalized;

            float angle = Vector3.Angle(targetDirection, lastFacingDirection);

            if (angle > 90)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            return false;
        }
    }

    public void StopAction()
    {
        IsAttacking = false;

        MyAnimator.SetBool("Attack", IsAttacking);

        foreach (GearSocket g in gearSockets)
        {
            g.animator.SetBool("Attack", IsAttacking);
        }

        if (actionRoutine != null)
        {
            StopCoroutine(actionRoutine);
        }

        SpellBook.Instance.StopCasting();
    }

    private void StopInit()
    {
        if(initRoutine != null)
        {
            StopCoroutine(initRoutine);
        }
    }

    public override void HandleLayers()
    {
        base.HandleLayers();

        if (IsMoving)
        {
            foreach (GearSocket g in gearSockets)
            {
                g.SetXY(Direction.x, Direction.y);
            }
        }
    }

    public override void ActivateLayer(string layerName)
    {
        base.ActivateLayer(layerName);

        foreach (GearSocket g in gearSockets)
        {
            g.ActivateLayer(layerName);
        }
    }
    
    public void GetMana(int mana)
    {
        Mana.StatCurrentValue += mana;
        CombatTextManager.Instance.CreateText(transform.position, mana.ToString(), SCTTYPE.GainMana, true);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {        
        if(collision.tag == "Enemy" || collision.tag == "Interactable")
        {
            IInteractable entity = collision.GetComponent<IInteractable>();

            if (!interactables.Contains(entity))
            {
                interactables.Add(entity);
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" || collision.tag == "Interactable")
        {
            if(interactables.Count > 0)
            {
                IInteractable entity = interactables.Find(x => x == collision.GetComponent<IInteractable>());

                if(entity != null)
                {
                    entity.StopInteract();                    
                }
                interactables.Remove(entity);
            }
        }
    }

    public void AddAttacker(Enemy enemy)
    {
        if (!attackers.Contains(enemy))
        {
            attackers.Add(enemy);
        }
    }

    public void GainXP(int amount)
    {
        Xp.StatCurrentValue += amount;

        CombatTextManager.Instance.CreateText(transform.position, amount.ToString(), SCTTYPE.XPGain, false);


        if(Xp.StatCurrentValue >= Xp.MaxValue)
        {
            StartCoroutine(LvlUpAlert());
        }
    }

    private IEnumerator LvlUpAlert()
    {
        while (!Xp.IsFull)
        {
            yield return null;
        }

        Level++;
        lvlUpAnimator.SetTrigger("LvlUp");

        levelText.text = Level.ToString();

        Xp.MaxValue = Mathf.Floor(100 * Level * Mathf.Pow(Level, 0.5f));

        Xp.StatCurrentValue = Xp.Overflow;
        Xp.ResetFill();

        if (Xp.StatCurrentValue >= Xp.MaxValue)
        {
            StartCoroutine(LvlUpAlert());
        }
    }

    public void UpdateLevel()
    {
        levelText.text = Level.ToString();
    }

  

    private void ClickToMove()
    {
        if(Path != null)
        {
            transform.parent.position = Vector2.MoveTowards(transform.parent.position, destination, Speed * Time.deltaTime);

            Vector3Int dest = astar.Tilemap.WorldToCell(destination);

            Vector3Int cur = astar.Tilemap.WorldToCell(current);

            if(cur.y > dest.y)
            {
                Direction = Vector2.down;
            }
            else if (cur.y < dest.y)
            {
                Direction = Vector2.up;
            }

            if (cur.y == dest.y)
            {
                if (cur.x >= dest.x)
                {
                    Direction = Vector2.left;
                }
                else if (cur.x <= dest.x)
                {
                    Direction = Vector2.right;
                }
            }


            float distance = Vector2.Distance(destination, transform.parent.position);

            if(distance <= 0f)
            {
                if(Path.Count > 0)
                {
                    current = destination;
                    destination = Path.Pop();
                }
                else
                {
                    Path = null;
                }
            }

        }
    }

    public void GetPath(Vector3 goal)
    {
        Path = null;
        Path = astar.Algorithm(transform.position, goal);

        if(Path != null) // otherwise you clicked water or something youre not supposed to walk on
        {
            current = Path.Pop();
            destination = Path.Pop();
            this.goal = goal;
        }       
    }

    public IEnumerator Respawn()
    {
        spriteRenderer.enabled = false;
        yield return new WaitForSeconds(5);
        health.Initialize(healthValue, healthValue);
        mana.Initialize(manaValue, manaValue);

        transform.parent.position = initPosition;
        spriteRenderer.enabled = true;
        MyAnimator.SetTrigger("Respawn");

        foreach (var rndr in gearRenderers)
        {
            rndr.enabled = true;
        }
    }

    public void HideGear()
    {
        foreach (var rndr in gearRenderers)
        {
            rndr.enabled = false;
        }
    }

}