using DG.Tweening;
using Runner.Player;
using Runner.Shop;
using Runner.UI;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Runner.Player.PlayerController;

namespace Runner.Managers
{
    public class GameManager : Singleton<GameManager>
    {
        public static readonly float BaseGameSpeed = 5f;
        public static readonly float BaseScoreMult = 0.1f;

        public static float GameSpeed = 0f;

        public static float CurrentScore = 0f;

        public static int HighScore = 0;
        public static int CurrentCoins = 0;
        public static float ScoreMult = 1f;

        public static bool IsPlaying = false;

        private static bool RestartGameplay = false;
        private static int Coins = 0;

        public static event Action<int> BalanceChanged;

        [SerializeField] private AnimationCurve _gameSpeedCurve;

        [HideInInspector] public float maxGameSpeed;
        [HideInInspector] public float gameTime;

        private bool _paused;

        public static float SpeedMultiplier { get; private set; }
        public static bool Paused {
            get => Inst._paused;
            set => Inst._paused = value;
        }

        public static int Balance
        {
            get => Coins;
            set
            {
                Coins = value;
                BalanceChanged?.Invoke(Coins);
            }
        }

        public static void StartSession()
        {
            IsPlaying = true;
            ViewManager.HideAllViews();
            ViewManager.ShowView<PlayerHud>();
            PlayerController player = PlayerController.Inst;

            CameraManager.Inst.IsFollowing = true;

            player.GetComponent<PlayerAnimation>().SetState((int)PlayerState.Running);
            player.transform.DOMove(Vector3.zero, 0.5f).OnComplete(() => PlayerController.Inst.enabled = true);
        }

        // When player left end game screen
        public static void EndSession(bool restart = false)
        {
            var save = SaveManager.CurrentSave;

            save.coins = Balance + CurrentCoins;
            save.highScore = HighScore;

            SaveManager.SaveCurrent();

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            RestartGameplay = restart;
        }

        // When player hits obstacle
        public static void EndGame()
        {
            ViewManager.HideAllViews();
            ViewManager.ShowView<DeathView>();
            IsPlaying = false;
            GameSpeed = 0f;

            int score = (int)CurrentScore;
            if (score > SaveManager.CurrentSave.highScore)
            {
                HighScore = score;
            }
        }

        public static void AddCoins(int coins = 1)
        {
            CurrentCoins += coins;
        }

        public static bool BuyShopItem(ShopItemInfo info)
        {
            if (Balance < info.price)
            {
                return false;
            }

            Balance -= info.price;

            return true;
        }

        protected override void Awake()
        {
            base.Awake();

            DOTween.SetTweensCapacity(500, 50);

            SaveManager.LoadSave();
            var save = SaveManager.CurrentSave;
            Debug.Log(save);
            HighScore = save.highScore;
            Balance = save.coins;
            CurrentCoins = 0;
            CurrentScore = 0;

            maxGameSpeed = _gameSpeedCurve[_gameSpeedCurve.length - 1].value;

            Update();
        }

        private void Start()
        {
            if (RestartGameplay)
            {
                StartSession();
            }
            RestartGameplay = false;
        }

        private void Update()
        {
            if (!IsPlaying || Paused) return;

            gameTime += Time.deltaTime;
            GameSpeed = _gameSpeedCurve.Evaluate(gameTime);

            CurrentScore += gameTime * GameSpeed * ScoreMult * BaseScoreMult;
        }
    }
}