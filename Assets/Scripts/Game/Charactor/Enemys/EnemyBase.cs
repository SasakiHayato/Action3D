using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorAI;

[RequireComponent(typeof(BehaviorTree))]
public abstract class EnemyBase : CharaBase, IBehavior
{
    [SerializeField] float _knockBackPower;
    [SerializeField] float _knockBackTime;
    [SerializeField] BehaviorTree _tree;

    protected BehaviorTree Tree { get => _tree; }
    protected float GetKnockBackTime => _knockBackTime;
    protected float GetKnockBackPower => _knockBackPower;

    public GameObject SetTarget() => gameObject;
    public void Call(IAction action) => action.Execute();

    public virtual void Dead(GameObject target)
    {
        ParticleUser particle = FieldManager.Instance.GetDeadParticle.Respons();
        particle.Use(target.transform);
        Destroy(target);
    }
    
    public abstract void KnockBack(Vector3 dir);
}
