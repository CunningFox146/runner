using Runner.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Runner.UI
{
    public class BasicButton : MonoBehaviour
    {
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();

            _button.onClick.AddListener(OnClickHandler);
        }

        private void OnClickHandler()
        {
            CameraManager.Inst.PlaySound("ButtonClick");
        }
    }
}