using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class EvadeState: IState
{
    private Enemy parent;
    private Transform transform;

    public void Enter(Enemy parent)
    {
        this.parent = parent;
        this.transform = parent.transform.parent;
        parent.Path = null;
    }

    public void Update()
    {
        parent.Direction = (parent.StartPosition - transform.position).normalized;

        float distance = Vector2.Distance(parent.StartPosition, transform.position);

        if(distance <= 0.1f)
        {
            parent.ChangeState(new IdleState());
        }
    }

    public void Exit()
    {
        parent.Direction = Vector2.zero;
        parent.Reset();
    }
}
