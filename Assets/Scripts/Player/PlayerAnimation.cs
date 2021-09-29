using Runner.Managers;
using Runner.Shop;
using System;
using UnityEngine;

namespace Runner.Player
{
    public class PlayerAnimation : MonoBehaviour
    {
        [SerializeField] private ShopItems _shopItems;

        private Animator _animator;

        private int _stateHash;

        void Awake()
        {
            _stateHash = Animator.StringToHash("state");
        }

        private void Start()
        {
            GameManager.SelectedItemChanged += UpdateSkin;
            UpdateSkin(GameManager.GetSelectedItem());
        }

        private void UpdateSkin(string infoName)
        {
            if (_animator != null)
            {
                Destroy(_animator.gameObject);
            }
            var skin = _shopItems.GetItem(infoName).skinPrefab;
            _animator = Instantiate(skin, transform).GetComponent<Animator>();
        }

        public void SetState(int state)
        {
            _animator.SetInteger(_stateHash, state);
        }
    }
}