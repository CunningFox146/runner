using Runner.Managers;
using UnityEngine;

namespace Runner.UI
{
    public class StartView : View
    {
        public void OpenShop()
        {
            ViewManager.HideView<StartView>();
            ViewManager.ShowView<ShopView>();
        }
    }
}