using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.Shop
{
    [CreateAssetMenu(fileName = "ShopItems", menuName = "Scriptable Objects/ShopItems", order = 4)]
    public class ShopItems : ScriptableObject
    {
        public List<ShopItemInfo> items;

        public ShopItemInfo GetItem(string name) => items.Find((item)=>item.itemName == name);
    }
}