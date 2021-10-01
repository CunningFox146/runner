using DG.Tweening;
using Runner.ExtentionClasses;
using Runner.Managers;
using UnityEngine;

namespace Runner.UI
{
    public class SkinDisplay : MonoBehaviour
    {
        [SerializeField] private Vector3 _startPos;
        [SerializeField] private Vector3 _startScale;
        [SerializeField] private Vector3 _startRotation;

        private Transform _model;
        private Sequence _changeSequence;

        public void ChangeSkin(GameObject newModel)
        {
            CameraManager.Inst.PlaySound("SkinSelected");

            if (_model != null)
            {
                Destroy(_model.gameObject);
            }
            _model = Instantiate(newModel, transform).transform;

            _model.SetLayerRecursively((int)Layers.UI);

            Destroy(_model.GetComponent<Animator>());

            _model.localPosition = _startPos;
            _model.localScale = Vector3.zero;
            _model.rotation = new Quaternion();

            if (_changeSequence != null)
            {
                _changeSequence.Kill();
                _changeSequence = null;
            }

            float duration = 0.5f;

            _changeSequence = DOTween.Sequence()
            .Append(_model.DOScale(_startScale, duration).SetEase(Ease.OutCubic))
            .Join(_model.DORotate(_startRotation, duration).SetEase(Ease.OutCubic));
        }
    }
}