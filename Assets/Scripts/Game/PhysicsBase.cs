using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// ï®óùãììÆÇÃä«óùÉNÉâÉX
/// </summary>

public class PhysicsBase : MonoBehaviour
{
    [Serializable]
    public class JumpData
    {
        public List<JumpParam> _jumpParams = new List<JumpParam>();

        [Serializable]
        public class JumpParam
        {
            public float InitialPower;
            public float Speed;
        }

        public int Count => _jumpParams.Count;
        public int CurrntCount => _count;
        public float InitialPower => _jumpParams[_count - 1].InitialPower;
        public float Speed => _jumpParams[_count - 1].Speed;

        int _count = 0;

        public bool CheckCount()
        {
            _count++;

            if (_count > Count) return false;
            else return true;
        }

        public void ResetCount() => _count = 0;
    }

    [Serializable]
    public class GravityData
    {
        public Vector3 Ray = Vector3.zero;
        public float RayDistance = 1;
        public LayerMask HitLayer;
        public float Scale = 1;
        public float Mass;
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

    public JumpData GetJumpData => _jumpData;

    public Vector3 Gravity { get => _gravity; private set { _gravity = value; } }
    Vector3 _gravity = Vector3.one;

    float _physicsGravity = Physics.gravity.y;
    float _timer;
    bool _isSetGravity = true;

    public bool IsGround { get; private set; }
    public float ImpulsePower { get; private set; }

    ForceType _forceType = ForceType.None;
    public ForceType CurrentForceType => _forceType;

    Vector3 _forceDir = Vector3.zero;
    float _forcePower = 0;

    float _forceTimer;

    void Update()
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
        _forceTimer = 0;

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

                InitGravityParam();
                
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
        }
    }

    void SetImpulse()
    {
        float mass = _gravityData.Mass;
        if (mass <= 0) mass = 1;
       
        float power = (_forcePower - _forceTimer * _physicsGravity * -1) / mass;
        
        ImpulsePower = power;
        _gravity = _forceDir * power;

        if (power < 0)
        {
            InitForceParam();
            InitGravityParam();
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

        ImpulsePower = 0;
    }
}
