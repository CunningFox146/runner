using UnityEngine;

namespace Runner.Managers
{
    public class SwipeManager : Singleton<SwipeManager>
    {
        private readonly bool IsMobile = Application.isMobilePlatform;

        [SerializeField] private float _tapRange = 10f;
        [SerializeField] private float _swipeRange = 50f;

        private Vector2 _touchStart;
        private Vector2 _touchCurrent;
        //private Vector2 _touchEnd;
        private bool _isSwipeStopped;

        public static bool SwipeRight { get; private set; }
        public static bool SwipeLeft { get; private set; }
        public static bool SwipeUp { get; private set; }
        public static bool SwipeDown { get; private set; }
        public static bool IsTapping { get; private set; }

        private bool IsTouching => IsMobile ? Input.touchCount > 0 : Input.GetMouseButton(0);
        private Vector2 TouchPosition => IsMobile ? Input.GetTouch(0).position : (Vector2)Input.mousePosition;

        void Update()
        {
            Swipe();
        }

        void LateUpdate()
        {
            ResetSwipes();
        }

        private TouchPhase GetPhase()
        {
            if (IsMobile) return Input.GetTouch(0).phase;
            if (Input.GetMouseButtonDown(0)) return TouchPhase.Began;
            if (Input.GetMouseButtonUp(0)) return TouchPhase.Ended;
            return TouchPhase.Moved;
        }

        private void Swipe()
        {
            TouchPhase phase = GetPhase();

            // This is here bc IsTouching is checking GetMouseButtonUp too
            if (phase == TouchPhase.Ended)
            {
                _isSwipeStopped = false;
                //_touchEnd = TouchPosition;

                Vector2 dist = _touchCurrent - _touchStart;

                if (Mathf.Abs(dist.x) < _tapRange || Mathf.Abs(dist.y) < _tapRange)
                {
                    IsTapping = true;
                }
            }

            if (!IsTouching) return;

            if (phase == TouchPhase.Began)
            {
                ResetSwipes();

                _isSwipeStopped = false;
                _touchStart = TouchPosition;
            }

            if (phase == TouchPhase.Moved)
            {
                _touchCurrent = TouchPosition;

                if (!_isSwipeStopped)
                {
                    OnTouchMoved();
                }
            }
        }

        private void OnTouchMoved()
        {
            Vector2 dist = _touchCurrent - _touchStart;
            if (dist.x < -_swipeRange)
            {
                SwipeLeft = true;
                _isSwipeStopped = true;
            }
            if (dist.x > _swipeRange)
            {
                SwipeRight = true;
                _isSwipeStopped = true;
            }
            if (dist.y < -_swipeRange)
            {
                SwipeDown = true;
                _isSwipeStopped = true;
            }
            if (dist.y > _swipeRange)
            {
                SwipeUp = true;
                _isSwipeStopped = true;
            }
        }

        private void ResetSwipes()
        {
            SwipeRight = false;
            SwipeLeft = false;
            SwipeUp = false;
            SwipeDown = false;
            IsTapping = false;
        }
    }
}