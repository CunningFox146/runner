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
            var skin = _shopItems.GetItem(GameManager.GetSelectedItem()).skinPrefab;
            _animator = Instantiate(skin, transform).GetComponent<Animator>();
        }

        public void SetState(int state)
        {
            _animator.SetInteger(_stateHash, state);
        }
    }
}