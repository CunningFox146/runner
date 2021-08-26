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

        [HideInInspector] public Vector3 startPoint;
        [HideInInspector] public float length;

        void Awake()
        {
            UpdateSetLength();
            //WarmTiles();
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

        public List<Vector3> CheckTiles()
        {
            var cache = GetCachedTiles();

            float tileSize = 2f;
            var list = new List<Vector3>();

            for (float x = startPoint.x; x <= _pointEnd.transform.position.x; x += tileSize)
            {
                for (float z = -1f; z <= 1f; z++)
                {
                    var pos = new Vector3(x, 0f, z * tileSize);
                    if (!cache.Contains(pos))
                    {
                        list.Add(pos);
                    }
                }
            }

            return list;
        }
    }
}