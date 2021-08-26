using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.Environment
{
    public class SetPiece : MonoBehaviour
    {
        [SerializeField] private GameObject _pointStart;
        [SerializeField] private GameObject _pointEnd;
        [SerializeField] private Transform _tilesContainer;

        [SerializeField] bool _isWarmingTiles;
        [SerializeField] GameObject _warmingPrefab;

        [HideInInspector] public Vector3 startPoint;
        [HideInInspector] public float length;

        void Awake()
        {
            UpdateSetLength();

            if (_isWarmingTiles)
            {
                WarmTiles();
            }
        }

        public void UpdateSetLength()
        {
            if (_pointStart != null && _pointEnd != null)
            {
                startPoint = _pointStart.transform.position;
                length = Vector3.Distance(startPoint, _pointEnd.transform.position);
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

        public List<Vector3> GetMissingTiles()
        {
            float tileSize = 2f;
            var list = new List<Vector3>();

            for (float x = startPoint.x; x <= _pointEnd.transform.position.x; x += tileSize)
            {
                for (float z = -1f; z <= 1f; z++)
                {
                    var pos = new Vector3(x, 0f, z * tileSize);
                    // Can't use !cache.Contains(pos) here, so we use plain raycast
                    if (!Physics.Raycast(pos + new Vector3(0f, 0.1f, 0f), Vector3.down, 0.2f))
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