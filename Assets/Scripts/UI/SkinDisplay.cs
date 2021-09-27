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
        [SerializeField] private Transform _model;

        private Sequence _changeSequence;
        private Vector3 _startScale;
        private Vector3 _startRotation;

        void Awake()
        {
            _startScale = _model.localScale;
            _startRotation = _model.rotation.eulerAngles;

            float duration = 0.5f;

            _changeSequence = DOTween.Sequence()
            .Append(_model.DOScale(_startScale, duration).SetEase(Ease.OutCubic))
            .Join(_model.DORotate(_startRotation, duration).SetEase(Ease.OutCubic))
            .Pause();
        }

        private void Start()
        {
            ChangeSkin();
        }

        public void ChangeSkin()
        {
            _model.localScale = Vector3.zero;
            _model.rotation = new Quaternion();

            _changeSequence.Restart();
            _changeSequence.Play();
        }
    }
}