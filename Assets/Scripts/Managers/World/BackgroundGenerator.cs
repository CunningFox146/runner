using System.Collections;
using System.Collections.Generic;
using Runner.Managers.ObjectPool;
using Runner.Util;
using UnityEngine;

namespace Runner.Managers.World
{
    public class BackgroundGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject[] _islands;
        [SerializeField] private float _spawnOffset;
        [SerializeField] private Vector3 _spawnRng;
        [SerializeField] private Vector3 _spawnRotation;
        [SerializeField] private float warmCount = 3f;

        private Queue<GameObject> _cache;
        private Transform _rotationTarget;

        private void Awake()
        {
            _cache = new Queue<GameObject>();
            _rotationTarget = Camera.main.transform;
        }

        private void Start()
        {
            if (warmCount > 0f)
            {
                Warm();
            }
        }

        private void Update()
        {
            if (_cache.Count > 0)
            {
                var obj = _cache.Peek();
                if (obj.transform.position.z < -10f)
                {
                    DestroyLast();
                    Spawn();
                }
            }

            float speed = GameManager.GameSpeed;
            foreach (GameObject island in _cache)
            {
                island.transform.Translate(-Vector3.forward * speed * Time.deltaTime, Space.World);
            }
        }

        private void DestroyLast()
        {
            var obj = _cache.Peek();
            _cache.Dequeue();
            ObjectPooler.Inst.ReturnObject(obj);
        }

        private void Warm()
        {
            for (float i = 0f; i < warmCount; i++)
            {
                Spawn(i);
            }
        }

        private Vector3 GetSpawnPos(float idx = 1f)
        {
            var offset = new Vector3(RandomUtil.Variance(0f, _spawnRng.x),
                RandomUtil.Variance(0f, _spawnRng.y), RandomUtil.Variance(0f, _spawnRng.z));
            return offset + new Vector3(_spawnOffset * (RandomUtil.RandomBool() ? -1f : 1f), 0f, 35f * idx);
        }

        private GameObject Spawn() => Spawn(_cache.Count);

        private GameObject Spawn(float idx)
        {
            float mult = RandomUtil.RandomBool() ? -1f : 1f;
            var offset = new Vector3(RandomUtil.Variance(0f, _spawnRng.x),
                RandomUtil.Variance(0f, _spawnRng.y), RandomUtil.Variance(0f, _spawnRng.z));
            var pos = offset + new Vector3(_spawnOffset * mult, 0f, 35f * idx);

            var obj = ObjectPooler.Inst.GetObject(ArrayUtil.GetRandomItem(_islands));
            obj.transform.position = pos;
            obj.transform.rotation = Quaternion.Euler(_spawnRotation * mult);
            obj.transform.parent = transform;

            _cache.Enqueue(obj);

            return obj;
        }
    }
}