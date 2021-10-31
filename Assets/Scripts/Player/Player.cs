using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField]
    private Stat mana;


    [SerializeField]
    private float maxMana;

    [SerializeField]
    private float manaValue;


    [SerializeField]
    private Transform[] exitPoints;

    private int exitIndex;

    Vector2 lastFacingDirection = Vector2.right;

    private SpellBook spellbook;

    public Transform MyTarget { get; set; }

    private Vector3 min, max;

    // Start is called before the first frame update
    protected override void Start()
    {
        spellbook = GetComponent<SpellBook>();
      
        mana.Initialize(manaValue, maxMana);

        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        GetInput();

        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, min.x, max.x),
            Mathf.Clamp(transform.position.y, min.y, max.y),
            transform.position.z);


        base.Update();
    }

   
    private void GetInput()
    {
        direction = Vector2.zero;


        //DEBUG ONLY
        if (Input.GetKeyDown(KeyCode.I))
        {
            mana.StatCurrentValue -= 6;
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            mana.StatCurrentValue += 6;
        }
        //DEBUG ONLY END

        if (Input.GetKey(KeyCode.W))
        {
            exitIndex = 0;
            direction += Vector2.up;

            lastFacingDirection = Vector2.up;
        }
        if (Input.GetKey(KeyCode.A))
        {
            exitIndex = 3;
            direction += Vector2.left;

            lastFacingDirection = Vector2.left;
        }
        if (Input.GetKey(KeyCode.S))
        {
            exitIndex = 2;
            direction += Vector2.down;

            lastFacingDirection = Vector2.down;
        }
        if (Input.GetKey(KeyCode.D))
        {
            exitIndex = 1;
            direction += Vector2.right;

            lastFacingDirection = Vector2.right;
        }
    }

    public void SetLimits(Vector3 min, Vector3 max)
    {
        this.min = min;
        this.max = max;
    }

    private IEnumerator Attack(int spellIndex)
    {
        Transform currentTarget = MyTarget;

        Spell s = spellbook.CastSpell(spellIndex);

        animator.SetBool("MagicAttack", true);
        IsAttacking = true;

        yield return new WaitForSeconds(s.CastTime);

        if(currentTarget != null && InLineOfSight())
        {
            SpellScript ss = Instantiate(s.SpellPrefab, exitPoints[exitIndex].position, Quaternion.identity).GetComponent<SpellScript>();

            ss.Initialize(currentTarget, s.Damage);
        }

        StopAttacking();     
    }

    public void CastSpell(int spellIndex)
    {
        if (MyTarget != null && !IsAttacking && !IsMoving && InLineOfSight())
        {
            MagicAttackRoutine = StartCoroutine(Attack(spellIndex));
        }
    }

    private bool InLineOfSight()
    {
        if (MyTarget != null)
        {
            Vector2 targetDirection = (MyTarget.transform.position - transform.position).normalized;

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

    public override void StopAttacking()
    {
        base.StopAttacking();

        spellbook.StopCasting();

    }
}