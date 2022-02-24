using UnityEngine;
using BehaviourTree;

public class WaitTime : IAction
{
    [SerializeField] float _minWaitTime;
    [SerializeField] float _maxWaitTime;
    [SerializeField] bool _setRandom;

    float _timer;
    float _waitTime;

    public void SetUp(GameObject user) 
    {
        if (_setRandom) _waitTime = Random.Range(_minWaitTime, _maxWaitTime);
        else _waitTime = _maxWaitTime;
    }

    public bool Execute()
    {
        _timer += Time.deltaTime;

        if (_timer > _waitTime)
        {
            _timer = 0;
            return true;
        }

        return false;
    }

    public void InitParam()
    {

    }
}
