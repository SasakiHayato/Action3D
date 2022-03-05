using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using EnemysData;

[RequireComponent(typeof(TreeManager))]
public abstract class EnemyBase : CharaBase
{
    [SerializeField] float _knockBackPower;
    [SerializeField] float _knockBackTime;
    [SerializeField] TreeManager _tree;
    [SerializeField] List<EnemyConditionalData> _enemyConditionals;

    protected TreeManager Tree { get => _tree; set { _tree = value; } }
    protected float GetKnockBackPower => _knockBackPower;

    public Vector3 MoveDir { protected get; set; } = new Vector3(0 ,1, 0);
    float _timer = 0;

    public List<EnemyConditionalData> GetEnemyConditionalDatas => _enemyConditionals;

    // IFieldEnemy
    public int GroupID { get; set; }
    public GameObject Target { get; set; }
    public EnemyData EnemyData { get; set; }

    protected virtual void Dead(GameObject target)
    {
        ParticleUser particle = FieldManager.Instance.GetDeadParticle.Respons();
        particle.Use(target.transform);
        Sounds.SoundMaster.PlayRequest(null, "EnemyDead", Sounds.SEDataBase.DataType.Enemys);

        IFieldEnemy iEnemy = target.GetComponent<IFieldEnemy>();

        if (iEnemy != null)
        {
            FieldManager.Instance.FieldData.Delete(GroupID, iEnemy);
            FieldManager.RequestExprosion(iEnemy.EnemyData, target.transform.position, Level);
        }

        IAttackCollision attackCollision = target.GetComponentInChildren<IAttackCollision>();
        
        if (attackCollision != null)
        {
            GameManager.Instance.RemoveAttackCollsion(attackCollision);
        }
        
        Destroy(target);
    }

    protected void SetKnockBack(ref bool check)
    {
        if (check)
        {
            _timer += Time.deltaTime;
            if (_timer > _knockBackTime)
            {
                check = false;
                PhsicsBase.Gravity.ResetTimer();
            }
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

[System.Serializable]
public class EnemyConditionalData
{
    public int ID;
    public bool Check { get; private set; } = false;
    public void SetBool(bool set) => Check = set;
}
