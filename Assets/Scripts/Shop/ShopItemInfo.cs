using System.Collections;
using UnityEngine;

namespace Runner.Shop
{
    [CreateAssetMenu(fileName = "ItemInfo", menuName = "Scriptable Objects/ShopItemInfo", order = 3)]
    public class ShopItemInfo : ScriptableObject
    {
        public string itemName;
        public int price;
        public GameObject previewPrefab;
        public GameObject skinPrefab;
    }
}