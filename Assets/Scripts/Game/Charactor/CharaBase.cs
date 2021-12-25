using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ObjectPhysics;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PhysicsBase))]
public class CharaBase : MonoBehaviour
{
    PhysicsBase _phsicsBase;
    public PhysicsBase PhsicsBase { get => _phsicsBase; }

    CharacterController _character;
    public CharacterController Character { get => _character; }

    private void Awake()
    {
        _phsicsBase = gameObject.AddComponent<PhysicsBase>();
        _character = GetComponent<CharacterController>();
    }
}
