using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace Runner.Interactable
{
    public class Coin : MonoBehaviour, IInteractable
    {
        [SerializeField] private Transform _model;

        void Start()
        {
            transform.DORotate(new Vector3(0f, -360f, 0f), 1f, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(-1);
        }

        public void OnInteractStart(GameObject player)
        {
            Destroy(gameObject);
        }

        public void OnInteractStop(GameObject player) {}
    }
}