using DG.Tweening;
using Runner.Player;
using Runner.Shop;
using Runner.SoundSystem;
using Runner.UI;
using System;
using UnityEngine;
using UnityEngine.Audio;
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

        public event Action<int> BalanceChanged;
        public event Action<string> SelectedItemChanged;

        [SerializeField] private ShopItems _shopItems;
        [SerializeField] private AnimationCurve _gameSpeedCurve;
        [SerializeField] private AudioMixer _currentMixer;

        [HideInInspector] public float maxGameSpeed;
        [HideInInspector] public float gameTime;

        private bool _paused;
        private SoundsEmitter _sound;

        public static float SpeedMultiplier { get; private set; }
        public static bool Paused
        {
            get => Inst._paused;
            set => Inst._paused = value;
        }

        public static int Balance
        {
            get => Coins;
            set
            {
                Coins = value;
                Inst.BalanceChanged?.Invoke(Coins);
            }
        }
        public static string GetSelectedItem()
        {
            GameSave save = SaveManager.CurrentSave;
            if (save != null && !string.IsNullOrEmpty(save.selectedItem))
            {
                return SaveManager.CurrentSave.selectedItem;
            }
            return Inst._shopItems.defaultSelectedItem.itemName;
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

            CameraManager.Inst.StopSound("BgMusic");
            CameraManager.Inst.PlaySound("Music/GameplayMusic", "BgMusic");
        }

        //Called at the end of the game and when buying/selecting something
        public static void Save()
        {
            GameSave save = SaveManager.CurrentSave;

            save.coins = Balance + CurrentCoins;
            save.highScore = HighScore;

            SaveManager.SaveCurrent();
        }

        // When player left end game screen
        public static void EndSession(bool restart = false)
        {
            Save();

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

            Inst._currentMixer.DOSetFloat("MusicPitch", 0.5f, 1f);
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
            SaveManager.CurrentSave.boughtItems.Add(info.itemName);
            Save();

            return true;
        }

        public static void SelectItem(ShopItemInfo info)
        {
            SaveManager.CurrentSave.selectedItem = info.itemName;
            Inst.SelectedItemChanged?.Invoke(info.itemName);
            Save();
        }

        public static bool IsItemBought(string name) => SaveManager.CurrentSave.boughtItems.Contains(name);

        protected override void Awake()
        {
            base.Awake();

            DOTween.SetTweensCapacity(500, 50);

            SaveManager.LoadSave();
            GameSave save = SaveManager.CurrentSave;
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
            CameraManager.Inst.StopSound("BgMusic");
            Inst._currentMixer.SetFloat("MusicPitch", 1f);

            if (RestartGameplay)
            {
                StartSession();
            }
            else
            {
                CameraManager.Inst.PlaySound("Music/MainMenuMusic", "BgMusic");
            }
            RestartGameplay = false;

            // Auto-add to bought items that cost 0
            foreach (ShopItemInfo item in _shopItems.items)
            {
                if (item.price <= 0)
                {
                    BuyShopItem(item);
                }
            }
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