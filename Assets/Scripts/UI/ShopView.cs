using Runner.Managers;
using UnityEngine;

namespace Runner.UI
{
    public class ShopView : View
    {
        public void CloseShop()
        {
            ViewManager.HideView<ShopView>();
            ViewManager.ShowView<StartView>();
        }
    }
}