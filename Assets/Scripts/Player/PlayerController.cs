using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Runner.Player
{
    public class PlayerController : MonoBehaviour
    {
        public enum PlayerLane
        {
            Left = -1,
            Center,
            Right
        }
        
        [SerializeField] private PlayerCollider _collider;
        [SerializeField] private float _rayLength = 10f;
        [SerializeField] private Text _debugText;
        [SerializeField] private float _TEMP_gameSpeed = 1f;
        [Header("Moving")]
        [SerializeField] private AnimationCurve _curve;
        [SerializeField] private float _laneOffset = 4f;
        [SerializeField] private float _laneChangeSpeed = 4f;
        [SerializeField] private float _gravitation = 10f;
        [Header("Jumping")]
        [SerializeField] private float _jumpLength = 4f;
        [SerializeField] private float _jumpHeight = 4f;
        [Header("Sliding")]
        [SerializeField] private float _slideLength = 4f;

        public Vector3 rayStart;
        public float groundOffset = 1f;

        private float _groundDist = 0f;
        private PlayerLane _lane = PlayerLane.Center;
        private Tween _laneTween;
        private Tween _slideTween;
        private Sequence _jumpSequence;
        private Sequence _slideSequence;
        
        private float SpeedMod => 1 / _TEMP_gameSpeed;
        private bool IsJumping => _jumpSequence != null && !_jumpSequence.IsComplete();
        private bool IsSliding => _slideSequence != null && !_slideSequence.IsComplete();
        
        public float GroundYOffset => _groundDist + groundOffset;

        void Update()
        {
            //QualitySettings.vSyncCount = 0;
            //Application.targetFrameRate = 10;

            if (Input.GetKeyDown(KeyCode.A))
            {
                ChangeSide(-1);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                ChangeSide(1);
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                Jump();
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                Slide();
            }
        }

        void FixedUpdate()
        {
            var pos = transform.TransformPoint(rayStart);
            float lastY = GroundYOffset;
            Debug.DrawLine(pos, -transform.up * _rayLength, Color.green);

            if (Physics.Raycast(pos, -transform.up, out RaycastHit ray, _rayLength, 1 << 6))
            {
                _groundDist = pos.y - ray.point.y;
                Debug.DrawLine(pos, ray.point, Color.red);
            }

            Debug.Log(GroundYOffset);

            _debugText.text = GroundYOffset.ToString();
            float r = Mathf.Abs(GroundYOffset - lastY);
            _debugText.color = new Color(r, 1f- lastY, 1f- lastY);
            UpdateGroundOffset();
        }

        public void OnHitObstacle(Collision collision)
        {
            Debug.Log("DEATH");
            enabled = false;
        }

        public void OnHitCollectable(Collision collision)
        {
            Debug.Log("COLLECTABLE");
        }

        private void UpdateGroundOffset()
        {
            if (IsJumping)
            {
                Debug.Log($"{transform.position.y} {GroundYOffset}");
                if (GroundYOffset < 0f && _jumpSequence.ElapsedPercentage() > 0.1f)
                {
                    _jumpSequence.Kill(complete: false);
                    _jumpSequence = null;
                }
                else
                {
                    return;
                }
            }
            Debug.DrawLine(transform.position, transform.position + new Vector3(0f, -GroundYOffset * _gravitation * Time.deltaTime, 0f), Color.blue);
            transform.Translate(0f, -GroundYOffset * _gravitation * Time.deltaTime, 0f);
        }
        
        private void ChangeSide(int sideDelta)
        {
            int lane = (int)_lane + sideDelta;
            if (!Enum.IsDefined(typeof(PlayerLane), lane)) return;

            DOTween.Kill(_laneTween, complete: false);

            _laneTween = transform.DOMoveX(lane * _laneOffset, _laneChangeSpeed).SetEase(_curve);
            _lane = (PlayerLane)lane;
        }
        
        private void Jump()
        {
            if (IsJumping || _slideTween != null) return;

            _slideTween.Kill(complete: false);
            _jumpSequence.Kill(complete: false);

            var speed = SpeedMod * 0.5f;

            var sequence = DOTween.Sequence();
            sequence.Append(transform.DOMoveY(transform.position.y + _jumpHeight, _jumpLength * speed).SetEase(Ease.OutSine));
            sequence.Append(transform.DOMoveY(transform.position.y + GroundYOffset, _jumpLength * speed).SetEase(Ease.InSine));
            sequence.OnComplete(() => _jumpSequence = null);

            _jumpSequence = sequence;
        }

        private void Slide()
        {
            if (IsJumping && _slideTween == null)
            {
                _jumpSequence.Kill(complete:false);
                _jumpSequence = null;

                _slideTween = transform.DOMoveY(0, 0.25f) // TODO Better calculate duration
                    .SetEase(Ease.Linear)
                    .OnComplete(() => _slideTween = null);
            }

            if (IsSliding) return;

            _collider.StartSliding();

            _slideSequence = DOTween.Sequence();
            _slideSequence
                .SetDelay(SpeedMod * _slideLength)
                .OnComplete(() =>
                {
                    _collider.StopSliding();
                    _slideSequence = null;
                });
        }

    }
}