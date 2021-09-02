using Runner.Player;
using UnityEngine;

namespace Runner.Managers
{
    public class CameraManager : Singleton<CameraManager>
    {
        [SerializeField] private PlayerController _player;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private float _moveSpeed;

        public bool isFollowing = true;

        void Start()
        {
            transform.position = _offset + _player.transform.position;
        }

        // Update is called once per frame
        void LateUpdate()
        {
            if (isFollowing)
            {
                transform.position = Vector3.Lerp(transform.position, _offset + _player.transform.position,
                    Time.deltaTime * _moveSpeed);
            }
        }
    }
}