using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharaBase : MonoBehaviour
{
    [SerializeField] float _gravityScale = 1;
    
    Gravity _selfRB;
    public Gravity Gravity { get => _selfRB; }

    CharacterController _character;
    public CharacterController Character { get => _character; }

    private void Awake()
    {
        _selfRB = gameObject.AddComponent<Gravity>();
        _character = GetComponent<CharacterController>();
        
        SetUpRigid();
    }

    void SetUpRigid()
    {
        _selfRB.SetCharactor = _character;
        _selfRB.SetScale = _gravityScale;
    }
}
