using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.World
{
    public class CoinGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject _coin;
        [SerializeField] private AnimationCurve _curve;
        [SerializeField] private float _spacing = 0.1f;

        private void Start()
        {
            foreach (Vector3 pos in GetSpawnPositions())
            {
                Instantiate(_coin, transform).transform.localPosition = pos;
            }
        }

        public List<Vector3> GetSpawnPositions()
        {
            var list = new List<Vector3>();
            if (_spacing <= 0f) return list; // This is bc we use it in edditor

            for (float z = 0; z <= _curve.keys[_curve.length - 1].time; z += _spacing)
            {
                list.Add(new Vector3(0f, _curve.Evaluate(z), z));
            }
            return list;
        }
    }
}