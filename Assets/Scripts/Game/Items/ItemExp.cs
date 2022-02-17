using UnityEngine;

/// <summary>
/// �o���l�A�C�e���̃N���X
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
    /// Item���юU�点��
    /// </summary>
    /// <param name="position">���������钆�S�_</param>
    public void Force(Vector3 position)
    {
        GetComponent<Rigidbody>().AddExplosionForce(2, position, 10, 2, ForceMode.Impulse);
    }
}
