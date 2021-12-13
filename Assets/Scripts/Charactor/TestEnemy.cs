using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour, IDamage
{
    public float AddDamage()
    {
        return 0;
    }

    public void GetDamage(float damage)
    {
        Debug.Log($"Get {damage} Damage. Obj{gameObject.name}");
    }
}
