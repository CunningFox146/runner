using System;
using System.Collections;
using System.Collections.Generic;
using Runner.Managers.ObjectPool;
using Runner.Util;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runner.Environment
{
    public class SetPiece : MonoBehaviour, IPoolReaction
    {
        public static Action<SetPiece> OnSetPieceExit;

        [SerializeField] private Transform _pointStart;
        [SerializeField] private Transform _pointEnd;
        [SerializeField] private Transform _tilesContainer;

        [SerializeField] private bool _isWarmingTiles;
        [SerializeField] private GameObject _warmingPrefab;

        [SerializeField] private float _grassChance = 0.5f;
        [SerializeField] private GameObject[] _grass;

        private bool _isExitPushed;

        public float Length => _pointEnd.position.z - _pointStart.position.z;
        
        void Awake()
        {

            if (_isWarmingTiles)
            {
                WarmTiles();
            }

            if (_grass != null && _grass.Length > 0)
            {
                Decorate();
            }
        }

        void Update()
        {
            if (_pointEnd.position.z <= 0f && !_isExitPushed)
            {
                OnSetPieceExit?.Invoke(this);
                _isExitPushed = true;
            }
        }

        public void ObjectPooled(bool inPool) =>_isExitPushed = false;

        public List<Transform> GetCachedTiles()
        {
            var cache = new List<Transform>();
            foreach (Transform child in _tilesContainer)
            {
                cache.Add(child);
            }

            return cache;
        }

        // Round positions to properly check tiles
        private bool IsTileCached(List<Transform> cache, Vector3 pos)
        {
            pos = new Vector3(Mathf.Round(pos.x), 0f, Mathf.Round(pos.z));

            foreach (Transform tile in cache)
            {
                var rounded = new Vector3(Mathf.Round(tile.position.x), 0f, Mathf.Round(tile.position.z));
                if (rounded == pos)
                {
                    return true;
                }
            }

            return false;
        }

        public List<Vector3> GetMissingTiles()
        {
            float tileSize = 2f;
            var list = new List<Vector3>();
            var cache = GetCachedTiles();
            
            for (float x = -tileSize; x <= tileSize; x+= tileSize)
            {
                for (float z = _pointStart.position.z + tileSize * 0.5f; z < _pointEnd.position.z; z += tileSize)
                {
                    var pos = new Vector3(x, 0f, z);
                    if (!IsTileCached(cache, pos))
                    {
                        list.Add(pos);
                    }
                }
            }

            return list;
        }
        
        private void WarmTiles()
        {
            foreach (Vector3 pos in GetMissingTiles())
            {
                Instantiate(_warmingPrefab, _tilesContainer).transform.position = pos;
            }
        }

        private void Decorate()
        {
            const float spacing = 0.9f; // tile scale is 2f
            foreach (Transform tile in GetCachedTiles())
            {
                if (!RandomUtil.RandomBool(_grassChance)) continue;

                var grass = Instantiate(ArrayUtil.GetRandomItem(_grass), tile);
                grass.transform.localPosition = new Vector3(Random.Range(-spacing, spacing), 0f, Random.Range(-spacing, spacing));
            }
        }
    }
}