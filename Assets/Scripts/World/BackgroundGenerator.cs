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
        [SerializeField] private float _startCount = 3f;

        private Queue<GameObject> _queue;
        private bool _isRight;

        protected virtual void Awake()
        {
            _queue = new Queue<GameObject>();
        }

        protected virtual void Start()
        {
            if (_startCount > 0f)
            {
                SpawnStartItems();
            }
        }

        protected virtual void Update()
        {
            if (_queue.Count > 0)
            {
                var obj = _queue.Peek();
                if (obj.transform.position.z < -5f)
                {
                    DestroyLast();
                    Spawn();
                }
            }

            float speed = GameManager.GameSpeed;
            foreach (GameObject island in _queue)
            {
                island.transform.Translate(-Vector3.forward * speed * Time.deltaTime, Space.World);
            }
        }

        protected virtual void DestroyLast()
        {
            var obj = _queue.Peek();
            _queue.Dequeue();
            ObjectPooler.Inst.ReturnObject(obj);
        }

        protected virtual void SpawnStartItems()
        {
            for (float i = 0f; i < _startCount; i++)
            {
                Spawn(i);
            }
        }
        
        protected virtual GameObject Spawn() => Spawn(_queue.Count);

        protected virtual GameObject Spawn(float idx)
        {
            var obj = ObjectPooler.Inst.GetObject(ArrayUtil.GetRandomItem(_items));
            _isRight = !_isRight;
            float mult = (_isRight) ? -1f : 1f;
            var pos = new Vector3(
                RandomUtil.Variance(_spawnOffset.x * mult, _spawnRng.x),
                RandomUtil.Variance(_spawnOffset.y, _spawnRng.y), 
                RandomUtil.Variance(_spawnSpacing * idx, _spawnRng.z));

            obj.transform.position = pos;
            obj.transform.rotation = Quaternion.Euler(_spawnRotation * mult);
            obj.transform.parent = transform;

            _queue.Enqueue(obj);

            return obj;
        }
    }
}