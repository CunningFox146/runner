using System.Collections;
using System.Collections.Generic;
using Runner.Environment;
using Runner.Managers.ObjectPool;
using Runner.Util;
using UnityEngine;

namespace Runner.Managers.World
{
    public class LevelGenerator : MonoBehaviour
    {
        public static readonly float TileSize = 2f;

        [SerializeField] private GameObject[] _setPieces;
        [SerializeField] private LevelPart _lastPiece;
        [SerializeField] private int _warmCount;
        [SerializeField] private int _maxCacheSize;
        [SerializeField] private int _startPos;
        
        private GameObject _lastPrefab;

        private bool _isWarming;
        private Queue<LevelPart> _cache;

        void Awake()
        {
            _cache = new Queue<LevelPart>();

            if (_lastPiece != null)
            {
                _cache.Enqueue(_lastPiece);
            }

            LevelPart.OnLevelPartExit += OnSetPieceExitHandler;
        }

        void Start()
        {
            for (int i = 0; i < _warmCount; i++)
            {
                ReleaseSet();
            }
        }

        void Update()
        {
            foreach (LevelPart piece in _cache)
            {
                piece.transform.Translate(-Vector3.forward * Time.deltaTime * GameManager.GameSpeed);
            }
        }

        private void OnSetPieceExitHandler(LevelPart levelPart)
        {
            if (_cache.Count > _maxCacheSize)
            {
                RemoveLastSet();
            }
            ReleaseSet();
        }

        private void ReleaseSet()
        {
            GameObject prefab = null;
            do
            {
                prefab = ArrayUtil.GetRandomItem(_setPieces);
            } while (prefab == _lastPrefab);

            var obj = ObjectPooler.Inst.GetObject(prefab);
            var setPiece = obj.GetComponent<LevelPart>();

            float offset = _lastPiece ? _lastPiece.pointEnd.position.z : _startPos;

            obj.transform.position = new Vector3(0f, 0f, offset - setPiece.pointStart.position.z);
            obj.transform.parent = transform;

            _cache.Enqueue(setPiece);
            _lastPiece = setPiece;
            _lastPrefab = prefab;
        }

        private void RemoveLastSet()
        {
            var piece = _cache.Dequeue();
            ObjectPooler.Inst.ReturnObject(piece.gameObject);
        }
    }
}