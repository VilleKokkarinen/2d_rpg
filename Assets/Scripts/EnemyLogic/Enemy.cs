using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void HealthChanged(float health);

public delegate void CharacterRemoved();

public class Enemy : Character, IInteractable
{
    public event HealthChanged healthChanged;

    public event CharacterRemoved characterRemoved;

    [SerializeField]
    protected int baseDamage;

    private bool canDoDamage = true;

    [SerializeField]
    private Sprite portrait;

    public Sprite Portrait { get => portrait; }


    [SerializeField]
    private CanvasGroup healthGroup;

    private IState currentState;

    [SerializeField]
    private LootTable lootTable;

    public float AttackTime { get; set; }

    [SerializeField]
    private float attackRange;

    [SerializeField]
    private float initAggroRange;

    public float AggroRange { get; set; }

    public bool InRange
    {
        get
        {
            if (Target == null)
                return false;

            return Vector2.Distance(transform.position, Target.transform.position) < AggroRange;
        }
    }

    public Vector3 StartPosition { get; set; }

    public AStar Astar { get => astar; }
    public float AttackRange { get => attackRange; set => attackRange = value; }

    [SerializeField]
    private AStar astar;

    [SerializeField]
    private LayerMask LOSMask;

    protected void Awake()
    {
        health.Initialize(healthValue, maxHealth);
        StartPosition = transform.parent.transform.position;
        AggroRange = initAggroRange;

        ChangeState(new IdleState());
    }

    protected override void Start()
    {
        base.Start();
        MyAnimator.SetFloat("x", 1);
        MyAnimator.SetFloat("y", 1);
    }

    protected override void Update()
    {
        if (IsAlive)
        {
            if (!IsAttacking)
            {
                AttackTime += Time.deltaTime;
            }

            currentState.Update();

            if(Target != null && !Player.Instance.IsAlive){
                ChangeState(new PathEvadeState());
            }
        }

        base.Update();
    }

  
    public void OnHealthChanged(float health)
    {
        if (healthChanged != null)
        {
            healthChanged(health);
        }
    }


    public void OnCharacterRemoved()
    {
        if (characterRemoved != null)
        {
            characterRemoved();
        }
        Destroy(gameObject);
    }

    public Character Select()
    {
        healthGroup.alpha = 1;

        return this;
    }

    public void DeSelect()
    {
        healthGroup.alpha = 0;

        healthChanged -= new HealthChanged(UIManager.Instance.UpdateTargetFrame);
        characterRemoved -= new CharacterRemoved(UIManager.Instance.HideTargetFrame);
    }

    public override void TakeDamage(float damage, Character source)
    {
        if(!(currentState is EvadeState))
        {
            if (IsAlive)
            {
                SetTarget(source);
                base.TakeDamage(damage, source);
                OnHealthChanged(health.StatCurrentValue);

                if (!IsAlive)
                {
                    Player.Instance.Attackers.Remove(this);
                    Player.Instance.GainXP(XPmanager.CalculateXP(this));
                }
            }          
        }
    }

    public void DoDamage()
    {
        if (canDoDamage)
        {
            Target.TakeDamage(baseDamage, this);
            canDoDamage = false;
        }     
    }

    public void CanDoDamage()
    {
        canDoDamage = true;
    }

    public void ChangeState(IState newState)
    {
        if(currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;

        currentState.Enter(this);
    }

    public void SetTarget(Character target)
    {
        if(Target == null && !(currentState is EvadeState))
        {
            float distance = Vector2.Distance(transform.position, target.transform.position);
            AggroRange = initAggroRange;

            AggroRange += distance;

            Target = target;
        }
    }

    public void Reset()
    {
        Target = null;
        AggroRange = initAggroRange;
        Health.StatCurrentValue = Health.MaxValue;
        OnHealthChanged(health.StatCurrentValue);

    }

    public void Interact()
    {
        if (!IsAlive)
        {
            List<Drop> drops = new List<Drop>();

            foreach (IInteractable interactable in Player.Instance.Interactables)
            {
                if(interactable is Enemy && !(interactable as Enemy).IsAlive)
                {
                    drops.AddRange((interactable as Enemy).lootTable.GetLoot());
                }
            }

            LootWindow.Instance.CreatePages(drops);
        }
    }

    public void StopInteract()
    {
        LootWindow.Instance.Close();
    }

    public bool CanSeePlayer()
    {
        Vector2 targetDirection = (Target.transform.position - transform.position).normalized;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection, Vector2.Distance(transform.position, Target.transform.position), LOSMask);

        if (hit.collider != null)
        {
            return false;
        }
        return true;
    }
}
