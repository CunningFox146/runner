using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

namespace Runner.Player
{
    public class PlayerController : MonoBehaviour
    {
        private readonly int WalkableMask = 1 << 6;

        [SerializeField] private PlayerCollider _collider;
        [SerializeField] private float _TEMP_progress = 0f;
        [SerializeField] private Text _debugText;
        [Header("Lanes")]
        [SerializeField] private float[] _lanes;
        [SerializeField] private int _lane;
        [Header("Moving")]
        [SerializeField] private float _laneChangeTime = 1f;
        [SerializeField] private AnimationCurve _fallingCurve;
        [Header("Jump")]
        [SerializeField] private float _jumpTime = 1;
        [SerializeField] private float _jumpHeight = 4f;
        [SerializeField] private AnimationCurve _jumpingCurve;
        [Header("Slide")]
        [SerializeField] private float _slideTime = 4f;
        [SerializeField] private float _slideFallMult = 2f;
        [SerializeField] private AnimationCurve _slidingCurve;
        [Header("Fall")]
        [SerializeField] private float _fallSpeed = 4f;
        
        private PlayerState _state = PlayerState.Running;
        
        private Coroutine _fallCoroutine;
        private Coroutine _jumpCoroutine;
        private Coroutine _slideCoroutine;
        private Coroutine _sideCoroutine;

        private bool _isGrounded;
        private Vector3 _groundPos;

        private string DebugString => $"State:{_state}\nIsGrounded:{_isGrounded.ToString()}\nGround:{_groundPos.ToString()}\nCurrentLane:{_lane}";
        
        public enum PlayerState
        {
            Running,
            Falling,
            Jump,
            Slide,
            Death
        }
        
        void Start()
        {
            _groundPos = CheckGround();
        }

        void Update()
        {
            _TEMP_progress += Time.deltaTime;
            _debugText.text = DebugString;

            UpdateInput();
            UpdatePosition();
        }
        
        void FixedUpdate()
        {
            _groundPos = CheckGround();
        }

        private void UpdateInput()
        {
            if (Input.GetKeyDown(KeyCode.A) || SwipeManager.SwipeLeft)
            {
                ChangeSide(-1);
            }
            else if (Input.GetKeyDown(KeyCode.D) || SwipeManager.SwipeRight)
            {
                ChangeSide(1);
            }

            if (Input.GetKey(KeyCode.W) || SwipeManager.SwipeUp)
            {
                Jump();
            }
            else if (Input.GetKeyDown(KeyCode.S) || SwipeManager.SwipeDown)
            {
                Slide();
            }
        }

        private bool CanChangeSide(Vector3 direction)
        {
            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, 4f, WalkableMask))
            {
                return false;
            }

