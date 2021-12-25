using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitParticle : MonoBehaviour, IPool
{
    Transform _parent;
    ParticleSystem.MainModule _particleMain;

    void Update()
    {
        if (!IsUse) return;
    }

    public void SetUp(Transform parent)
    {
        _parent = parent;
        _particleMain = GetComponent<ParticleSystem>().main;
        _particleMain.stopAction = ParticleSystemStopAction.Callback;
        gameObject.SetActive(false);
    }

    public void Use(Transform target, Quaternion rotate)
    {
        IsUse = true;
        gameObject.SetActive(true);
        transform.parent = null;

        transform.position = target.position;
        transform.rotation = rotate;
    }

    void OnParticleSystemStopped()
    {
        Delete();
    }

    public void Delete()
    {
        IsUse = false;
        transform.SetParent(_parent);
        gameObject.SetActive(false);
    }

    public bool IsUse { get; private set; } = false;
}
