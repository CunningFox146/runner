using UnityEngine;

namespace Runner
{
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        public static T Inst { get; private set; }

        public virtual void Awake()
        {
            if (Inst != null)
            {
                Debug.LogWarning($"Tried to create new instance of singleton {typeof(T)}");
                return;
            }
            Inst = this as T;
        }
    }
}
