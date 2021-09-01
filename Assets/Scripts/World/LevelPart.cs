using Runner.Managers.World;
using Runner.Util;
using Runner.World;
using Runner.World.LevelTemplates;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.Environment
{
    public class LevelPart : MonoBehaviour
    {
        [SerializeField] public Transform pointStart;
        [SerializeField] public Transform pointEnd;
        [SerializeField] private Transform _tilesContainer;
        [SerializeField] private LevelTemplate _template;

        private Dictionary<Vector3, Transform> _tiles;

        void Awake()
        {
            _tiles = new Dictionary<Vector3, Transform>();

            if (_template.tilePrefab != null)
            {
                GenerateTiles();
            }

            if (_template.decorPrefab != null)
            {
                DecorateBlocks();
            }
        }

        public List<Vector3> GetTilesPos()
        {
            float tileSize = LevelGenerator.TileSize;
            List<Vector3> list = new List<Vector3>();

            for (float x = -tileSize; x <= tileSize; x += tileSize)
            {
                for (float z = pointStart.position.z + tileSize * 0.5f; z < pointEnd.position.z; z += tileSize)
                {
                    list.Add(new Vector3(x, 0f, z));
                }
            }

            return list;
        }

        private void GenerateTiles()
        {
            // Iterate through children before we generate tiles bc we'll already know generated positions
            foreach (Transform obj in _tilesContainer)
            {
                if (obj.CompareTag("Block"))
                {
                    _tiles[obj.position] = obj;
                }
            }

            foreach (Vector3 pos in GetTilesPos())
            {
                GameObject obj = Instantiate(_template.tilePrefab, _tilesContainer);
                obj.transform.position = pos;
                _tiles[pos] = obj.transform;
            }
        }

        private void DecorateBlocks()
        {
            float spacing = _template.spacing * LevelGenerator.TileSize * 0.5f;
            foreach (KeyValuePair<Vector3, Transform> pair in _tiles)
            {
                if (_tiles.ContainsKey(pair.Key + Vector3.up * LevelGenerator.TileSize)) continue; // If there's a tile above

                if (RandomUtil.Bool(_template.decorChance))
                {
                    BlockDecor.Decorate(pair.Value, _template.decorPrefab, spacing);
                }
            }
        }
    }
}