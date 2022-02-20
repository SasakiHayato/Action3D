using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �O���[�o���X�y�[�X�Ŏg����C���^�[�t�F�[�X�B
/// </summary>

public interface IDamage
{
    void GetDamage(int damage, AttackType type);
}

public interface IFieldEnemy
{
    int GroupID { get; set; }
    EnemysData.EnemyData EnemyData { get; set; }
    GameObject Target { get; set; }
}

public interface IPool
{
    bool IsUse { get; }
    void SetUp(Transform parent);
    void Delete();
}

public interface IAttackCollision
{
    string TagName { get; }
    GameObject Target { get; }
    Collider Collider { get; }
}