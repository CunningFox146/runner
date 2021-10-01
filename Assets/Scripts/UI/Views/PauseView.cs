using DG.Tweening;
using Runner.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Runner.UI.Views
{
    public class PauseView : View
    {
        [SerializeField] private GridLayoutGroup _menu;
        [SerializeField] private Text _header;
        [SerializeField] private Text _countdown;

        private Tween _countdownTween;

        private Image _image;

        public override void Hide()
        {
            base.Hide();
            GameManager.Paused = false;
        }

        public override void Show()
        {
            base.Show();

            if (_countdownTween != null)
            {
                _countdownTween.Kill();
                _countdownTween = null;
            }

            GameManager.Paused = true;

            if (_image == null)
            {
                _image = GetComponent<Image>();
            }

            _image.gameObject.SetActive(true);
            _menu.gameObject.SetActive(true);
            _header.gameObject.SetActive(true);
            _countdown.gameObject.SetActive(false);
        }

        public void Resume()
        {
            _image.gameObject.SetActive(false);
            _menu.gameObject.SetActive(false);
            _header.gameObject.SetActive(false);
            _countdown.gameObject.SetActive(true);

            _countdown.text = "3";
            _countdownTween = DOTween.To(
                () => 2,
                x => _countdown.text = (x + 1).ToString(),
                0,
                3).OnComplete(() => Hide());
        }
    }
}
