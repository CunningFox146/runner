using System.Collections;
using System.Collections.Generic;
using Runner;
using UnityEngine;

public class test : Singleton<test>
{
    public float speed = 5f;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0f, 0f, -speed * Time.deltaTime);
    }
}
