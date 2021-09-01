using Runner.Util;
using UnityEngine;

namespace Runner.World
{
    public static class BlockDecor
    {
        public static void Decorate(Transform block, GameObject decorPrefab, float spacing)
        {
            var obj = GameObject.Instantiate(decorPrefab, block);
            obj.transform.localPosition = new Vector3(Random.Range(-spacing, spacing), 0f, Random.Range(-spacing, spacing));
        }
    }
}