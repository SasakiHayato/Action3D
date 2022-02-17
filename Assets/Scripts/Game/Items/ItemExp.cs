using UnityEngine;

/// <summary>
/// 経験値アイテムのクラス
/// </summary>

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

    /// <summary>
    /// Itemを飛び散らせる
    /// </summary>
    /// <param name="position">爆発させる中心点</param>
    public void Force(Vector3 position)
    {
        GetComponent<Rigidbody>().AddExplosionForce(2, position, 10, 2, ForceMode.Impulse);
    }
}
