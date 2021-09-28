using Runner.Shop;
using UnityEngine;
using UnityEngine.UI;

namespace Runner.UI
{
    public class ShopItem : MonoBehaviour
    {
        [SerializeField] private Text _name;
        [SerializeField] private Text _price;
        [SerializeField] private Image _icon;
        [SerializeField] private Transform _itemContainer;
        [Space]
        [SerializeField] private Sprite _lockSprite;
        [SerializeField] private Sprite _pickedSprite;

        public GameObject skinPrefab;

        private ItemStatus _status;
        public ItemStatus Status
        {
            get => _status;
            set
            {
                _status = value;
                UpdateStatus();
            }
        }

        public enum ItemStatus
        {
            Locked,
            Unlocked,
            Picked,
        }

        private void Start()
        {
            UpdateStatus();
        }

        public void Init(ShopItemInfo info)
        {
            skinPrefab = info.skinPrefab;

            _name.text = info.name;
            _price.text = info.price.ToString();
            Instantiate(info.previewPrefab, _itemContainer);
        }

        private void UpdateStatus()
        {
            switch (_status)
            {
                case ItemStatus.Locked:
                    _icon.gameObject.SetActive(true);
                    _icon.sprite = _lockSprite;
                    break;

                case ItemStatus.Unlocked:
                    _icon.gameObject.SetActive(false);
                    break;

                case ItemStatus.Picked:
                    _icon.gameObject.SetActive(true);
                    _icon.sprite = _pickedSprite;
                    break;
            }
        }
    }
}