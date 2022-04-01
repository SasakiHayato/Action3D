using UnityEngine;
using StateMachine;
using System.Collections.Generic;

public class CmManager : MonoBehaviour
{
    public class CmData
    {
        public class Data
        {
            public State State;
            public Vector3 Pos;
        }

        private static CmData _instance = new CmData();
        public static CmData Instance => _instance;

        public State CurrentState;
        public State NextState;

        public Transform User;
        public Transform NextTarget;

        public Vector3 Position;
        public Vector3 TransitionPos;

        List<Data> _datas = new List<Data>();

        public void AddData(State state, Vector3 pos)
        {
            Data data = new Data();
            data.State = state;
            data.Pos = pos;

            _datas.Add(data);
        }

        public Data GetData(State state)
        {
            foreach (Data data in _datas)
            {
                if (state == data.State) return data;
            }

            return null;
        }
    }

    public enum State
    {
        Normal,
        Lockon,
        Shake,
        Transition,
    }

    [SerializeField] Transform _user;
    [SerializeField] LayerMask _collsionLayer;
    [SerializeField] float _zoomRate;
    [SerializeField] float _deadZoomDist;
    [SerializeField] StateManager _state = new StateManager();

    Transform _cmPoint;
    Vector3 _zoomPos = Vector3.zero;

    float _distRate;

    void Start()
    {
        CmData.Instance.User = _user;

        _state.SetUp(gameObject)
            .AddState(State.Normal, "Normal")
            .AddState(State.Lockon, "Lockon")
            .AddState(State.Transition, "Transition")
            .AddState(State.Shake, "Shake")
            .RunRequest(State.Normal);

        CreatePoint();

        _distRate = Vector3.Distance(_user.position, transform.position);
    }

    void CreatePoint()
    {
        GameObject obj = new GameObject("CamPoint");
        obj.transform.position = CmData.Instance.Position;
        _cmPoint = obj.transform;
    }

    void Update()
    {
        if (GameManager.Instance.OptionState == GameManager.Option.Close)
        {
            _state.Update();
        }

        CollisionObstacle();
        transform.position = CmData.Instance.Position + _zoomPos;
        _cmPoint.position = CmData.Instance.Position.normalized * _distRate;
    }

    public void RequestShakeCm()
    {
        _state.ChangeState(State.Shake);
    }

    void CollisionObstacle()
    {
        float dist = Vector3.Distance(_user.position, _cmPoint.position);

        RaycastHit hit;
        bool isHit = Physics.Raycast(_user.position, _cmPoint.position, out hit, dist, _collsionLayer);
        
        if (isHit)
        {
            Vector3 setPps = ZoomPosHrizontal(hit.distance);
            setPps.y = ZoomPosVitical(hit);
            
            _zoomPos = setPps;
        }
        else
        {
            _zoomPos = Vector3.zero;
        }
    }

    Vector3 ZoomPosHrizontal(float hitDist)
    {
        if (hitDist < _deadZoomDist) hitDist = _deadZoomDist;

        float rate = Mathf.Lerp(0, _zoomRate, (_distRate / hitDist) / 10);
        Vector3 setPos = transform.forward * rate * 2;
        setPos.y = 0;
        
        return setPos;
    }

    float ZoomPosVitical(RaycastHit hit)
    {
        float result = 0;
        
        if (_user.position.y >= hit.transform.position.y)
        {
            result = Mathf.Abs(hit.transform.position.y + _user.position.y);
        }
        
        return result ;
    }
}
