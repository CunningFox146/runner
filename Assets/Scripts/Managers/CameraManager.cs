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

        public bool isFollowing = true;

        protected override void Awake()
        {
            base.Awake();

            _camera = GetComponent<Camera>();

            LevelGenerator.OnTemplateChanged += OnTemplateChangedHandler;
        }

        private void OnTemplateChangedHandler(LevelTemplate oldTemplate, LevelTemplate newTemplate)
        {
            _camera.DOColor(newTemplate.skyColor, 1f).SetEase(Ease.InCubic);
        }

        void Start()
        {
            transform.position = _offset + _player.transform.position;
        }

        // Update is called once per frame
        void LateUpdate()
        {
            if (isFollowing)
            {
                transform.position = Vector3.Lerp(transform.position, _offset + _player.transform.position,
                    Time.deltaTime * _moveSpeed);
            }
        }
    }
}