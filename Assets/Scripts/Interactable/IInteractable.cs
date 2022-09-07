using UnityEngine;

namespace Runner.Interactable
{
    public interface IInteractable
    {
        public void OnInteractStart(GameObject player);
        public void OnInteractStop(GameObject player);
    }
}
