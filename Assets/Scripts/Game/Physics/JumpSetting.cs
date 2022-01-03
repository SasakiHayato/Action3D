using UnityEngine;

namespace ObjectPhysics
{
    public class JumpSetting : MonoBehaviour
    {
        [SerializeField] int _count = 1;
        [SerializeField] float _initialVelocity;
        [SerializeField] float _speed = 1;

        float _timer = 0;
        int _saveCount;

        void Start()
        {
            _saveCount = _count;
        }

        public float SetVelocity(ref bool isJump, out PhysicsBase.PhysicsType type)
        {
            if (isJump)
            {
                isJump = false;
                _count--;
            }

            if (_count < 0)
            {
                type = PhysicsBase.PhysicsType.IsGravity;
                _timer = 0;

                return 0;
            }

            float g = Physics.gravity.y * -1;
            _timer += Time.fixedDeltaTime * _speed;

            float y = _initialVelocity * _timer - (g * _timer * _timer) / 2;

            if (y < 0)
            {
                type = PhysicsBase.PhysicsType.IsGravity;
                _timer = 0;
            }
            else type = PhysicsBase.PhysicsType.Jump;

            return y;
        }

        public void ResetCount()
        {
            _timer = 0;
            _count = _saveCount;
        }
    }
}
