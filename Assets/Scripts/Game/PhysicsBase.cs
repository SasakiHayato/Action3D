using UnityEngine;
using System;

public class PhysicsBase : MonoBehaviour
{
    [Serializable]
    class JumpData
    {
        public int Count;
        public float InitialPower;
        public float Speed;

        int _saveCount;

        public void SstUp()
        {
            _saveCount = Count;
        }

        public bool CheckCount()
        {
            _saveCount--;

            if (_saveCount < 0) return false;
            else return true;
        }

        public void ResetCount() => _saveCount = Count;
    }

    [Serializable]
    public class GravityData
    {
        public Vector3 Ray = Vector3.zero;
        public float RayDistance = 1;
        public LayerMask HitLayer;
        public float Scale = 1;
    }

    public enum ForceType
    {
        Jump,
        Impulse,

        None,
    }

    [SerializeField] Transform _offSet;
    [SerializeField] GravityData _gravityData = new GravityData();
    [SerializeField] JumpData _jumpData = new JumpData();
    
    public Vector3 Gravity { get => _gravity; private set { _gravity = value; } }
    Vector3 _gravity = Vector3.one;

    float _physicsGravity = Physics.gravity.y;
    float _timer;
    bool _isSetGravity;

    public bool IsGround { get; private set; }

    ForceType _forceType = ForceType.None;
    public ForceType CurrentForceType => _forceType;

    Vector3 _forceDir = Vector3.zero;
    float _forcePower = 0;

    float _forceTimer;

    void Start()
    {
        _jumpData.SstUp();
    }

    void Update()
    {
        ForceUpdate();
    }

    void FixedUpdate()
    {
        _forceTimer += Time.fixedDeltaTime;

        if(_isSetGravity) SetGravity();
    }

    public void Force(ForceType type, Vector3 dir = default, float power = 0)
    {
        _forceDir = dir.normalized;
        _forcePower = power;
        _forceType = type;

        SetUpForceEvent(type);
    }

    void SetUpForceEvent(ForceType type)
    {
        switch (type)
        {
            case ForceType.Jump:

                _isSetGravity = false;
                if (!_jumpData.CheckCount()) InitForceParam();
                else _forceTimer = 0;

                break;
            case ForceType.Impulse:

                _isSetGravity = false;
                break;

            case ForceType.None:

                _isSetGravity = true;
                InitGravityParam();

                break;
        }
    }

    void ForceUpdate()
    {
        switch (_forceType)
        {
            case ForceType.Jump:
               
                SetJump();
                break;
            case ForceType.Impulse:

                SetImpulse();
                break;
            case ForceType.None:

                _isSetGravity = true;
                GroundCheck();
                break;
        }
    }

    void SetJump()
    {
        float g = _physicsGravity * -1;
        float t = _forceTimer * _jumpData.Speed;

        float y = _jumpData.InitialPower * t - (g * t * t) / 2;
        _gravity.y = y;

        if (y < 0)
        {
            InitForceParam();
            InitGravityParam();
            _forceType = ForceType.None;
        }
    }

    void SetImpulse()
    {
        float power = _forcePower - _forceTimer * _physicsGravity * -1;
        _gravity = _forceDir * power;

        if (power < 0)
        {
            InitForceParam();
            InitGravityParam();
            _forceType = ForceType.None;
        }
    }

    void SetGravity()
    {
        _timer += Time.fixedDeltaTime;
        _gravity.y = _timer * _physicsGravity * _gravityData.Scale;
    }

    void GroundCheck()
    {
        Vector3 dir = _gravityData.Ray.normalized;
        float distance = _gravityData.RayDistance;
        
        if (Physics.Raycast(_offSet.position, dir, distance, _gravityData.HitLayer))
        {
            InitGravityParam();
            _jumpData.ResetCount();

            IsGround = true;
        }
        else
        {
            IsGround = false;
        }
    }

    void InitGravityParam()
    {
        _timer = 0;
        _gravity = Vector3.one;
        _gravity.y = -1;
    }

    void InitForceParam()
    {
        _forceDir = Vector3.zero;
        _forcePower = 0;
        _forceType = ForceType.None;

        _forceTimer = 0;
    }
}
