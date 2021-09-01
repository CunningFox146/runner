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
    public class LevelPart : MonoBehaviour, IPoolReaction
    {
        public static Action<LevelPart> OnLevelPartExit;

        [SerializeField] public Transform pointStart;
        [SerializeField] public Transform pointEnd;
        [SerializeField] private Transform _tilesContainer;
        
        [SerializeField] private GameObject _fillTile;
        
        private bool _isExitPushed;
        
        void Awake()
        {
            if (_fillTile != null)
            {
                FillTiles();
            }
        }

        void Update()
        {
            if (pointEnd.position.z <= 0f && !_isExitPushed)
            {
                OnLevelPartExit?.Invoke(this);
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
        
        private void FillTiles()
        {
            foreach (Vector3 pos in GetMissingTiles())
            {
                Instantiate(_fillTile, _tilesContainer).transform.position = pos;
            }
        }
    }
}