using DG.Tweening;
using UnityEngine;

namespace Runner.Environment.Decor
{
    public class Grass : MonoBehaviour
    {
        private Tween _tween;
        private float _startScale;

        void Start()
        {
            transform.rotation = Quaternion.AngleAxis(Random.Range(-360f, 360f), Vector3.up);
            _startScale = transform.localScale.y;
        }

        void OnTriggerEnter(Collider collider)
        {
            CancelTween();

            _tween = transform.DOScaleY(_startScale * 0.2f, 0.25f).SetEase(Ease.OutCubic);
        }

        void OnTriggerExit(Collider collider)
        {
            CancelTween();

            _tween = transform.DOScaleY(_startScale, 0.25f).SetEase(Ease.InBounce);
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