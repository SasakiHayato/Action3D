using UnityEngine;
using BehaviorAI;

[RequireComponent(typeof(BehaviorTree))]
public abstract class EnemyBase : CharaBase, IBehavior
{
    [SerializeField] float _speed;
    [SerializeField] float _knockBackPower;
    [SerializeField] float _knockBackTime;
    [SerializeField] BehaviorTree _tree;

    protected BehaviorTree Tree { get => _tree; }
    protected float GetKnockBackPower => _knockBackPower;
    public float Speed { get => _speed; set { _speed = value; } }

    public Vector3 MoveDir { protected get; set; } = new Vector3(0 ,1, 0);
    float _timer = 0;

    public GameObject SetTarget() => gameObject;
    public void Call(IAction action) => action.Execute();

    public virtual void Dead(GameObject target)
    {
        ParticleUser particle = FieldManager.Instance.GetDeadParticle.Respons();
        particle.Use(target.transform);
        Sounds.SoundMaster.Request(null, "EnemyDead", 2);
        Destroy(target);
    }

    public void SetKnockBack(ref bool check)
    {
        if (check)
        {
            _timer += Time.deltaTime;
            if (_timer > _knockBackTime) check = false;
            else check = true;
            Character.Move(MoveDir * Time.deltaTime * GetKnockBackPower);
            return;
        }
        else
        {
            _timer = 0;
        }
    }
    
    public abstract void KnockBack(Vector3 dir);
}
