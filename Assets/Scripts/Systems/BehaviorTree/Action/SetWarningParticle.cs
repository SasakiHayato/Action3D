using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourAI;
using DG.Tweening;

public class SetWarningParticle : IAction
{
    [SerializeField] float _setNextActionTime = 0.2f;
    [SerializeField] Vector3 _offSet = new Vector3(4,4,4);

    float _timer;
    bool _isCall = false;

    public GameObject Target { get; set; }

    public void SetUp()
    {
        _timer = 0;
        _isCall = false;
    }

    public bool Execute()
    {
        if (!_isCall)
        {
            _isCall = true;
            Debug.Log("ParticleCall");
            GameObject obj = Object.Instantiate((GameObject)Resources.Load("WarningParticle"));
            obj.transform.position = Target.GetComponentInChildren<TargetCorrector>().transform.position;
            obj.transform.DOScale(_offSet, _setNextActionTime / 2)
                .OnComplete(() =>
                {
                    obj.transform.DOKill();
                    Object.Destroy(obj);
                });
        }

        _timer += Time.deltaTime;

        if (_timer > _setNextActionTime) return true;
        else return false;
    }
}
