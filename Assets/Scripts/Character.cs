using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField]
    private float speed;

    protected Vector2 direction;

    protected Animator animator;

    private Rigidbody2D Myrigidbody;

    public bool IsMoving { get { return direction.x != 0 || direction.y != 0; } }

    protected bool IsAttacking = false;
    protected Coroutine MagicAttackRoutine;


    // Start is called before the first frame update
    protected virtual void Start()
    {
        Myrigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        HandleLayers();
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        Myrigidbody.velocity = direction.normalized * speed;

        //AnimateMovement(direction);
    }

    public void HandleLayers()
    {
        if (IsMoving)
        {
            StopAttacking();

            ActivateLayer("WalkLayer");

            animator.SetFloat("x", direction.x);
            animator.SetFloat("y", direction.y);
        }
        else if (IsAttacking)
        {
            ActivateLayer("MagicAttackLayer");
        }
        else
        {
            ActivateLayer("IdleLayer");
        }
    }

    public void ActivateLayer(string layerName)
    {
        for(int i = 0; i < animator.layerCount; i++)
        {
            animator.SetLayerWeight(i, 0);
        }

        animator.SetLayerWeight(animator.GetLayerIndex(layerName), 1);

    }

    public void StopAttacking()
    {
        if(MagicAttackRoutine != null)
        {
            StopCoroutine(MagicAttackRoutine);

            IsAttacking = false;
            animator.SetBool("MagicAttack", IsAttacking);
        }
    }

}
