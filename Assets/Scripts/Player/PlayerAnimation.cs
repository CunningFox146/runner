using Runner.Managers;
using Runner.Shop;
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
            GameManager.Inst.SelectedItemChanged += SetSkin;
            SetSkin(GameManager.GetSelectedItem());
        }

        private void SetSkin(string infoName)
        {
            if (_animator != null)
            {
                Destroy(_animator.gameObject);
            }
            GameObject skin = _shopItems.GetItem(infoName).skinPrefab;
            _animator = Instantiate(skin, transform).GetComponent<Animator>();
        }

        public void SetState(int state)
        {
            if (_animator != null)
            {
                _animator.SetInteger(_stateHash, state);
            }
        }
    }
}