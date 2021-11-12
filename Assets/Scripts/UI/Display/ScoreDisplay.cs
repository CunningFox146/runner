using Runner.Managers;

namespace Runner.UI.Display
{
    public class ScoreDisplay : Displayer
    {
        protected override void UpdateText()
        {
            _text.text = ((int)(GameManager.CurrentScore)).ToString();
        }
    }
}
