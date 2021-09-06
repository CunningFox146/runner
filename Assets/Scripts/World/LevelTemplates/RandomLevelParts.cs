using Runner.Util;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.World.LevelTemplates
{
    [CreateAssetMenu(fileName = "Parts", menuName = "Scriptable Objects/LevelParts", order = 2)]
    public class RandomLevelParts : LevelPartsOrder
    {
        public GameObject[] parts;

        private List<GameObject> _queue = new List<GameObject>();

        private void FillQueue()
        {
            for (int i = 0; i < 2 - _queue.Count; i++)
            {
                GameObject lastItem = _queue.Count > 0 ? _queue[_queue.Count - 1] : null;
                GameObject item;
                do
                {
                    item = ArrayUtil.GetRandomItem(parts);
                } while (item == lastItem);

                _queue.Add(item);
            }
        }

        public override GameObject GetAndRemoveNextLevel()
        {
            if (_queue.Count == 0)
            {
                FillQueue();
            }

            GameObject obj = _queue[0];
            _queue.RemoveAt(0);
            FillQueue();
            return obj;
        }

        public override GameObject GetLevel(int idx) => _queue[idx];

        public override GameObject GetNextLevel() => _queue[0];

        public override void RemoveLevel(int idx)
        {
            _queue.RemoveAt(idx);
            FillQueue();
        }
    }
}