using UnityEngine;

public class CmManager : MonoBehaviour
{
    [SerializeField] Transform _user;
    [SerializeField] Transform _targetCm;
    [SerializeField] Vector3 _offSetPos;
    [SerializeField] float _sensitivityX = 0.5f;
    [SerializeField] float _sensitivityY = 0.5f;
    [SerializeField] float _viewDelay;
    [SerializeField] float _dead = 0.1f;

    float _rotateTimer;
    float _horizontalAngle;
    float _verticleAngle;

    float _dist;
    float _savePosY;
    Vector2 _saveHorizontalPos;

    void Start()
    {
        Inputter inputter = new Inputter();
        Inputter.SetInstance(inputter, inputter);
        Inputter.Instance.Load();

        _targetCm.position = _user.position + _offSetPos;
        _dist = Vector3.Distance(_user.position, _targetCm.position);

        _saveHorizontalPos = new Vector2(_targetCm.position.x, _targetCm.position.z);
        _horizontalAngle = Mathf.Atan2(_saveHorizontalPos.y, _saveHorizontalPos.x) * Mathf.Rad2Deg - 90;

        _savePosY = _targetCm.position.y;
    }

    void Update()
    {
        Vector2 input = (Vector2)Inputter.GetValue(InputType.CmMove);
        
        Vector2 pos = HorizontalPos(input.normalized.x);
        float y = VerticlePos(input.normalized.y);

        _targetCm.position = new Vector3(pos.x, y, pos.y);

        View();
    }

    Vector2 HorizontalPos(float x)
    {
        float s = float.Parse(x.ToString("0.0"));
        if (Mathf.Abs(s * s) < _dead) return _saveHorizontalPos;

        if (x > 0) _horizontalAngle++;
        else if (x < 0) _horizontalAngle--;

        float angle = _horizontalAngle * _sensitivityX;

        if (angle >= 360 || angle <= -360)
        {
            _horizontalAngle = 0;
            angle = 0;
        }
        
        float rad = angle * Mathf.Deg2Rad;
        Vector3 pos = new Vector3(Mathf.Cos(rad), 0, Mathf.Sin(rad)) * _dist;

        Vector3 setPos = pos + _user.position;

        _saveHorizontalPos = new Vector2(setPos.x, setPos.z);

        return _saveHorizontalPos;
    }

    float VerticlePos(float y)
    {
        float s = float.Parse(y.ToString("0.0"));
        if (Mathf.Abs(s * s) <  _dead) return _savePosY;

        if (y > 0) _verticleAngle++;
        else if (y < 0) _verticleAngle--;

        float angle = _verticleAngle * _sensitivityY * -1;

        float rad = angle * Mathf.Deg2Rad;
        _savePosY = _offSetPos.y + Mathf.Sin(rad) * _dist;
        return _savePosY;
    }

    void View()
    {
        _rotateTimer += Time.deltaTime / _viewDelay;

        Vector3 dir = _user.position - _targetCm.position;
        Quaternion q = Quaternion.LookRotation(dir.normalized);
        _targetCm.rotation = Quaternion.Lerp(_targetCm.rotation, q, _rotateTimer);

        if (_targetCm.rotation == q) _rotateTimer = 0;
    }
}
