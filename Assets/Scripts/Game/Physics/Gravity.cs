using UnityEngine;

namespace ObjectPhysics
{
    public class Gravity
    {
        public float Scale { get; set; }
        public bool IsReal { get; set; }

        float _timer; 

        public float IsGravity(bool isGround)
        {
            if (isGround)
            {
                _timer = 0;
                return 0;
            }

            if (IsReal) _timer += Time.fixedUnscaledDeltaTime;
            else _timer += Time.fixedDeltaTime;

            return _timer * Physics.gravity.y * Scale;
        }

        public void ResetTimer() => _timer = 0;
    }
}
