using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.Player
{
    public class PlayerCollider : MonoBehaviour
    {
        [Range(0f, 1f)]
        [SerializeField] private float _slideScale = 0.5f;
        [SerializeField] private float _rayLength = 10f;

        private BoxCollider _collider;
        private Vector3 _sizeStart;
        private Vector3 _centerStart;
        private Vector3 _rayStart;
        private float _groundDist = 0f;
        private float _groundOffset = 1f;

        public float GroundYOffset => -_groundDist + _groundOffset;

        void Awake()
        {
            _collider = GetComponent<BoxCollider>();
        }

        void Start()
        {
            _centerStart = _collider.center;
            _sizeStart = _collider.size;
            
            var min = _collider.center - _collider.size * 0.5f;
            var max = _collider.center + _collider.size * 0.5f;

            _groundOffset = max.y * 2f;
            _rayStart = new Vector3(min.x + max.x, max.y, max.z);
        }

        void Update()
        {
            var point = transform.TransformPoint(_rayStart); // The front top center side of box collider
            
            if (Physics.Raycast(point, -transform.up, out RaycastHit ray, _rayLength, 1 << 6))
            {
                _groundDist = point.y - ray.point.y;
                Debug.Log(GroundYOffset);
                Debug.DrawLine(point, ray.point, Color.red);
            }
            //Debug.DrawLine(point, point + new Vector3(0f, -10f, 0f), Color.red);
        }

        public void StartSliding()
        {
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