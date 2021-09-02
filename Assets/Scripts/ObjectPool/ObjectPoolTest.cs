using Runner.ObjectPool;
using System.Collections;
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
            GameObject o = ObjectPooler.Inst.GetObject(obj);
            yield return new WaitForSeconds(.1f);
            ObjectPooler.Inst.ReturnObject(o);
        }
    }
}
