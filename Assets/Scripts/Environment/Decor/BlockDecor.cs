using Runner.Util;
using UnityEngine;

namespace Runner.Environment.Decor
{
    public class BlockDecor : MonoBehaviour
    {

        [SerializeField] private float _grassChance = 0.5f;
        [SerializeField] private GameObject _grass;
        [SerializeField] private float _spacing;

        void Start()
        {
            if (_grass != null)
            {
                Decorate();
            }
        }

        private void Decorate()
        {
            if (!RandomUtil.RandomBool(_grassChance)) return;

            var grass = Instantiate(_grass, transform);
            grass.transform.localPosition = new Vector3(Random.Range(-_spacing, _spacing), 0f, Random.Range(-_spacing, _spacing));
        }
    }
}