using Runner.Util;
using UnityEngine;

namespace Runner.Environment
{
    public class BlockDecor : MonoBehaviour
    {

        [SerializeField] private float _chance = 0.5f;
        [SerializeField] private GameObject _decorPrefab;
        [SerializeField] private float _spacing;

        void Start()
        {
            if (_decorPrefab != null)
            {
                Decorate();
            }
        }

        private void Decorate()
        {
            if (!RandomUtil.RandomBool(_chance)) return;

            var obj = Instantiate(_decorPrefab, transform);
            obj.transform.localPosition = new Vector3(Random.Range(-_spacing, _spacing), 0f, Random.Range(-_spacing, _spacing));
        }
    }
}