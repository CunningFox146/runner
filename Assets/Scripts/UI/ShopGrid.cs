using Runner.Shop;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.UI
{
    public class ShopGrid : MonoBehaviour
    {
        [SerializeField] private GameObject _itemPrefab;
        [SerializeField] private List<ShopItemInfo> _itemsInfo;

        private void Start()
        {
            for (int i = 0; i < 10; i++)
            {
                Instantiate(_itemPrefab, transform).GetComponent<ShopItem>().Init(_itemsInfo[0], this);
            }
        }
    }
}
