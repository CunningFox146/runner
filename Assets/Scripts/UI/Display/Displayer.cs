using UnityEngine;
using UnityEngine.UI;

namespace Runner.UI.Display
{
    public abstract class Displayer : MonoBehaviour
    {
        protected Text _text;

        protected virtual void Awake()
        {
            _text = GetComponent<Text>();
        }

        protected virtual void Start()
        {
            UpdateText();
        }

        protected virtual void LateUpdate()
        {
            UpdateText();
        }

        protected abstract void UpdateText();
    }
}