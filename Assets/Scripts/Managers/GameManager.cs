using UnityEngine;

namespace Runner.Managers
{
    public class GameManager : Singleton<GameManager>
    {
        public static readonly float BaseGameSpeed = 5f;
        public static float GameSpeed;
        public static bool IsPlaying = true;

        [SerializeField] private AnimationCurve _gameSpeedCurve;
        
        [HideInInspector] public float maxGameSpeed;
        [HideInInspector] public float gameTime;

        public static float SpeedMultiplier { get; private set; }

        public static void EndGame()
        {
            IsPlaying = false;
            GameSpeed = 0f;
        }

        protected override void Awake()
        {
            base.Awake();

            maxGameSpeed = _gameSpeedCurve[_gameSpeedCurve.length - 1].value;

            Update();
        }

        private void Start()
        {

        }

        private void Update()
        {
            if (!IsPlaying) return;

            gameTime += Time.deltaTime;
            GameSpeed = _gameSpeedCurve.Evaluate(gameTime);
        }

        
    }
}