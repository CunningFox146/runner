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

        [SerializeField] private PlayerCollider _collider;
        [Header("Moving")]
        [SerializeField] private float _laneOffset = 4f;
        [SerializeField] private float _laneChangeSpeed = 4f;
        [SerializeField] private float _jumpLength = 4f;
        [SerializeField] private float _jumpHeight = 4f;
        [SerializeField] private float _slideLength = 4f;


        private PlayerLane _lane = PlayerLane.Center;
        private Vector3 _targetPos = Vector3.zero;
        
        private bool _isJumping;
        private float _jumpStart; // Stop jumping after some distance so if the game goes faster we'll be able to scale the jump

        private bool _isSliding;
        private float _slideStart; // Same as jumping

        private float _TEMP_progress = 0f;

        void Update()
        {
            _TEMP_progress += Time.deltaTime;

            UpdateInput();
            UpdatePosition();
            
            Debug.DrawLine(transform.position, _targetPos, Color.red);
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
        
        private void UpdateJump()
        {
            if (!_isJumping) return;

            float progress = (_TEMP_progress - _jumpStart) / _jumpLength;

            if (progress >= 1f)
            {
                _isJumping = false;
                _targetPos.y = 0f;
                return;
            }

            _targetPos.y = Mathf.Sin(progress * Mathf.PI) * _jumpHeight;
        }

        private void UpdateSlide()
        {
            float progress = (_TEMP_progress - _slideStart) / _slideLength;

            if (progress >= 1f)
            {
                _isSliding = false;
                _collider.StopSliding();
            }
        }

        private void UpdatePosition()
        {
            UpdateSlide();
            UpdateJump();

            transform.position = Vector3.MoveTowards(transform.position, _targetPos, Time.deltaTime * _laneChangeSpeed);
        }

        private void ChangeSide(int sideDelta)
        {
            int lane = (int)_lane + sideDelta;
            if (!Enum.IsDefined(typeof(PlayerLane), lane)) return;

            _targetPos.x = lane * _laneOffset;
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

            _collider.StartSliding();
        }
    }
}