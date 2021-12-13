using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharaBase : MonoBehaviour
{
    Gravity _gravity;
    public Gravity Gravity { get => _gravity; }

    CharacterController _character;
    public CharacterController Character { get => _character; }

    private void Awake()
    {
        _gravity = gameObject.AddComponent<Gravity>();
        _character = GetComponent<CharacterController>();
    }
}
