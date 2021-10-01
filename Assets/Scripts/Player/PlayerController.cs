using Runner.Managers;
using Runner.SoundSystem;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Runner.Player
{
    public class PlayerController : Singleton<PlayerController>
    {
        [SerializeField] private PlayerCollider _collider;
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

        private Rigidbody _rb;
        private PlayerAnimation _animation;
        private SoundsEmitter _sound;

        private PlayerState _state;

        private Coroutine _fallCoroutine;
        private Coroutine _jumpCoroutine;
        private Coroutine _slideCoroutine;
        private Coroutine _sideCoroutine;

        private bool _isGrounded;
        private Vector3 _groundPos;

        private PlayerState State
        {
            get => _state;
            set
            {
                _state = value;
                _animation.SetState((int)_state);
            }
        }
        private string DebugString => $"State:{State}\nIsGrounded:{_isGrounded.ToString()}\nGround:{_groundPos.ToString()}\nCurrentLane:{_lane}";

        public enum PlayerState
        {
            Running,
            Jump,
            Falling,
            Slide,
            Death,
            SideChange,
        }

        protected override void Awake()
        {
            base.Awake();
            _rb = GetComponent<Rigidbody>();
            _animation = GetComponent<PlayerAnimation>();
            _sound = GetComponent<SoundsEmitter>();
        }

        void Start()
        {
            _groundPos = CheckGround();

            State = PlayerState.Running;
        }

        void Update()
        {
            if (GameManager.Paused) return;

            if (_debugText != null)
            {
                _debugText.text = DebugString;
            }

            UpdateInput();
            UpdatePosition();
        }

        void FixedUpdate()
        {
            if (GameManager.Paused) return;

            _groundPos = CheckGround();
            //CheckObstacleHit();
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

            if (Input.GetKeyDown(KeyCode.W) || SwipeManager.SwipeUp)
            {
                Jump();
            }
            else if (Input.GetKeyDown(KeyCode.S) || SwipeManager.SwipeDown)
            {
                Slide();
            }
        }

        private bool CanChangeSide(Vector3 direction) => !Physics.Raycast(
            transform.position + new Vector3(0f, 1f, 0f),
            direction, out RaycastHit hit, 4f);

        private void ChangeSide(int sideDelta)
        {
            int lane = _lane + sideDelta;
            if (lane < 0 || lane + 1 > _lanes.Length || !CanChangeSide(new Vector3(sideDelta, 0f, 0f)))
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
            _sound.Play("Jump");

            if (_slideCoroutine != null)
            {
                StopCoroutine(_slideCoroutine);
                _slideCoroutine = null;
                _collider.StopSliding();
            }

            if (State == PlayerState.Slide)
            {
                State = PlayerState.Running;
            }

            if (State == PlayerState.Jump || !_isGrounded) return;
            _isGrounded = false;
            State = PlayerState.Jump;

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
            if (State == PlayerState.Jump && _jumpCoroutine != null)
            {
                StopCoroutine(_jumpCoroutine);
                _jumpCoroutine = null;
            }

            if (State == PlayerState.Slide) return;
            State = PlayerState.Slide;

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
            Vector3 rayStart = transform.position + new Vector3(0f, rayLength, 0f);

            if (Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, 20f, 1 << (int)Layers.Walkable))
            {
                Debug.DrawLine(rayStart, hit.point, Color.red);
                _isGrounded = Vector3.Distance(rayStart, hit.point) < rayLength + minDist;

                return hit.point;
            }

            throw new UnityException("Failed to get ground!");
        }

        private void UpdatePosition()
        {
            if (State == PlayerState.Jump) return;

            if (!_isGrounded)
            {
                _fallCoroutine ??= StartCoroutine(FallingCoroutine());
                return;
            }

            Vector3 pos = transform.position;
            transform.position = new Vector3(pos.x, Mathf.MoveTowards(pos.y, _groundPos.y, Time.deltaTime * 10f), pos.z);
        }

        IEnumerator JumpingCoroutine()
        {
            float startHeight = transform.position.y;
            float endHeight = transform.position.y + _jumpHeight;
            float timer = 0f;

            while (true)
            {
                if (GameManager.Paused)
                {
                    yield return null;
                }

                //Debug.Log("JUMP");
                timer = Mathf.Min(timer + Time.deltaTime, _jumpTime);
                float delta = _jumpingCurve.Evaluate(timer / _jumpTime);

                transform.position = new Vector3(transform.position.x,
                    Mathf.Lerp(startHeight, endHeight, delta),
                    transform.position.z);

                yield return null;

                if (Mathf.Approximately(timer, _jumpTime)) break;
            }

            State = PlayerState.Falling;
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

            State = PlayerState.Falling;

            while (true)
            {
                if (GameManager.Paused)
                {
                    yield return null;
                }

                //Debug.Log("FALL");
                float endHeight = _groundPos.y;
                timer = Mathf.Clamp(timer + Time.deltaTime, 0f, fallTime);
                float delta = _fallingCurve.Evaluate(timer / fallTime);

                transform.position = new Vector3(transform.position.x,
                    Mathf.Lerp(startHeight, endHeight, delta),
                    transform.position.z);

                yield return null;

                if (_isGrounded || Mathf.Approximately(timer, fallTime)) break;
            }
            _sound.Play("Land");
            State = PlayerState.Running;
            _fallCoroutine = null;
        }

        IEnumerator SideCoroutine()
        {
            float startPos = transform.position.x;
            float endPos = _lanes[_lane];
            float timer = 0f;

            if (_isGrounded)
            {
                State = PlayerState.SideChange;
            }

            while (true)
            {
                if (GameManager.Paused)
                {
                    yield return null;
                }

                //Debug.Log("SIDE");
                timer = Mathf.Min(timer + Time.deltaTime, _laneChangeTime);
                float delta = _slidingCurve.Evaluate(timer / _laneChangeTime);

                transform.position = new Vector3(Mathf.Lerp(startPos, endPos, delta),
                    transform.position.y,
                    transform.position.z);

                yield return null;

                if (Mathf.Approximately(timer, _laneChangeTime)) break;
            }

            State = PlayerState.Running;
            _sideCoroutine = null;
        }

        IEnumerator SlidingCoroutine()
        {
            float timer = 0f;
            while (true)
            {
                if (GameManager.Paused)
                {
                    yield return null;
                }

                if (_isGrounded)
                {
                    State = PlayerState.Slide;
                }
                //Debug.Log("SLIDE");
                timer = Mathf.Min(timer + Time.deltaTime, _slideTime);
                if (Mathf.Approximately(timer, _slideTime)) break;

                yield return null;
            }

            State = PlayerState.Running;
            _collider.StopSliding();
            _slideCoroutine = null;
        }

        //private void CheckObstacleHit()
        //{
        //    var start = transform.position + Vector3.up * 0.25f;
        //    Debug.DrawLine(start, transform.forward * 0.5f, Color.red);
        //    if (Physics.Raycast(start, Vector3.forward, out RaycastHit hit, 0.5f))
        //    {
        //        if (hit.transform.root.CompareTag("Evaluation") || hit.transform.gameObject.layer == InteractableMask) return;

        //        if (hit.point.z -start.z <= 0.1f)
        //        {
        //            OnHitObstacle(hit);
        //        }
        //    }
        //}

        public void OnHitObstacle(GameObject obstacle)
        {
            _sound.Play("Hit");

            GameManager.EndGame();

            if (_sideCoroutine != null)
            {
                StopCoroutine(_sideCoroutine);
                _sideCoroutine = null;
            }
            if (_slideCoroutine != null)
            {
                StopCoroutine(_slideCoroutine);
                _slideCoroutine = null;
            }
            if (_jumpCoroutine != null)
            {
                StopCoroutine(_jumpCoroutine);
                _jumpCoroutine = null;
            }

            if (!_isGrounded)
            {
                CameraManager.Inst.IsFollowing = false;
                Vector3 dir = (Camera.main.transform.position - transform.position).normalized;
                _rb.constraints = RigidbodyConstraints.None;
                _rb.AddForce(dir * 15f, ForceMode.VelocityChange);
                _rb.AddTorque(dir * 10f, ForceMode.VelocityChange);
            }

            State = PlayerState.Death;
            enabled = false;
        }
    }
}