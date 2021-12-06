using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AttackSetting;

[RequireComponent(typeof(CharacterController))]
public class PlayerBase : MonoBehaviour
{
    [SerializeField] float _walkSpeed = 1f;
    [SerializeField] float _dashSpeed = 1f;
    [SerializeField] float _avoidSpeed = 1f;
    [SerializeField] float _avoidTime = 1f;
    [SerializeField] float _rotateSpeed;

    AttackSettings _attack;
    Gravity _gravity;

    CharacterController _cc;

    GameObject _mainCm;

    MoveController _move;
    
    void Start()
    {
        _move = GetComponent<MoveController>();
        _attack = GetComponent<AttackSettings>();
        _cc = GetComponent<CharacterController>();
        _mainCm = GameObject.FindGameObjectWithTag("MainCamera");
        _gravity = GetComponent<Gravity>();
        InputSetUp();
    }

    void InputSetUp()
    {
        Inputter.Instance.Inputs.Player.Fire.started += context => Avoid();
        Inputter.Instance.Inputs.Player.Jump.started += context => Jump();
        Inputter.Instance.Inputs.Player.Attack.started += context => Attack();
    }

    void Update()
    {
        Vector3 move = MovePlayer();
        if (move.x != 0 || move.z != 0)
        {
            Rotate();
        }

        Vector3 velocity = Vector3.Scale(move, _gravity.Velocity);
        _cc.Move(velocity * Time.deltaTime);
    }

    void Rotate()
    {
        float rotateY = _mainCm.transform.localRotation.eulerAngles.y;
        Quaternion look = Quaternion.Euler(0, rotateY, 0);
        Quaternion rotate = Quaternion.Slerp
            (transform.localRotation, look, _rotateSpeed * Time.deltaTime);
        
        transform.localRotation = rotate;
    }

    Vector3 MovePlayer()
    {
        Vector2 move = (Vector2)Inputter.GetValue(InputType.PlayerMove);
        _move.GetInput(move);
        Vector3 foward = _mainCm.transform.forward * move.y * _move.GetSpeed;
        Vector3 right = _mainCm.transform.right * move.x * _move.GetSpeed;
        
        return new Vector3(foward.x + right.x, 1, foward.z + right.z);
    }

    void Jump() => _gravity.Force();
    void Attack() => _attack.Request(ActionType.Ground);
    void Avoid() => StartCoroutine(GoAvoid());

    IEnumerator GoAvoid()
    {
        float time = 0;
        while (time < _avoidTime)
        {
            time += Time.deltaTime;
            
            yield return null;
        }
     
    }
}