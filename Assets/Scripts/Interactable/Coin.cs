using DG.Tweening;
using Runner.Managers;
using Runner.SoundSystem;
using UnityEngine;

namespace Runner.Interactable
{
    public class Coin : MonoBehaviour, IInteractable
    {
        [SerializeField] private Transform _model;

        private Tween _tween;

        void Start()
        {
            _tween = transform.DORotate(new Vector3(0f, -360f, 0f), 3f, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(-1);
        }

        void OnDestroy()
        {
            if (_tween != null)
            {
                _tween.Kill();
            }
        }

        public void OnInteractStart(GameObject player)
        {
            GetComponent<SoundsEmitter>().Play("Coin");
            _tween.Kill();
            _model.DOScale(Vector3.one * 0.5f, 0.25f);
            _model.DOMove(player.transform.position + Vector3.up * 0.5f, 0.25f)
                .OnComplete(() =>
                {
                    GameManager.AddCoins();
                    Destroy(gameObject);
                });
        }

        public void OnInteractStop(GameObject player) { }
    }
}