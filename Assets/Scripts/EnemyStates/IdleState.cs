using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class IdleState: IState
{
    private Enemy parent;

    public void Enter(Enemy parent)
    {
        this.parent = parent;
        //this.parent.Target = null;
        this.parent.Reset();
    }

    public void Update()
    {
        if (parent.Target != null)
        {
            parent.ChangeState(new PathState());
        }
    }

    public void Exit()
    {

    }
}
