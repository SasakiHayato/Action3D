using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemExp : ItemBase
{
    int _exp;
    int _level;

    public override void SetOtherData(object[] datas)
    {
        _exp = (int)datas[0];
        _level = (int)datas[1];
        
        base.SetOtherData(datas);
    }

    public override void Use()
    {
        GameManager.Instance.GetExp(_exp, _level);
        base.Use();
    }

    public void Force(Vector3 position)
    {
        GetComponent<Rigidbody>().AddExplosionForce(2, position, 10, 2, ForceMode.Impulse);
    }
}
