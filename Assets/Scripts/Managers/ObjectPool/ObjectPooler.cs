using System;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

namespace Runner.Managers.ObjectPool
{
    public class ObjectPooler : Singleton<ObjectPooler>
    {
        [SerializeField] private PoolObject[] _objects;
        [SerializeField] private Dictionary<GameObject, Queue<GameObject>> _pool; // Prefab-Instance
        [SerializeField] private Dictionary<GameObject, GameObject> _prefabLookup; // Instance-Prefab

        public override void Awake()
        {
            base.Awake();

            Warm();
        }

        private void Warm()
        {
            _pool = new Dictionary<GameObject, Queue<GameObject>>();
            _prefabLookup = new Dictionary<GameObject, GameObject>();

            foreach (PoolObject data in _objects)
            {
                _pool[data.prefab] = new Queue<GameObject>();
                for (int i = 0; i < data.count; i++)
                {
                    var obj = Instantiate(data.prefab);
                    obj.SetActive(false);
                    obj.transform.parent = transform;

                    _pool[data.prefab].Enqueue(obj);
                    _prefabLookup[obj] = data.prefab;
                }
            }
        }

        public GameObject GetObject(GameObject prefab)
        {
            GameObject obj = null;
            if (_pool[prefab].Count == 0)
            {
                Debug.LogWarning($"[Object pool]: pool for {prefab} is empty.");
                obj = Instantiate(prefab);
            }
            else
            {
                obj = _pool[prefab].Dequeue();
            }

            _prefabLookup[obj] = prefab;
            obj.transform.parent = null;
            obj.SetActive(true);
            
            if (obj.TryGetComponent(out IPoolReaction reaction))
            {
                reaction.ObjectPooled(false);
            }

            return obj;
        }

        public void ReturnObject(GameObject obj)
        {
            if (obj.TryGetComponent(out IPoolReaction reaction))
            {
                reaction.ObjectPooled(true);
            }

            var prefab = _prefabLookup[obj];
            _pool[prefab].Enqueue(obj);
            obj.SetActive(false);
            obj.transform.parent = transform;
        }
        
    }

    [Serializable]
    public struct PoolObject
    {
        public GameObject prefab;
        public int count;
    }
}