using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ObjectPhysics;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PhysicsBase))]
public class CharaBase : MonoBehaviour
{
    [SerializeField] int _hp;
    [SerializeField] int _power;
    [SerializeField] float _speed;

    public int HP { get => _hp; protected set { _hp = value; } }
    public int Power { get => _power; protected set { _power = value; } }
    public float Speed { get => _speed; protected set { _speed = value; } }

    PhysicsBase _phsicsBase;
    public PhysicsBase PhsicsBase { get => _phsicsBase; }

    CharacterController _character;
    public CharacterController Character { get => _character; }

    private void Awake()
    {
        _phsicsBase = gameObject.AddComponent<PhysicsBase>();
        _character = GetComponent<CharacterController>();
    }

    public void SetParam(int hp, int power, float speed, int level)
    {
        if (level == 1)
        {
            HP = hp;
            Power = power;
        }
        else
        {
            float add = ((float)level / 10) - 0.1f;
            Debug.Log(add);
            HP = hp + (int)(hp * add);
            Power = power * (int)(power * add);
        }

        Speed = speed;
    }
}
