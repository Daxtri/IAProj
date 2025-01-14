using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitIdleState : UnitState
{
    private Vector3 _targetpos;
    private Vector3 _direction;

    public UnitIdleState (Base enemy, UnitStateMachine unitStateMachine) : base(enemy, unitStateMachine)
    {

    }

    public override void EnterState()
    {
        base.EnterState();

        _targetpos = GetRandomPointInSquare();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    // Patrol behaviour
    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if (enemy.IsChasing)
            enemy.StateMachine.ChangeState(enemy.ChaseState);

        _direction = (_targetpos - enemy.transform.position).normalized;
        enemy.MoveUnit(_direction * enemy.RandMoveSpeed);

        if((enemy.transform.position - _targetpos).sqrMagnitude < 0.01f)
        {
            _targetpos = GetRandomPointInSquare();
        }
    }

    private Vector3 GetRandomPointInSquare()
    {
        float x = enemy.transform.position.x + Random.Range(-7,7);
        float y = enemy.transform.position.y;
        float z = enemy.transform.position.z + Random.Range(-7,7);

        return new Vector3(x, y, z);
    }
}
