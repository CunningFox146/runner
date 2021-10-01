using Runner.Managers;

namespace Runner.UI.Views
{
    public class PlayerHud : View
    {
        public void Pause()
        {
            ViewManager.ShowView<PauseView>();
        }
    }
}