using System;
using System.Collections;
using System.Collections.Generic;
using Runner.Managers.ObjectPool;
using Runner.Managers.World;
using Runner.Util;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runner.Environment
{
    public class SetPiece : MonoBehaviour, IPoolReaction
    {
        public static Action<SetPiece> OnSetPieceExit;

        [SerializeField] public Transform pointStart;
        [SerializeField] public Transform pointEnd;

        [SerializeField] private Transform _tilesContainer;

        [SerializeField] private bool _isWarmingTiles;
        [SerializeField] private GameObject _warmingPrefab;
        
        private bool _isExitPushed;
        
        void Awake()
        {
            if (_isWarmingTiles)
            {
                WarmTiles();
            }
        }

        void Update()
        {
            if (pointEnd.position.z <= 0f && !_isExitPushed)
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
                for (float z = pointStart.position.z + tileSize * 0.5f; z < pointEnd.position.z; z += tileSize)
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
    }
}