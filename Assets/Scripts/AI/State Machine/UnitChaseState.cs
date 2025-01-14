using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitChaseState : UnitState
{

    private Transform _targetTransform;
    private float _movementSpeed = 7f;

    public UnitChaseState(Base enemy, UnitStateMachine unitStateMachine) : base(enemy, unitStateMachine)
    {
        _targetTransform = GameObject.Find("Target").transform;
    }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        Vector3 moveDir = (_targetTransform.position - enemy.transform.position).normalized;
        enemy.MoveUnit(moveDir * _movementSpeed);

        if (!enemy.IsChasing)
            enemy.StateMachine.ChangeState(enemy.IdleState);
    }
}
