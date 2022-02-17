using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : UIWindowParent.UIWindowChild
{
    [SerializeField] GameObject _damageTextPrefab;
    [SerializeField] Color _playerTextColor;
    [SerializeField] Color _enemyTextColor;

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
        int damage = (int)data[0];
        GameObject target = (GameObject)data[1];
        Color color = (ColorType)data[2] == ColorType.Player ? _playerTextColor : _enemyTextColor;

        StateMachine state = target.GetComponent<StateMachine>();
        if (state == null || state.GetCurrentState != StateMachine.StateType.Avoid)
        {
            _textPool.Respons().Use(damage, target.transform, color);
        }
    }
}
