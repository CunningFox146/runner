using Runner.Managers;
using Runner.Shop;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.UI
{
    public class ShopGrid : MonoBehaviour
    {
        [SerializeField] private GameObject _itemPrefab;
        [SerializeField] private ShopItems _items;

        private List<ShopItem> _shopItems;
        private ShopItem _selectedItem;

        private void Awake()
        {
            _shopItems = new List<ShopItem>();
        }

        private void Start()
        {
            foreach (ShopItemInfo info in _items.items)
            {
                ShopItem item = Instantiate(_itemPrefab, transform).GetComponent<ShopItem>();
                item.Init(info, this);

                if (GameManager.IsItemBought(info.itemName))
                {
                    item.SetBought();
                }

                if (info.itemName == GameManager.SelectedItem)
                {
                    SelectItem(item);
                }
                _shopItems.Add(item);
            }
        }

        public void SelectItem(ShopItem item)
        {
            _selectedItem?.DeselectItem();
            _selectedItem = item;
            _selectedItem.SelectItem();

            GameManager.SelectItem(item.info);
        }
    }
}
