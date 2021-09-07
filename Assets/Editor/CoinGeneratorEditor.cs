using Runner.World;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
    [CustomEditor(typeof(CoinGenerator))]
    public class CoinGeneratorEditor : UnityEditor.Editor
    {
        private CoinGenerator _target;

        public void OnEnable()
        {
            _target = (CoinGenerator)target;
        }

        public void OnSceneGUI()
        {
            float size = 0.5f;
            Handles.color = Color.red;
            foreach (Vector3 pos in _target.GetSpawnPositions())
            {
                Handles.DrawWireDisc(_target.transform.position + pos + Vector3.up * size, Vector3.forward, size, 5f);
            }
        }
    }
}