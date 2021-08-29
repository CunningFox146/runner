using System.Collections;
using System.Collections.Generic;
using Runner.Managers.ObjectPool;
using UnityEngine;

namespace Runner.Managers.World
{
    public class BackgroundGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject _cloud;
        [SerializeField] private Vector3 _cloudSpawnOffset;

        private Queue<GameObject> _cloudsCache;

        private void Awake()
        {
            _cloudsCache = new Queue<GameObject>();
        }

        private void Start()
        {

        }

        private void Warm()
        {

        }

        private void SpawnCloud(Vector3 pos)
        {
            var cloud = ObjectPooler.Inst.GetObject(_cloud);
            cloud.transform.position = pos;

            _cloudsCache.Enqueue(cloud);
        }
    }
}