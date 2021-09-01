using Runner.Environment;
using Runner.Managers.ObjectPool;
using Runner.Util;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.Managers.World
{
    public class LevelGenerator : MonoBehaviour
    {
        public static readonly float TileSize = 2f;

        [SerializeField] private GameObject[] _partsPrefabs;
        [SerializeField] private LevelPart _lastPiece;
        [SerializeField] private int _startPieceCount;
        [SerializeField] private int _pieceLimit;

        private Queue<LevelPart> _piecesToRemove;
        private List<LevelPart> _parts;
        private GameObject _lastPrefab;

        void Awake()
        {
            _parts = new List<LevelPart>();
            _piecesToRemove = new Queue<LevelPart>();

            if (_lastPiece != null)
            {
                _parts.Add(_lastPiece);
            }
        }

        void Start()
        {
            for (int i = 0; i < _startPieceCount; i++)
            {
                GeneratePart();
            }
        }

        void Update()
        {
            foreach (LevelPart piece in _parts)
            {
                piece.transform.Translate(-Vector3.forward * Time.deltaTime * GameManager.GameSpeed);

                if (piece.pointEnd.position.z < 0f && !_piecesToRemove.Contains(piece))
                {
                    _piecesToRemove.Enqueue(piece);
                }
            }

            Debug.Log(_piecesToRemove.Count);
            if (_piecesToRemove.Count > _pieceLimit)
            {
                RemoveLastPart();
                GeneratePart();
            }
        }

        private void GeneratePart()
        {
            GameObject prefab = null;
            do
            {
                prefab = ArrayUtil.GetRandomItem(_partsPrefabs);
            } while (prefab == _lastPrefab);

            var obj = ObjectPooler.Inst.GetObject(prefab);
            var part = obj.GetComponent<LevelPart>();

            obj.transform.position = new Vector3(0f, 0f, _lastPiece.pointEnd.position.z - part.pointStart.position.z);
            obj.transform.parent = transform;

            _parts.Add(part);
            _lastPiece = part;
            _lastPrefab = prefab;
        }

        private void RemoveLastPart()
        {
            var piece = _piecesToRemove.Dequeue();
            _parts.Remove(piece);
            ObjectPooler.Inst.ReturnObject(piece.gameObject);
        }
    }
}