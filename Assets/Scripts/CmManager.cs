using UnityEngine;

public class CmManager : MonoBehaviour
{
    [SerializeField] Transform _user;
    [SerializeField] Transform _targetCm;
    [SerializeField] Vector3 _offSetPos;
    [SerializeField] float _sensitivity = 0.5f;
    [SerializeField] float _rotateDelay;

    float _rotateTimer;
    float _angle;

    float _dist;

    void Start()
    {
        Inputter inputter = new Inputter();
        Inputter.SetInstance(inputter, inputter);
        Inputter.Instance.Load();

        _targetCm.position = _user.position + _offSetPos;
        _dist = Vector3.Distance(_user.position, _targetCm.position);
        Debug.Log(_dist);
    }

    void Update()
    {
        RotateX();
        View();
    }

    void RotateX()
    {
        Vector2 input = (Vector2)Inputter.GetValue(InputType.CmMove);
        
        if (input.x > 0) _angle++;
        else if (input.x < 0) _angle--;
        
        float angle = _angle * _sensitivity * -1;

        if (angle >= 360 || angle <= -360) angle = 0;
        Debug.Log(angle);

        float rad = angle * Mathf.Deg2Rad;
        Vector3 pos = new Vector3(Mathf.Sin(rad), 0, Mathf.Cos(rad)) * _dist;

        Vector3 setPos = pos + _user.position;
        setPos.y = _offSetPos.y;
        _targetCm.position = setPos;
    }

    void RotateY()
    {

    }

    void View()
    {
        _rotateTimer += Time.deltaTime / _rotateDelay;

        Vector3 dir = _user.position - _targetCm.position;
        Quaternion q = Quaternion.LookRotation(dir.normalized);
        _targetCm.rotation = Quaternion.Lerp(_targetCm.rotation, q, _rotateTimer);

        if (_targetCm.rotation == q) _rotateTimer = 0;
    }
}
