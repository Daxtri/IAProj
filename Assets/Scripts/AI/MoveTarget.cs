using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTarget : MonoBehaviour
{
    void Update()
    {
        Vector3 position = this.transform.position;

        if (Input.GetKey(KeyCode.A))
            position.x--;
        if (Input.GetKey(KeyCode.D))
            position.x++;
        if (Input.GetKey(KeyCode.S))
            position.z--;
        if (Input.GetKey(KeyCode.W))
            position.z++;

        this.transform.position = position;
    }
}
