using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour, IMoveable, ITriggerCheck
{

    public Rigidbody RB { get; set; }

    public bool IsChasing { get; set; }

    public UnitStateMachine StateMachine { get; set; }
    public UnitIdleState IdleState { get; set; }
    public UnitChaseState ChaseState { get; set; }

    // idle variable
    public float MoveSpeed = 5f;


    private void Awake()
    {
        StateMachine = new UnitStateMachine();

        IdleState = new UnitIdleState(this, StateMachine);
        ChaseState = new UnitChaseState(this, StateMachine);
    }
    private void Start()
    {
        RB = GetComponent<Rigidbody>();

        StateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        StateMachine.CurrentUnitState.FrameUpdate();
    }

    // movement function
    public void MoveUnit(Vector3 velocity)
    {
        RB.velocity = velocity;
    }

    // distance check
    public void SetChaseStatus(bool isChasing)
    {
        IsChasing = isChasing;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, 25f);
    }
}
