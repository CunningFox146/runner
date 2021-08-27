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
        [SerializeField] private int _warmCount;
        [SerializeField] private int _maxCacheSize;
        [SerializeField] private int _startPos;

        private SetPiece _lastPiece;

        private bool _isWarming;
        private Queue<SetPiece> _cache;

        void Awake()
        {
            _cache = new Queue<SetPiece>();

            SetPiece.OnSetPieceExit += OnSetPieceExitHandler;
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
            foreach (SetPiece piece in _cache)
            {
                piece.transform.Translate(-Vector3.forward * Time.deltaTime * 5f);
            }
        }

        private void OnSetPieceExitHandler(SetPiece setPiece)
        {
            if (_cache.Count > _maxCacheSize)
            {
                RemoveLastSet();
            }
            ReleaseSet();
        }

        private void ReleaseSet()
        {
            var prefab = ArrayUtil.GetRandomItem(_setPieces);
            var obj = ObjectPooler.Inst.GetObject(prefab);
            var setPiece = obj.GetComponent<SetPiece>();

            float offset = _lastPiece ? _lastPiece.pointEnd.position.z : _startPos;

            obj.transform.position = new Vector3(0f, 0f, offset - setPiece.pointStart.position.z);
            obj.transform.parent = transform;

            _cache.Enqueue(setPiece);
            _lastPiece = setPiece;
        }

        private void RemoveLastSet()
        {
            var piece = _cache.Dequeue();
            ObjectPooler.Inst.ReturnObject(piece.gameObject);
        }
    }
}