            return true;
        }

        private void ChangeSide(int sideDelta)
        {
            int lane = _lane + sideDelta;
            if (lane < 0 || lane + 1 > _lanes.Length || !CanChangeSide(new Vector3(sideDelta, 0, 0f)))
            {
                //TODO Hit player
                return;
            };

            _lane = lane;

            if (_sideCoroutine != null)
            {
                StopCoroutine(_sideCoroutine);
                _sideCoroutine = null;
            }

            _sideCoroutine = StartCoroutine(SideCoroutine());
        }

        private void Jump()
        {
            if (_slideCoroutine != null)
            {
                StopCoroutine(_slideCoroutine);
                _slideCoroutine = null;
                _collider.StopSliding();
            }

            if (_state == PlayerState.Slide)
            {
                _state = PlayerState.Running;
            }

            if (_state == PlayerState.Jump || !_isGrounded) return;
            _isGrounded = false;
            _state = PlayerState.Jump;

            if (_fallCoroutine != null)
            {
                StopCoroutine(_fallCoroutine);
                _fallCoroutine = null;
            }

            if (_jumpCoroutine != null)
            {
                StopCoroutine(_jumpCoroutine);
                _jumpCoroutine = null;
            }

            _jumpCoroutine = StartCoroutine(JumpingCoroutine());

        }

        private void Slide()
        {
            if (_state == PlayerState.Jump && _jumpCoroutine != null)
            {
                StopCoroutine(_jumpCoroutine);
                _jumpCoroutine = null;
            }

            if (_state == PlayerState.Slide) return;
            _state = PlayerState.Slide;

            if (_slideCoroutine != null)
            {
                StopCoroutine(_slideCoroutine);
                _slideCoroutine = null;
            }
            
            _slideCoroutine = StartCoroutine(SlidingCoroutine());

            _collider.StartSliding();
            
            // Apply fall multiplier
            if (!_isGrounded)
            {
                if (_fallCoroutine != null)
                {
                    StopCoroutine(_fallCoroutine);
                    _fallCoroutine = null;
                }
            }
        }

        private Vector3 CheckGround()
        {
            float rayLength = 1f;
            float minDist = 0.1f;
            var rayStart = transform.position + new Vector3(0f, rayLength, 0f);

            if (Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, 20f, WalkableMask))
            {
                Debug.DrawLine(rayStart, hit.point, Color.red);
                _isGrounded = Vector3.Distance(rayStart, hit.point) < rayLength + minDist;
                
                return hit.point;
            }

            throw new UnityException("Failed to get ground!");
        }
        
        private void UpdatePosition()
        {
            if (_state == PlayerState.Jump) return;

            if (!_isGrounded)
            {
                _fallCoroutine ??= StartCoroutine(FallingCoroutine());
                return;
            }

            var pos = transform.position;
            transform.position = new Vector3(pos.x, Mathf.MoveTowards(pos.y, _groundPos.y, Time.deltaTime * 10f), pos.z);
        }

        IEnumerator JumpingCoroutine()
        {
            float startHeight = transform.position.y;
            float endHeight = transform.position.y + _jumpHeight;
            float timer = 0f;

            while (true)
            {
                Debug.Log("JUMP");
                timer = Mathf.Min(timer + Time.deltaTime, _jumpTime);
                float delta = _jumpingCurve.Evaluate(timer / _jumpTime);

                transform.position = new Vector3(transform.position.x,
                    Mathf.Lerp(startHeight, endHeight, delta),
                    transform.position.z);

                yield return null;

                if (Mathf.Approximately(timer, _jumpTime)) break;
            }

            _state = PlayerState.Falling;
            _jumpCoroutine = null;
        }

        IEnumerator FallingCoroutine()
        {
            float startHeight = transform.position.y;
            float timer = 0f;
            float fallTime = transform.position.y / _fallSpeed;

            if (_slideCoroutine != null)
            {
                fallTime *= _slideFallMult;
            }

            _state = PlayerState.Falling;

            while (true)
            {
                Debug.Log("FALL");
                float endHeight = _groundPos.y;
                timer = Mathf.Clamp(timer + Time.deltaTime, 0f, fallTime);
                float delta = _fallingCurve.Evaluate(timer / fallTime);
                
                transform.position = new Vector3(transform.position.x,
                    Mathf.Lerp(startHeight, endHeight, delta),
                    transform.position.z);

                yield return null;

                if (_isGrounded || Mathf.Approximately(timer, fallTime)) break;
            }

            _state = PlayerState.Running;
            _fallCoroutine = null;
        }

        IEnumerator SideCoroutine()
        {
            float startPos = transform.position.x;
            float endPos = _lanes[_lane];
            float timer = 0f;

            while (true)
            {
                Debug.Log("SIDE");
                timer = Mathf.Min(timer + Time.deltaTime, _laneChangeTime);
                float delta = _slidingCurve.Evaluate(timer / _laneChangeTime);

                transform.position = new Vector3(Mathf.Lerp(startPos, endPos, delta),
                    transform.position.y,
                    transform.position.z);

                yield return null;

                if (Mathf.Approximately(timer, _laneChangeTime)) break;
            }

            _state = PlayerState.Running;
            _sideCoroutine = null;
        }

        IEnumerator SlidingCoroutine()
        {
            float timer = 0f;
            while (true)
            {
                if (_isGrounded)
                {
                    _state = PlayerState.Slide;
                }
                Debug.Log("SLIDE");
                timer = Mathf.Min(timer + Time.deltaTime, _slideTime);
                if (Mathf.Approximately(timer, _slideTime)) break;

                yield return null;
            }

            _state = PlayerState.Running;
            _collider.StopSliding();
            _slideCoroutine = null;
        }
        
        public void OnHitObstacle(GameObject obstacle)
        {
            Debug.Log("DEATH");

            enabled = false;
        }
    }
}