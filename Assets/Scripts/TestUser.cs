using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUser : MonoBehaviour
{
    CharacterController _character;
    NewPhysicsBase _physicsBase;

    void Start()
    {
        _character = GetComponent<CharacterController>();
        _physicsBase = GetComponent<NewPhysicsBase>();
    }

    void Update()
    {
        Vector3 set = Vector3.Scale(new Vector3(0, 1, 0), _physicsBase.Gravity);
        _character.Move(set * Time.deltaTime);
    }
}
