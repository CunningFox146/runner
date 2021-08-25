using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner.Player
{
    public class PlayerAnimation : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        private int _stateHash;

        void Awake()
        {
            _stateHash = Animator.StringToHash("state");
        }

        public void SetState(int state)
        {
            _animator.SetInteger(_stateHash, state);
        }
    }
}