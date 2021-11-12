using Runner.Managers;

namespace Runner.UI.Display
{
    public class CoinsDisplay : Displayer
    {
        protected override void UpdateText()
        {
            _text.text = GameManager.CurrentCoins.ToString();
        }
    }
}
