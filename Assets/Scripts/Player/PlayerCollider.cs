using System;
using System.Collections;
using System.Collections.Generic;
using Runner.Interactable;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Runner.Player
{
    public class PlayerCollider : MonoBehaviour
    {
        [Range(0f, 1f)]
        [SerializeField] private float _slideScale = 0.5f;
        [SerializeField] private PlayerController _controller;

        private BoxCollider _collider;
        private Vector3 _sizeStart;
        private Vector3 _centerStart;
        
        void Awake()
        {
            _collider = GetComponent<BoxCollider>();
        }

        void Start()
        {
            _centerStart = _collider.center;
            _sizeStart = _collider.size;

            var min = _collider.center - _collider.size;
            var max = _collider.center + _collider.size;
        }

        void OnTriggerEnter(Collider collider)
        {
            if (collider.CompareTag("Evaluation") ||
                (collider.gameObject.layer == (int)Layers.Walkable &&
                 Vector3.Distance(transform.position, collider.transform.position) >= 1.5f)) // If distance is far then we landed on the ground and ignore hit
                return;
            
            if (collider.TryGetComponent(out IInteractable interactable))
            {
                interactable.OnInteractStart(gameObject);
                return;
            }
            
            _controller.OnHitObstacle(collider.gameObject);
        }

        void OnTriggerExit(Collider collider)
        {
            if (collider.TryGetComponent(out IInteractable interactable))
            {
                interactable.OnInteractStop(gameObject);
            }
        }
        
        public void StartSliding()
        {
            StopSliding();
            _collider.size = new Vector3(_collider.size.x, _collider.size.y * _slideScale, _collider.size.z);
            _collider.center = _collider.center - new Vector3(0.0f, _collider.size.y * 0.5f, 0.0f); // Set collider's center on the center of the mesh
        }
        
        public void StopSliding()
        {
            _collider.center = _centerStart;
            _collider.size = _sizeStart;
        }
    }
}