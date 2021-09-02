using Runner.Util;
using Runner.World.LevelTemplates;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.World
{
    public class LevelPart : LevelItem
    {
        [SerializeField] public LevelTemplate template;
        [SerializeField] protected Transform _tilesContainer;

        private Dictionary<Vector3, Transform> _tiles;

        void Awake()
        {
            _tiles = new Dictionary<Vector3, Transform>();

            if (template.tilePrefab != null)
            {
                GenerateTiles();
            }

            if (template.decorPrefab != null)
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

        public void GenerateTiles()
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
                GameObject obj = Instantiate(template.tilePrefab, _tilesContainer);
                obj.transform.position = pos;
                _tiles[pos] = obj.transform;
            }
        }

        private void DecorateBlocks()
        {
            float spacing = template.spacing * LevelGenerator.TileSize * 0.5f;
            foreach (KeyValuePair<Vector3, Transform> pair in _tiles)
            {
                if (_tiles.ContainsKey(pair.Key + Vector3.up * LevelGenerator.TileSize)) continue; // If there's a tile above

                if (RandomUtil.Bool(template.decorChance))
                {
                    BlockDecor.Decorate(pair.Value, template.decorPrefab, spacing);
                }
            }
        }
    }
}