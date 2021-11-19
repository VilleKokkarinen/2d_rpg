using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class FollowState: IState
{
    private Enemy parent;

    private Vector3 offset;

    public void Enter(Enemy parent)
    {
        Player.Instance.AddAttacker(parent);
        this.parent = parent;
        parent.Path = null;
    }

    public void Update()
    {
        if(parent.Target != null)
        {
            parent.Direction = ((parent.Target.transform.position + offset) - parent.transform.position).normalized;

            float distance = Vector2.Distance(parent.Target.transform.position+offset, parent.transform.position);

            string animName = parent.spriteRenderer.sprite.name;

            if (animName.Contains("right"))
            {
               //offset = new Vector3(0f, 0.85f);
            }
            else if (animName.Contains("left"))
            {
                //offset = new Vector3(0f, 0.85f);
            }
            else if (animName.Contains("up"))
            {
                //offset = new Vector3(0f, 0.85f);
            }
            else if (animName.Contains("down"))
            {
                //offset = new Vector3(0f, 0f);
            }


            if (distance <= parent.AttackRange)
            {
                parent.ChangeState(new AttackState());
            }
        }
        if (!parent.InRange)
        {
            Debug.Log("Lost target");
            parent.ChangeState(new PathEvadeState());
        }
        else if (!parent.CanSeePlayer())
        {
            parent.ChangeState(new PathState());
        }

    }

    public void Exit()
    {
        parent.Direction = Vector2.zero;
        parent.RigidBody.velocity = Vector2.zero;
        //parent.Path = null;
    }
}
