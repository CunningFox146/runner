using DG.Tweening;
using Runner.Player;
using Runner.World;
using Runner.World.LevelTemplates;
using UnityEngine;

namespace Runner.Managers
{
    public class CameraManager : Singleton<CameraManager>
    {
        [SerializeField] private PlayerController _player;
        [SerializeField] private Camera _camera;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private float _moveSpeed;

        private Tween _uiModeTween;

        private bool _isFollowing;
        public bool IsFollowing
        {
            get => _isFollowing;
            set
            {
                _isFollowing = value;
                if (_isFollowing)
                {
                    if (_uiModeTween != null)
                    {
                        _uiModeTween.Kill();
                        _uiModeTween = null;
                    }
                    transform.DORotate(new Vector3(30f, 0f, 0f), 0.5f).SetEase(Ease.InOutCubic);
                }
                else
                {
                    _uiModeTween = transform.DORotate(transform.rotation.eulerAngles + new Vector3(0f, 20f, 0f), 5f);
                    _uiModeTween.SetEase(Ease.InOutCubic);
                    _uiModeTween.SetLoops(-1, LoopType.Yoyo);
                }
            }
        }

        protected override void Awake()
        {
            base.Awake();

            _camera = GetComponent<Camera>();


            IsFollowing = false;

            LevelGenerator.OnTemplateChanged += OnTemplateChangedHandler;
        }

        void OnDestroy()
        {
            if (_uiModeTween != null)
            {
                _uiModeTween.Kill();
            }
        }

        private void OnTemplateChangedHandler(LevelTemplate oldTemplate, LevelTemplate newTemplate)
        {
            _camera.DOColor(newTemplate.skyColor, 1f).SetEase(Ease.InCubic);
        }


        // Update is called once per frame
        void LateUpdate()
        {
            if (IsFollowing)
            {
                Follow();
            }
        }

        private void Follow()
        {
            transform.position = Vector3.Lerp(transform.position, _offset + _player.transform.position,
                    Time.deltaTime * _moveSpeed);
        }
    }
}