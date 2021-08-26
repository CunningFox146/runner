using System.Collections;
using System.Collections.Generic;
using Runner.Environment;
using Runner.Util;
using UnityEngine;

namespace Runner.Managers.World
{
    public class LevelGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject[] _setPieces;
        [SerializeField] private int _warmCount;
        [SerializeField] private int _cacheSize;
        [SerializeField] private SetPiece _lastPiece;

        private bool _isWarming;
        private Queue<SetPiece> _cache;

        void Awake()
        {
            _cache = new Queue<SetPiece>();
            _cache.Enqueue(_lastPiece);

            _isWarming = true;
            for (int i = 0; i < _warmCount; i++)
            {
                ReleaseSet();
            }
            _isWarming = false;

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
            ReleaseSet();
        }

        private void ReleaseSet()
        {
            var piece = Instantiate(ArrayUtil.GetRandomItem<GameObject>(_setPieces), transform);
            var setPiece = piece.GetComponent<SetPiece>();
            var pieceLength = setPiece.Length;

            piece.transform.position = new Vector3(0f, 0f, _lastPiece.transform.position.z + (_lastPiece.Length + pieceLength) * 0.5f);

            _cache.Enqueue(setPiece);
            _lastPiece = setPiece;

            if (!_isWarming && _cache.Count > _cacheSize)
            {
                RemoveLastSet();
            }
        }

        private void RemoveLastSet()
        {
            var piece = _cache.Dequeue();
            Destroy(piece.gameObject);
        }
    }
}