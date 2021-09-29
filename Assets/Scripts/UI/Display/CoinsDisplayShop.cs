using DG.Tweening;
using Runner.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Runner.UI.Display
{
    public class CoinsDisplayShop : MonoBehaviour
    {
        private Text _text;
        private int _lastBalance = 0;

        private void Awake()
        {
            _text = GetComponent<Text>();
        }

        private void Start()
        {
            GameManager.BalanceChanged += BalanceChangedHandler;
            BalanceChangedHandler(GameManager.Balance, true);
        }

        private void BalanceChangedHandler(int balance)
        {
            BalanceChangedHandler(balance, false);
        }
        private void BalanceChangedHandler(int balance, bool isNotAnimating = false)
        {
            //isNotAnimating = true;
            if (isNotAnimating)
            {
                _text.text = balance.ToString();
            }
            else
            {
                int current = _lastBalance;
                DOTween.To(
                    () => current,
                    (val) => {
                        current = val;
                        _text.text = current.ToString();
                    },
                    balance,
                    1f);
            }

            _lastBalance = balance;
        }
    }
}
