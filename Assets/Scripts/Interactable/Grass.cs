using DG.Tweening;
using UnityEngine;

namespace Runner.Interactable
{
    public class Grass : MonoBehaviour, IInteractable
    {
        private Tween _tween;
        private float _startScale;

        void Start()
        {
            transform.rotation = Quaternion.AngleAxis(Random.Range(-360f, 360f), Vector3.up);
            _startScale = transform.localScale.y;
        }
        
        public void OnInteractStart(GameObject player)
        {
            CancelTween();

            _tween = transform.DOScaleY(_startScale * 0.2f, 0.25f).SetEase(Ease.OutCubic);
        }

        public void OnInteractStop(GameObject player)
        {
            CancelTween();

            _tween = transform.DOScaleY(_startScale, 0.25f).SetEase(Ease.OutBounce);
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