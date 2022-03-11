using UnityEngine;

/// <summary>
/// DamageÇéÛÇØÇΩç€ÇÃTextï\é¶
/// </summary>

public class DamageText : ChildrenUI
{
    [SerializeField] Color _playerTextColor;
    [SerializeField] Color _enemyTextColor;

    ObjectPool<DamageTextSetter> _textPool = new ObjectPool<DamageTextSetter>();
    
    public override void SetUp()
    {
        _textPool.SetUp(gameObject.GetComponent<DamageTextSetter>(), ParentPanel.transform, 10);
        gameObject.SetActive(false);
    }

    public override void CallBack(object[] data)
    {
        int damage = (int)data[0];
        GameObject target = (GameObject)data[1];
        Color color = (ColorType)data[2] == ColorType.Player ? _enemyTextColor : _playerTextColor;

        CharaBase charaBase = target.GetComponent<CharaBase>();
        if (charaBase == null || charaBase.BaseState.CurrentStateType != Player.State.Avoid.ToString())
        {
            _textPool.Respons().Use(damage, target.transform, color);
        }
    }
}
