using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Runner.UI
{
    public class SkinDisplay : MonoBehaviour
    {
        [SerializeField] private Vector3 _startPos;
        [SerializeField] private Vector3 _startScale;
        [SerializeField] private Vector3 _startRotation;

        private Transform _model;

        public void ChangeSkin(GameObject newModel)
        {
            if (_model != null)
            {
                Destroy(_model.gameObject);
            }
            _model = Instantiate(newModel, transform).transform;
            _model.gameObject.layer = (int)Layers.UI;

            Destroy(_model.GetComponent<Animator>());

            _model.localPosition = _startPos;
            _model.localScale = Vector3.zero;
            _model.rotation = new Quaternion();

            float duration = 0.5f;
            DOTween.Sequence()
            .Append(_model.DOScale(_startScale, duration).SetEase(Ease.OutCubic))
            .Join(_model.DORotate(_startRotation, duration).SetEase(Ease.OutCubic));
        }
    }
}