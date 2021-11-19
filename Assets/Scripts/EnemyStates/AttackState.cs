using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;

class AttackState: IState
{
    private Enemy parent;

    private float attackCooldown = 1;

    private float extraRange = .1f;

    public void Enter(Enemy parent)
    {
        this.parent = parent;
        parent.RigidBody.velocity = Vector2.zero;
        parent.Direction = Vector2.zero;
    }

    public void Update()
    {
        if (parent.AttackTime >= attackCooldown && !parent.IsAttacking)
        {
            parent.AttackTime = 0;
            parent.StartCoroutine(Attack());
        }

        if(parent.Target != null)
        {
            float distance = Vector2.Distance(parent.Target.transform.parent.position, parent.transform.parent.position);

            if(distance >= parent.AttackRange + extraRange && !parent.IsAttacking)
            {
                if(parent is RangedEnemy)
                {
                    parent.ChangeState(new PathState());
                }
                else
                {
                    parent.ChangeState(new FollowState());
                }
             
            }          
        }
        else
        {
            parent.ChangeState(new IdleState());
        }
    }

    public void Exit()
    {
        
    }

    public IEnumerator Attack()
    {
        parent.IsAttacking = true;

        parent.MyAnimator.SetTrigger("Attack");

        yield return new WaitForSeconds(parent.MyAnimator.GetCurrentAnimatorStateInfo(2).length);

        parent.IsAttacking = false;
    }
}
