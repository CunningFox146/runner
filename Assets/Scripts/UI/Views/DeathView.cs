using Runner.Managers;
using UnityEngine.UI;

namespace Runner.UI.Views
{
    public class DeathView : View
    {
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();

            _button.onClick.AddListener(() => GameManager.EndSession(true));
        }
    }
}