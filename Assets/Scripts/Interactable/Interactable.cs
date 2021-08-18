using UnityEngine;

namespace Runner.Interactable
{
    public class Interactable : MonoBehaviour
    {
        protected virtual void OnInteract(GameObject player)
        {
            Destroy(gameObject);
        }

        protected virtual void OnTriggerEnter(Collider coll)
        {
            var obj = coll.transform.root.gameObject;
            if (obj.CompareTag("Player"))
            {
                OnInteract(obj);
            }
        }
    }
}
