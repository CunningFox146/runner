using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.UI
{
    public abstract class View : MonoBehaviour
    {
        public virtual void Show() => gameObject.SetActive(true);
        public virtual void Hide() => gameObject.SetActive(false);
    }
}