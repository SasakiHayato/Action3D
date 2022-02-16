using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : UIWindowParent.UIWindowChild
{
    [SerializeField] GameObject _damageTextPrefab;
    ObjectPool<DamageTextSetter> _textPool = new ObjectPool<DamageTextSetter>();

    public override void SetUp()
    {
        _textPool.SetUp(_damageTextPrefab.GetComponent<DamageTextSetter>(), ParentPanel.transform, 10);
        _damageTextPrefab.SetActive(false);
    }

    public override void UpDate()
    {
        
    }

    public override void CallBack(object[] data)
    {
        
    }
}
