using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamage
{
    void GetDamage(int damage);
}

public interface IFieldEnemy
{
    int GroupID { get; set; }
    bool IEnemyDead { get; set; }
}

