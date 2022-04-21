using UnityEngine;
using BehaviourTree;
using DG.Tweening;

/// <summary>
/// パーティクルをだすAI行動
/// </summary>

public class SetParticleEffect : IAction
{
    [SerializeField] string _particleName;
    [SerializeField] float _setNextActionTime = 0.2f;
    [SerializeField] Vector3 _offSet = new Vector3(4,4,4);

    float _timer;
    bool _isCall = false;

    GameObject _user;

    public void SetUp(GameObject user)
    {
        _user = user;
        _timer = 0;
        _isCall = false;
    }

    public bool Execute()
    {
        if (!_isCall)
        {
            _isCall = true;
            GameObject obj = Object.Instantiate((GameObject)Resources.Load("Particle/"+_particleName));
            obj.transform.position = _user.GetComponent<CharaBase>().OffSetPosObj.transform.position;
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

    public void InitParam()
    {
        _isCall = false;
        _timer = 0;
    }
}
