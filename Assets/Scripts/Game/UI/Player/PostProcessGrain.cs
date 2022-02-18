using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

/// <summary>
/// PostProcess,　グレインの表示
/// </summary>

public class PostProcessGrain : UIWindowParent.UIWindowChild
{
    [SerializeField] PostProcessProfile _processProfile;
    [SerializeField] float _effectHpParcent;

    CharaBase _charaBase;
    Grain _grain;

    public override void SetUp()
    {
        _charaBase = Object.FindObjectOfType<Player>().GetComponent<CharaBase>();
        _grain = _processProfile.GetSetting<Grain>();
    }

    public override void UpDate()
    {
        float effect = Mathf.Lerp(0, _charaBase.MaxHP, _effectHpParcent);
        
        if (effect > _charaBase.HP) _grain.active = true;
        else _grain.active = false;
    }

    public override void CallBack(object[] data) { }
}
