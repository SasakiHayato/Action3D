using UnityEngine;
using System;

public class NewPhysicsBase : MonoBehaviour
{
    [Serializable]
    class JumpData
    {
        public int Count;
        public float InitialPower;
        public float Speed;

        int _count;

        public void SstUp()
        {
            _count = Count;
        }

        public bool CheckCount()
        {
            _count--;

            if (_count < 0)
            {
                _count = Count;
                return false;
            }
            else
            {
                return true;
            }
        }

        public void ResetCount() => _count = Count;
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

    ForceType _forceType = ForceType.None;
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

                if (!_jumpData.CheckCount())
                {
                    InitForceParam();
                }
                else
                {
                    _forceTimer = 0;
                }

                break;
            case ForceType.Impulse:
                break;
        }
    }

    void ForceUpdate()
    {
        switch (_forceType)
        {
            case ForceType.Jump:
                Debug.Log("Jump");
                SetJump();
                break;
            case ForceType.Impulse:


                break;
            case ForceType.None:

                Debug.Log("None");
                GroundCheck();
                InitForceParam();
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
            _isSetGravity = false;
        }
        else
        {
            _isSetGravity = true;
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
