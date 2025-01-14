using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitChaseCheck : MonoBehaviour
{
    public GameObject Target { get; set; }
    private Base _enemy;

    private void Awake()
    {
        Target = GameObject.Find("Target");
        _enemy = GetComponentInParent<Base>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject == Target)
            _enemy.SetChaseStatus(true);
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject == Target)
            _enemy.SetChaseStatus(false);
    }
}
