using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
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

        private readonly int WalkableMask = 1 << 6;

        [SerializeField] private PlayerCollider _collider;
        [SerializeField] private float _rayLength;
        [SerializeField] private float _yOffset = 0.5f;
        [SerializeField] private float _TEMP_progress = 0f;
        [Header("Moving")]
        [SerializeField] private float _laneOffset = 4f;
        [SerializeField] private float _laneChangeSpeed = 4f; //Coordinates
        [Header("Jump")]
        [SerializeField] private float _jumpLength = 4f; //Seconds
        [SerializeField] private float _jumpHeight = 4f;
        [Header("Slide")]
        [SerializeField] private float _slideLength = 4f; //Seconds

        public float groundOffset;
        public Vector3 rayStart;
        private float _groundDist;

        private PlayerLane _lane = PlayerLane.Center;
        private Vector3 _targetPos = Vector3.zero;

        private bool _isJumping;
        private float _jumpStart; // Stop jumping after some distance so if the game goes faster we'll be able to scale the jump

        private bool _isSliding;
        private float _slideStart; // Same as jumping

        void Start()
        {
            UpdateYOffset();
        }

        void Update()
        {
            _TEMP_progress += Time.deltaTime;

            UpdateInput();
            UpdatePosition();
        }
        
        void FixedUpdate()
        {
            UpdateYOffset();
        }

        private float GetGroundOffsetAtPoint(Vector3 point)
        {
            var start = new Vector3(point.x, _rayLength, point.z);

            if (Physics.Raycast(start, -transform.up, out RaycastHit hit, _rayLength, WalkableMask))
            {
                return hit.point.y;
            }

            return _rayLength;
        }
        
        private void UpdatePosition()
        {
            var target = new Vector3(_targetPos.x, _yOffset + _targetPos.y, _targetPos.z);
            if (!_isJumping)
            {
                target.y += _groundDist;
            }

            transform.position = Vector3.MoveTowards(transform.position, target, _laneChangeSpeed * Time.deltaTime);
        }
        
        private void UpdateInput()
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

        private void UpdateYOffset()
        {
            var pos = transform.position;
            float point = GetGroundOffsetAtPoint(pos);
            _groundDist = point;

            Debug.DrawLine(pos, new Vector3(pos.x, point, pos.z), Color.cyan);
        }

        private IEnumerator JumpCoroutine()
        {
            int ticks = 0;
            float startPos = _yOffset + _groundDist;
            while (_isJumping)
            {
                float progress = Mathf.Min((_TEMP_progress - _jumpStart) / _jumpLength, 1f);
                bool isTooLow = _groundDist >= transform.position.y - _yOffset;
                if (progress >= 1f || (isTooLow && ticks > 1))
                {
                    _isJumping = false;
                    _targetPos.y = 0f;
                    yield break;
                }

                _targetPos.y = startPos + Mathf.Sin(progress * Mathf.PI) * _jumpHeight;
                ticks++;
                yield return null;
            }
        }

        private IEnumerator SlideCoroutine()
        {
            while (_isSliding)
            {
                float progress = (_TEMP_progress - _slideStart) / _slideLength;

                if (progress >= 1f)
                {
                    _isSliding = false;
                    _collider.StopSliding();
                    yield break;
                }

                yield return null;
            }
        }
        
        private void ChangeSide(int sideDelta)
        {
            int lane = (int)_lane + sideDelta;
            if (!Enum.IsDefined(typeof(PlayerLane), lane)) return;

            float pos = lane * _laneOffset;
            float offset = GetGroundOffsetAtPoint(new Vector3(pos, 0f, transform.position.z));
            if (offset > transform.position.y)
            {
                //Do something
                Debug.Log("SIDE IS TOO HIGH");
                return;
            };
            _targetPos.x = pos;
            _lane = (PlayerLane)lane;
        }

        private void Jump()
        {
            if (_isSliding)
            {
                _isSliding = false;
                _collider.StopSliding();
            }

            if (_isJumping) return;

            _isJumping = true;
            _jumpStart = _TEMP_progress;
            StartCoroutine(JumpCoroutine());
        }

        private void Slide()
        {
            if (_isJumping)
            {
                _isJumping = false;
                _targetPos.y = 0f;
            }

            if (_isSliding) return;
            _isSliding = true;
            _slideStart = _TEMP_progress;
            StartCoroutine(SlideCoroutine());

            _collider.StartSliding();
        }

        public void OnHitObstacle(GameObject obstacle)
        {
            Debug.Log("DEATH");

            enabled = false;
        }
    }
}