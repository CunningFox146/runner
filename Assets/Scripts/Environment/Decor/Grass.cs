using DG.Tweening;
using UnityEngine;

namespace Runner.Environment.Decor
{
    public class Grass : MonoBehaviour
    {
        [SerializeField] private Transform _model;
        private Tween _tween;

        void OnTriggerEnter(Collider collider)
        {
            CancelTween();

            _tween = _model.DOScaleY(0.2f, 0.25f).SetEase(Ease.OutCubic);
        }

        void OnTriggerExit(Collider collider)
        {
            CancelTween();

            _tween = _model.DOScaleY(1f, 0.25f).SetEase(Ease.InBounce);
        }

        private void CancelTween()
        {
            if (_tween != null)
            {
                _tween.Kill();
                _tween = null;
            }
        }
    }
}