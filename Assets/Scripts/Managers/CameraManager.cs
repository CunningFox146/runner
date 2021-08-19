using System.Collections;
using System.Collections.Generic;
using Runner.Player;
using UnityEngine;

namespace Runner.Managers
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private PlayerController _player;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private float _moveSpeed;

        void Start()
        {
            transform.position = _offset + _player.transform.position;
        }

        // Update is called once per frame
        void LateUpdate()
        {
            transform.position = Vector3.Lerp(transform.position, _offset + _player.transform.position,
                Time.deltaTime * _moveSpeed);
        }
    }
}