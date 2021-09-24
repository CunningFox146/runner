 using UnityEngine;

namespace Runner
{
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        public static T Inst { get; private set; }

        protected virtual void Awake()
        {
            if (Inst != null)
            {
                Debug.Log($"Tried to create new instance of singleton {typeof(T)}");
                Destroy(gameObject);
                return;
            }
            Inst = this as T;
        }
    }
}
