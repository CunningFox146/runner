using Runner.Managers;

namespace Runner.UI.Views
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