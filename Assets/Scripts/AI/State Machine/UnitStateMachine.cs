using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStateMachine 
{
    public UnitState CurrentUnitState { get; set; }

    //which state to start in
    public void Initialize(UnitState startingState)
    {
        CurrentUnitState = startingState;
        CurrentUnitState.EnterState();
    }

    public void ChangeState(UnitState newState)
    {
        CurrentUnitState.ExitState();
        CurrentUnitState = newState;
        CurrentUnitState.EnterState();
    }
}
