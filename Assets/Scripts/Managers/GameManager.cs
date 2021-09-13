using Runner.Player;
using Runner.UI;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Runner.Managers
{
    public class GameManager : Singleton<GameManager>
    {
        public static readonly float BaseGameSpeed = 5f;
        public static readonly float BaseScoreMult = 0.1f;

        public static float GameSpeed = 0f;

        public static float CurrentScore = 0f;

        public static float MaxScore = 0f;
        public static int CurrentCoins = 0;
        public static int Balance = 0;
        public static float ScoreMult = 1f;

        public static bool IsPlaying = false;

        [SerializeField] private AnimationCurve _gameSpeedCurve;

        [HideInInspector] public float maxGameSpeed;
        [HideInInspector] public float gameTime;

        public static float SpeedMultiplier { get; private set; }

        public static void StartSession()
        {
            IsPlaying = true;
            PlayerController.Inst.enabled = true;
            ViewManager.HideAllViews();
            ViewManager.ShowView<PlayerHud>();
        }

        // When player leaved end game screen
        public static void EndSession()
        {
            //TODO: Save data
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        // When player hits obstacle
        public static void EndGame()
        {
            IsPlaying = false;
            GameSpeed = 0f;
        }

        public static void AddCoins(int coins = 1)
        {
            CurrentCoins += coins;
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

            CurrentScore += gameTime * GameSpeed * ScoreMult * BaseScoreMult;
        }
    }
}