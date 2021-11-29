using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    [SerializeField] GameObject _parent;
    [SerializeField] float _radius;
    [SerializeField] float _yPos;
    [SerializeField] float _moveRate;

    float _hAngle = 0;
    float _vAngle = 0;

    void Update()
    {
        Vector3 parent = _parent.transform.position;

        float setX = parent.x + Horizontal().x + Vertical().x;
        float setY = parent.y + _yPos + Vertical().y;
        float setZ = parent.z + Horizontal().y;

        transform.position = new Vector3(setX, setY, setZ);

        transform.LookAt(_parent.transform);
    }

    Vector2 Horizontal()
    {
        if (Mathf.Abs(_hAngle) / _moveRate >= 360) _hAngle = 0;

        float rad = (_hAngle / _moveRate) * (Mathf.PI / 180);

        if(Input.GetKey(KeyCode.L)) _hAngle++;
        else if (Input.GetKey(KeyCode.J)) _hAngle--;

        return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * _radius;
    }

    Vector2 Vertical()
    {
        if (Mathf.Abs(_vAngle) / _moveRate >= 360) _vAngle = 0;

        float rad = (_vAngle / _moveRate) * (Mathf.PI / 180);

        if (Input.GetKey(KeyCode.I)) _vAngle++;
        else if (Input.GetKey(KeyCode.K)) _vAngle--;

        return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * _radius;
    }
}
