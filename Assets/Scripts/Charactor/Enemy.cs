using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : EnemyBase
{
    void Update()
    {
        Tree.Repeater(this);
    }
}
