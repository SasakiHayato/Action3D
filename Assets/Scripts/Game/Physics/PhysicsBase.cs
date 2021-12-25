using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPhysics
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(JumpSetting))]
    public class PhysicsBase : MonoBehaviour
    {
        public enum PhysicsType
        {
            IsGravity,
            Jump,
        }

        PhysicsType _type = PhysicsType.IsGravity;

        [SerializeField] float _gravityScale = 1;
        [SerializeField] bool _isReal = false;
        
        CharacterController _character;
        Gravity _gravity = new Gravity();
        JumpSetting _jump;
        
        Vector3 _velocity = Vector3.one;
        public Vector3 GetVelocity => _velocity;

        bool _isJump = false;

        void Awake()
        {
            _character = GetComponent<CharacterController>();
            _jump = GetComponent<JumpSetting>();
            _gravity.Scale = _gravityScale;
            _gravity.IsReal = _isReal;
        }

        void FixedUpdate()
        {
            if (_character.isGrounded) _jump.ResetCount();

            switch (_type)
            {
                case PhysicsType.IsGravity:
                    _velocity.y = _gravity.IsGravity(_character.isGrounded);
                    break;
                case PhysicsType.Jump:
                    _velocity.y = _jump.SetVelocity(ref _isJump, out _type);
                    _gravity.ResetTimer();
                    break;
            }
        }

        public void SetJump()
        {
            if (_type == PhysicsType.Jump) return;

            _type = PhysicsType.Jump;
            _isJump = true;
        }
    }
}

