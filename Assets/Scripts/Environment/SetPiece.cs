using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.Environment
{
    public class SetPiece : MonoBehaviour
    {
        public static Action<SetPiece> OnSetPieceExit;

        [SerializeField] private Transform _pointStart;
        [SerializeField] private Transform _pointEnd;
        [SerializeField] private Transform _tilesContainer;

        [SerializeField] private bool _isWarmingTiles;
        [SerializeField] private GameObject _warmingPrefab;

        private bool _isExitPushed;

        public float Length => _pointEnd.position.z - _pointStart.position.z;


        void Awake()
        {
            if (_isWarmingTiles)
            {
                WarmTiles();
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

        public List<Vector3> GetCachedTiles()
        {
            var cache = new List<Vector3>();
            foreach (Transform child in _tilesContainer)
            {
                cache.Add(child.position);
            }

            return cache;
        }

        // Round positions to properly check tiles
        private bool IsTileCached(List<Vector3> cache, Vector3 pos)
        {
            pos = new Vector3(Mathf.Round(pos.x), 0f, Mathf.Round(pos.z));

            foreach (Vector3 point in cache)
            {
                var rounded = new Vector3(Mathf.Round(point.x), 0f, Mathf.Round(point.z));
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
    }
}