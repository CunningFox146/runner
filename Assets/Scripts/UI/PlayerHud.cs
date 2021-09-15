using Runner.Managers;

namespace Runner.UI
{
    public class PlayerHud : View
    {
        public void Pause()
        {
            ViewManager.ShowView<PauseView>();
        }
    }
}