using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitState
{
    protected Base enemy;
    protected UnitStateMachine unitStateMachine;

    public UnitState(Base enemy, UnitStateMachine unitStateMachine)
    {
        this.enemy = enemy;
        this.unitStateMachine = unitStateMachine;
    }

    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void FrameUpdate() { }

}
