using Runner.Player;
using UnityEngine;

namespace Runner.Shop
{
    [CreateAssetMenu(fileName = "ItemInfo", menuName = "Scriptable Objects/ShopItemInfo")]
    public class ShopItemInfo : ScriptableObject
    {
        [Tooltip("WARNING: this field has to be unique.")]
        public string itemName;

        public int price;
        public GameObject skinPrefab;
        public PlayerState shopAnimation;
    }
}