using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotController : MonoBehaviour
{
    [SerializeField] Vector3 _pos = Vector3.zero;
    [SerializeField] Bullet _bullet;
    [SerializeField] float _coolTime;
    [SerializeField] float _power;

    GameObject _parent;
    GameObject _mainCm;
    float _currentTime = 0;

    void Start()
    {
        _parent = GameObject.FindGameObjectWithTag("Player");
        _mainCm = GameObject.FindGameObjectWithTag("MainCamera");
    }

    void Update()
    {
        _currentTime += Time.deltaTime;
        if (_currentTime > _coolTime)
        {
            _currentTime = 0;
            SetBullet();
        }

        TrackingParent();
    }

    void TrackingParent()
    {
        Vector3 parentPos = _parent.transform.position;

        float x = parentPos.x + _pos.x;
        float y = parentPos.y + _pos.y;
        float z = parentPos.z + _pos.z;

        transform.position = new Vector3(x, y, z);
    }

    Vector3 RockOnTarget()
    {
        Vector3 set = Vector3.zero;
        
        
        return set;
    }

    void SetBullet()
    {
        Bullet obj = Instantiate(_bullet);
        obj.transform.position = transform.position;
        Vector3 forward = _mainCm.transform.forward;
        
        Vector3 setDir = RockOnTarget();
        if (setDir == Vector3.zero) setDir = new Vector3(forward.x, 0, forward.z);
        obj.SetUp();
        obj.Shot(setDir);
    }
}
