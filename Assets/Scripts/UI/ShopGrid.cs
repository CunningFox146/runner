using Runner.Shop;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.UI
{
    public class ShopGrid : MonoBehaviour
    {
        [SerializeField] private GameObject _itemPrefab;
        [SerializeField] private List<ShopItemInfo> _itemsInfo;

        private List<ShopItem> _shopItems;
        private ShopItem _selectedItem;

        private void Awake()
        {
            _shopItems = new List<ShopItem>();
        }

        private void Start()
        {
            for (int i = 0; i < 10; i++)
            {
                ShopItem item = Instantiate(_itemPrefab, transform).GetComponent<ShopItem>();
                item.Init(_itemsInfo[0], this);
                _shopItems.Add(item);
            }
        }

        public void SelectItem(ShopItem item)
        {
            _selectedItem?.DeselectItem();
            _selectedItem = item;
        }
    }
}
