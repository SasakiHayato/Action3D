using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using EnemysData;

/// <summary>
/// Enemyの基底クラス
/// </summary>

[RequireComponent(typeof(TreeManager))]
public abstract class EnemyBase : CharaBase
{
    public enum State
    {
        BehaviorTree,
        KnockBack,
    }

    [SerializeField] TreeManager _tree;
    [SerializeField] List<EnemyConditionalData> _enemyConditionals;

    protected TreeManager Tree { get => _tree; set { _tree = value; } }
    public Vector3 MoveDir { protected get; set; } = new Vector3(0 ,1, 0);
    
    public List<EnemyConditionalData> GetEnemyConditionalDatas => _enemyConditionals;

    // IFieldEnemy
    public int GroupID { get; set; }
    public GameObject Target { get; set; }
    public EnemyData EnemyData { get; set; }
    public bool CanMove { get; set; } = false;

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
}


// Note. ビヘイビアーツリー関連。一度だけTrueを返すようにするデータクラス
[System.Serializable]
public class EnemyConditionalData
{
    public int ID;
    public bool Check { get; private set; } = false;
    public void SetBool(bool set) => Check = set;
}
