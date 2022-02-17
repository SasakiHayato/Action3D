using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// グローバルスペースで使われるインターフェース達
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

