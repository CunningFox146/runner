using Runner.ExtentionClasses;
using Runner.Managers;
using Runner.Shop;
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

        public ShopItemInfo info;

        private ShopGrid _shop;
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

        public void Init(ShopItemInfo itemInfo, ShopGrid shop)
        {
            _shop = shop;

            info = itemInfo;

            _name.text = info.name;
            _priceText.text = info.price.ToString();

            GameObject preview = Instantiate(info.skinPrefab, _itemContainer);
            preview.transform.localScale = Vector3.one * 135f;
            preview.transform.localPosition = new Vector3(0, -150f, -150f);
            preview.transform.eulerAngles = new Vector3(0, 180f, 0f);
            preview.transform.SetLayerRecursively((int)Layers.UI);
            Destroy(preview.GetComponent<Animator>());
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
                    _shop.SelectItem(this);
                    break;

                default: break;
            }
        }

        private void BuyItem()
        {
            if (GameManager.BuyShopItem(info))
            {
                CameraManager.Inst.PlaySound("ItemBought");
                SetBought();
            }
        }

        public void SetBought()
        {
            if (Status == ItemStatus.Locked)
            {
                Status = ItemStatus.Unlocked;
            }
        }

        public void SelectItem()
        {
            Status = ItemStatus.Picked;
        }

        public void DeselectItem()
        {
            Status = ItemStatus.Unlocked;
        }
    }
}