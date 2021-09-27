using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Runner.UI
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] private Slider _scrollbar;
        [SerializeField] private Text _valueText;
        [SerializeField] private ParticleSystem _particles;
        [SerializeField] private float _tweenDuration = 1f; // from 0 to 1, scales
        [Range(0f, 1f)]
        [SerializeField] private float _startProgress = 0f;
        [SerializeField] private float _maxValue = 0f;

        private Tween _tween;
        private float _progress = 0f;

        public float Progress
        {
            get => _progress;
            set
            {
                Debug.Log(value);
                _progress = value;
                _scrollbar.value = _progress;
                _valueText.text = ((int)(_progress * _maxValue)).ToString();
            }
        }

        private void Start()
        {
            Progress = _startProgress;
            Animate(1f);
        }

        public void Animate(float delta)
        {
            float oldVal = Progress;
            float newVal = Mathf.Clamp01(Progress + delta);
            float duration = Mathf.Abs(Progress - oldVal) * _tweenDuration;

            if (_tween != null)
            {
                _tween.Kill();
                _tween = null;
            }

            _particles.Play();
            _tween = DOTween.To(
                () => Progress,
                (val) => Progress = val,
                newVal,
                1)
                .OnComplete(() => _particles.Stop())
                .SetEase(Ease.InCubic);
        }
    }
}