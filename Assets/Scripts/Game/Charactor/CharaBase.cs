using UnityEngine;
using ObjectPhysics;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PhysicsBase))]
public class CharaBase : MonoBehaviour
{
    [SerializeField] int _hp;
    [SerializeField] int _power;
    [SerializeField] float _speed;
    [SerializeField] int _level;

    public int HP { get => _hp; protected set { _hp = value; } }
    public int MaxHP { get; private set; } = 0;
    public int Power { get => _power; protected set { _power = value; } }
    public float Speed { get => _speed; protected set { _speed = value; } }
    public int Level { get => _level; protected set { _level = value; } }

    PhysicsBase _phsicsBase;
    public PhysicsBase PhsicsBase { get => _phsicsBase; }

    CharacterController _character;
    public CharacterController Character { get => _character; }

    const float MaxAddCount = 0.4f;

    private void Awake()
    {
        _phsicsBase = gameObject.AddComponent<PhysicsBase>();
        _character = GetComponent<CharacterController>();
    }

    virtual public void SetParam(int hp, int power, float speed, int level)
    {
        if (level == 1)
        {
            HP = hp;
            Power = power;
        }
        else
        {
            float add = ((float)level / 10) * 2 - 0.1f;
            if (add > MaxAddCount) add = MaxAddCount;
            HP = hp + (int)(hp * add);
            Power = power + (int)(power * add);
        }

        MaxHP = HP;
        Speed = speed;
        Level = level;
    }
}
