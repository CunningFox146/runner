using Runner.Managers;
using UnityEngine;

namespace Runner.UI
{
    public class DeathView : View
    {
        private void Update()
        {
            if (Input.anyKey)
            {
                ViewManager.HideView<DeathView>();
                GameManager.EndSession();
            }
        }
    }
}