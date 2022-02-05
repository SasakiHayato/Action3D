using UnityEngine;
using BehaviourAI;

public class WaitTime : IAction
{
    [SerializeField] float _minWaitTime;
    [SerializeField] float _maxWaitTime;
    [SerializeField] bool _setRandom;

    float _timer;
    float _waitTime;

    public GameObject Target { get; set; }

    public void SetUp() 
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
}
