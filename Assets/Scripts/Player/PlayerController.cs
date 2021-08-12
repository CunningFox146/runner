using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using UnityEngine;

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
        [SerializeField] private float _TEMP_gameSpeed = 1f;
        [Header("Moving")]
        [SerializeField] private float _laneOffset = 4f;
        [SerializeField] private float _laneChangeSpeed = 4f;
        [Header("Jumping")]
        [SerializeField] private float _jumpLength = 4f;
        [SerializeField] private float _jumpHeight = 4f;
        [Header("Sliding")]
        [SerializeField] private float _slideLength = 4f;

        private PlayerLane _lane = PlayerLane.Center;
        private Tween _laneTween;
        private Tween _slideTween;
        private Sequence _jumpSequence;
        private Sequence _slideSequence;
        
        private float SpeedMod => 1 / _TEMP_gameSpeed;
        private bool IsJumping => _jumpSequence != null && !_jumpSequence.IsComplete();
        private bool IsSliding => _slideSequence != null && !_slideSequence.IsComplete();

        void Update()
        {
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
        
        private void ChangeSide(int sideDelta)
        {
            int lane = (int)_lane + sideDelta;
            if (!Enum.IsDefined(typeof(PlayerLane), lane)) return;

            DOTween.Kill(_laneTween, complete: false);

            _laneTween = transform.DOMoveX(lane * _laneOffset, _laneChangeSpeed).SetEase(Ease.Linear);
            _lane = (PlayerLane)lane;
        }
        
        private void Jump()
        {
            if (IsJumping || _slideTween != null) return;

            _slideTween.Kill(complete: false);
            _jumpSequence.Kill(complete: false);

            var speed = SpeedMod * 0.5f;

            var sequence = DOTween.Sequence();
            sequence.Append(transform.DOMoveY(_jumpHeight, _jumpLength * speed).SetEase(Ease.OutSine));
            sequence.Append(transform.DOMoveY(0f, _jumpLength * speed).SetEase(Ease.InSine));
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