using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float _speed = 3;
    Rigidbody _rb;
    GameObject _camera;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _camera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector2 move = Move(h, v);
        _rb.velocity = new Vector3(move.x, _rb.velocity.y, move.y) * _speed;

    }

    Vector2 Move(float h, float v)
    {
        Vector3 cmF = _camera.transform.forward * v;
        Vector3 cmR = _camera.transform.right * h;

        return new Vector2(cmF.x + cmR.x, cmF.z + cmR.z);
    }
}
