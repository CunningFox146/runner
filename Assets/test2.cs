using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test2 : MonoBehaviour
{
    void Update()
    {
        var pos = new Vector3(0f, 10f, 0f);
        if (Physics.Raycast(pos, -transform.up, out RaycastHit hit, 10f, 1 << 6))
        {
            Debug.DrawLine(pos, hit.point, Color.cyan);
            transform.position = new Vector3(pos.x, 0.5f + hit.point.y, pos.z);
        }
    }
}
