using Runner.Util;
using UnityEngine;

namespace Runner.Environment
{
    public class MeshRandomizer : MonoBehaviour
    {
        [SerializeField] private Mesh[] _meshes;
        private MeshFilter _filter;

        void Awake()
        {
            _filter = GetComponent<MeshFilter>();
        }

        void Start()
        {
            RandomizeMesh();
        }

        public void RandomizeMesh()
        {
            _filter.mesh = ArrayUtil.GetRandomItem(_meshes);
        }
    }
}