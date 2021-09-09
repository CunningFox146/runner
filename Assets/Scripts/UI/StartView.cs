using Runner.Managers;
using UnityEngine;

namespace Runner.UI
{
    public class StartView : View
    {
        private void Update()
        {
            if (Input.anyKey)
            {
                ViewManager.PopView();
                GameManager.StartSession();
            }
        }
    }
}