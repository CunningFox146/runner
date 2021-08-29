using System.Collections.Generic;
using Runner.Managers.ObjectPool;
using Runner.Util;
using UnityEngine;

namespace Runner.Managers.World
{
    public class BackgroundGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject[] _items;
        [SerializeField] private float _spawnSpacing;
        [SerializeField] private Vector2 _spawnOffset;
        [SerializeField] private Vector3 _spawnRng;
        [SerializeField] private Vector3 _spawnRotation;
        [SerializeField] private float warmCount = 3f;

        private Queue<GameObject> _cache;

        protected virtual void Awake()
        {
            _cache = new Queue<GameObject>();
        }

        protected virtual void Start()
        {
            if (warmCount > 0f)
            {
                Warm();
            }
        }

        protected virtual void Update()
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

        protected virtual void DestroyLast()
        {
            var obj = _cache.Peek();
            _cache.Dequeue();
            ObjectPooler.Inst.ReturnObject(obj);
        }

        protected virtual void Warm()
        {
            for (float i = 0f; i < warmCount; i++)
            {
                Spawn(i);
            }
        }
        
        protected virtual GameObject Spawn() => Spawn(_cache.Count);

        protected virtual GameObject Spawn(float idx)
        {
            var obj = ObjectPooler.Inst.GetObject(ArrayUtil.GetRandomItem(_items));
            float mult = RandomUtil.RandomBool() ? -1f : 1f;
            var pos = new Vector3(
                RandomUtil.Variance(_spawnOffset.x * mult, _spawnRng.x),
                RandomUtil.Variance(_spawnOffset.y, _spawnRng.y), 
                RandomUtil.Variance(_spawnSpacing * idx, _spawnRng.z));

            obj.transform.position = pos;
            obj.transform.rotation = Quaternion.Euler(_spawnRotation * mult);
            obj.transform.parent = transform;

            _cache.Enqueue(obj);

            return obj;
        }
    }
}