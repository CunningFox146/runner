using DG.Tweening;
using UnityEngine;

namespace Runner.Interactable
{
    public class Coin : MonoBehaviour, IInteractable
    {
        [SerializeField] private Transform _model;

        private Tween _tween;

        void Start()
        {
            _tween = transform.DORotate(new Vector3(0f, -360f, 0f), 3f, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(-1);
        }

        public void OnInteractStart(GameObject player)
        {
            _tween.Kill();
            Destroy(gameObject);
        }

        public void OnInteractStop(GameObject player) { }
    }
}