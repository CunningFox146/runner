using Runner.Managers;
using Runner.ObjectPool;
using Runner.Util;
using Runner.World.LevelTemplates;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.World
{
    public class LevelGenerator : MonoBehaviour
    {
        public static readonly float TileSize = 2f;

        public static event Action<LevelTemplate, LevelTemplate> OnTemplateChanged;

        [SerializeField] private GameObject[] _partsPrefabs;

        [SerializeField] private GameObject[] _transitionPrefabs;
        [SerializeField] private GameObject _transitionBridge;
        [SerializeField] private float _bridgeChance = 0.25f;

        [SerializeField] private LevelPart _lastPiece;
        [SerializeField] private int _startPieceCount;
        [SerializeField] private int _pieceLimit;

        public LevelTemplate currentTemplate;
        private Queue<LevelItem> _piecesToRemove;
        private List<LevelItem> _parts;
        private GameObject _lastPrefab;

        void Awake()
        {
            _parts = new List<LevelItem>();
            _piecesToRemove = new Queue<LevelItem>();

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
            foreach (LevelItem piece in _parts)
            {
                piece.transform.Translate(-Vector3.forward * Time.deltaTime * GameManager.GameSpeed);

                if (piece.pointStart.position.z <= 0f && piece.pointEnd.position.z > 0f)
                {
                    UpdateCurrentBiome(piece);
                }

                if (piece.pointEnd.position.z < 0f && !_piecesToRemove.Contains(piece))
                {
                    _piecesToRemove.Enqueue(piece);
                }
            }

            if (_piecesToRemove.Count > _pieceLimit)
            {
                LevelItem part = _piecesToRemove.Dequeue();
                RemovePart(part);
                if ((part as LevelPart) != null)
                {
                    GeneratePart();
                }
            }
        }

        private void UpdateCurrentBiome(LevelItem piece)
        {
            if (piece.TryGetComponent(out LevelPart levelPart) && levelPart.template != currentTemplate)
            {
                OnTemplateChanged?.Invoke(currentTemplate, levelPart.template);
                currentTemplate = levelPart.template;
            }
        }

        private void GeneratePart()
        {
            GameObject prefab;
            do
            {
                prefab = ArrayUtil.GetRandomItem(_partsPrefabs);
            } while (prefab == _lastPrefab);

            GameObject obj = ObjectPooler.Inst.GetObject(prefab);
            LevelPart part = obj.GetComponent<LevelPart>();

            float offset;

            if (part.template != _lastPiece.template)
            {
                LevelTransfer transition = GenerateTransition(_lastPiece.template, part.template);
                transition.transform.position = new Vector3(0f, 0f, _lastPiece.pointEnd.position.z - transition.pointStart.position.z);

                offset = transition.pointEnd.position.z - part.pointStart.position.z;

                _parts.Add(transition);
            }
            else
            {
                offset = _lastPiece.pointEnd.position.z - part.pointStart.position.z;
            }

            obj.transform.position = new Vector3(0f, 0f, offset);
            obj.transform.parent = transform;

            _parts.Add(part);
            _lastPiece = part;
            _lastPrefab = prefab;
        }

        private LevelTransfer GenerateTransition(LevelTemplate oldTemplate, LevelTemplate newTemplate)
        {
            var prefab = RandomUtil.Bool(_bridgeChance) ? _transitionBridge : ArrayUtil.GetRandomItem(_transitionPrefabs);

            GameObject transition = ObjectPooler.Inst.GetObject(prefab);
            LevelTransfer transfer = transition.GetComponent<LevelTransfer>();
            transfer.GenerateTiles(oldTemplate, newTemplate);

            return transfer;
        }

        private void RemovePart(LevelItem item)
        {
            _parts.Remove(item);
            ObjectPooler.Inst.ReturnObject(item.gameObject);
        }
    }
}