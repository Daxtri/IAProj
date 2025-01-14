using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITriggerCheck
{
    bool IsChasing { get; set; }

    void SetChaseStatus(bool isChasing);
}