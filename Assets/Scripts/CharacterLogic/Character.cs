using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public abstract class Character : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private Vector2 direction;

    public Animator MyAnimator { get; set; }

    [SerializeField]
    private Rigidbody2D rigidBody;

    [SerializeField]
    protected Stat health;

    public Stat Health
    {
        get { return health; }
    }

    public bool IsMoving { get { return Direction.x != 0 || Direction.y != 0; } }

    public Vector2 Direction { get => direction; set => direction = value; }

    public float Speed { get => speed; set => speed = value; }

    public bool IsAttacking { get; set; }

    protected Coroutine actionRoutine;

    [SerializeField]
    private Transform hitBox;


    [SerializeField]
    protected float maxHealth;

    [SerializeField]
    protected float healthValue;

    public Character Target { get; set; }

    [SerializeField]
    private string type;
    public bool IsAlive
    {
        get
        {
           return Health.StatCurrentValue > 0;
        }
    }

    public string Type { get => type; set => type = value; }
    public int Level { get => level; set => level = value; }

    [SerializeField]
    private int level;

    public Transform CurrentTile { get; set; }
    public Rigidbody2D RigidBody { get => rigidBody; }

    public Stack<Vector3> Path { get; set; }

    public SpriteRenderer spriteRenderer { get; set; }
    public Transform HitBox { get => hitBox; set => hitBox = value; }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        //direction = Vector2.zero;
        //IsAttacking = false;
        MyAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {      
        if (Path == null)
        {
            if (IsAlive)
            {
                RigidBody.velocity = direction * Speed;
            }
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        HandleLayers();
    }


    public virtual void HandleLayers()
    {
        if (IsAlive)
        {
            if (IsMoving)
            {
                ActivateLayer("WalkLayer");

                MyAnimator.SetFloat("x", Direction.x);
                MyAnimator.SetFloat("y", Direction.y);
            }
            else if (IsAttacking)
            {
                ActivateLayer("AttackLayer");
            }
            else
            {
                ActivateLayer("IdleLayer");
            }
        }
        else
        {
            ActivateLayer("DeathLayer");
        }
       
    }

    public virtual void ActivateLayer(string layerName)
    {
        for(int i = 0; i < MyAnimator.layerCount; i++)
        {
            MyAnimator.SetLayerWeight(i, 0);
        }

        MyAnimator.SetLayerWeight(MyAnimator.GetLayerIndex(layerName), 1);

    }

    public void GetHealth(int health)
    {
        Health.StatCurrentValue += health;
        CombatTextManager.Instance.CreateText(transform.position, health.ToString(), SCTTYPE.GainHP, false);
    }

  

    public virtual void TakeDamage(float damage, Character source)
    {
        health.StatCurrentValue -= damage;
        CombatTextManager.Instance.CreateText(transform.position, damage.ToString(), SCTTYPE.LoseHP, false);

        if (health.StatCurrentValue <= 0)
        {
            Direction = Vector2.zero;
            RigidBody.velocity = Direction;

            GameManager.Instance.OnKillConfirmed(this);

            MyAnimator.SetTrigger("Die");          
        }
    }

}
