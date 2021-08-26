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
            
            for (int i = 0; i < _warmCount; i++)
            {
                ReleaseSet();
            }

            SetPiece.OnSetPieceExit += OnSetPieceExitHandler;
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
            var piece = ObjectPooler.Inst.GetObject(ArrayUtil.GetRandomItem(_setPieces));
            var setPiece = piece.GetComponent<SetPiece>();
            var pieceLength = setPiece.Length;
            float lastPos = _lastPiece ? _lastPiece.transform.position.z : _startPos;
            float lastLength = _lastPiece ? _lastPiece.Length : 0f;

            piece.transform.position = new Vector3(0f, 0f, lastPos + (lastLength + pieceLength) * 0.5f);
            piece.transform.parent = transform;

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