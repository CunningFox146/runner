using Runner.Environment;
using Runner.Managers.ObjectPool;
using Runner.Managers.World;
using Runner.World.LevelTemplates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.World
{
    public class LevelTransfer : LevelItem, IPoolReaction
    {
        private bool[][] _pattern = new bool[][]
        {
            new bool[] { true, false, true },
            new bool[] { true, true, false },
            new bool[] { false, false, true }
        };

        public void ObjectPooled(bool isInPool)
        {
            if (!isInPool) return;

            List<Transform> toRemove = new List<Transform>();
            foreach(Transform child in transform)
            {
                if (child.CompareTag("Block"))
                {
                    toRemove.Add(child);
                }
            }
            toRemove.ForEach((Transform item) => Destroy(item.gameObject));
        }

        public void GenerateTiles(LevelTemplate oldLevel, LevelTemplate newLevel)
        {
            float tileSize = LevelGenerator.TileSize;
            for (int x = 0; x < _pattern.Length; x++)
            {
                for (int z = 0; z < _pattern[x].Length; z++)
                {
                    var pos = new Vector3(-tileSize + tileSize * (float)x, 0f, pointStart.position.z + tileSize * 0.5f + tileSize * (float)z);
                    var prefab = (_pattern[x][z] ? oldLevel : newLevel).tilePrefab;
                    Instantiate(prefab, transform).transform.position = pos;
                }
            }
        }
    }
}