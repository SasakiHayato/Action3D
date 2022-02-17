using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPhysics
{
    /// <summary>
    /// 物理挙動の管理クラス
    /// </summary>

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
        [SerializeField] Vector3 _goundRayDir = Vector3.zero;
        [SerializeField] Vector3 _offSet = Vector3.zero;
        [SerializeField] LayerMask _mask;

        CharacterController _character;
        Gravity _gravity = new Gravity();
        public Gravity Gravity => _gravity;
        JumpSetting _jump;
        
        Vector3 _velocity = Vector3.one;
        public Vector3 GetVelocity => _velocity;

        bool _isJump = false;

        public bool IsGround { get; private set; } = false;

        void Awake()
        {
            _character = GetComponent<CharacterController>();
            _jump = GetComponent<JumpSetting>();
            _gravity.Scale = _gravityScale;
            _gravity.IsReal = _isReal;
        }

        void Update()
        {
            Vector3 tPos = transform.position;
            Vector3 offSetPos = new Vector3(tPos.x + _offSet.x, tPos.y + _offSet.y, tPos.z + _offSet.z);

            if (Physics.Raycast(offSetPos, _goundRayDir, _goundRayDir.magnitude, _mask))
                IsGround = true;
            else 
                IsGround = false;
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

        /// <summary>
        /// ジャンプすることを知らせる
        /// </summary>
        public void SetJump()
        {
            if (_type == PhysicsType.Jump) return;

            _type = PhysicsType.Jump;
            _isJump = true;
        }
    }
}

