using System.Collections;
using System.Collections.Generic;
using Runner.Managers.ObjectPool;
using UnityEngine;

public class ObjectPoolTest : MonoBehaviour
{
    [SerializeField] private GameObject obj;

    void Start()
    {
        StartCoroutine(Test());
    }

    IEnumerator Test()
    {
        while (true)
        {
            var o = ObjectPooler.Inst.GetObject(obj);
            yield return new WaitForSeconds(.1f);
            ObjectPooler.Inst.ReturnObject(o);
        }
    }
}
