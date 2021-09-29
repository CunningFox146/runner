using Runner.Shop;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Runner.UI
{
    public class ShopItem : MonoBehaviour
    {
        [SerializeField] private Text _name;
        [SerializeField] private Text _priceText;
        [SerializeField] private GameObject _price;
        [SerializeField] private Image _pickedIcon;
        [SerializeField] private Transform _itemContainer;

        public GameObject skinPrefab;

        private Button _button;
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

        private void Awake()
        {
            _button = GetComponent<Button>();

            _button.onClick.AddListener(OnClick);
        }

        private void Start()
        {
            UpdateStatus();
        }

        public void Init(ShopItemInfo info)
        {
            skinPrefab = info.skinPrefab;

            _name.text = info.name;
            _priceText.text = info.price.ToString();
            Instantiate(info.previewPrefab, _itemContainer);
        }

        private void UpdateStatus()
        {
            switch (_status)
            {
                case ItemStatus.Locked:
                    _pickedIcon.gameObject.SetActive(false);
                    _price.SetActive(true);
                    break;

                case ItemStatus.Unlocked:
                    _pickedIcon.gameObject.SetActive(false);
                    _price.SetActive(false);
                    break;

                case ItemStatus.Picked:
                    _pickedIcon.gameObject.SetActive(true);
                    _price.SetActive(false);
                    break;
            }
        }

        private void OnClick()
        {
            switch (_status)
            {
                case ItemStatus.Locked:
                    BuyItem();
                    break;

                case ItemStatus.Unlocked:
                    PickItem();
                    break;

                default: break;
            }
        }

        private void BuyItem()
        {
            Status = ItemStatus.Unlocked;
        }
        private void PickItem()
        {
            Status = ItemStatus.Picked;
        }
    }
}