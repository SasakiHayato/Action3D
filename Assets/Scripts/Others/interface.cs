using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

