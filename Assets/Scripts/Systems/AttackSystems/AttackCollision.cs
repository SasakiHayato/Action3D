using UnityEngine;
using AttackSetting;

/// <summary>
/// AttackSettingsとのやり取りをするクラス
/// </summary>

public class AttackCollision : MonoBehaviour, IAttack
{
    public enum Parent
    {
        Player,
        Enemy,
    }

    [SerializeField] int _groupID;
    public int GroupID { get => _groupID; }
    public Parent ParentID { get; private set; }

    GameObject _parent;
    GameObject _hitObj;
    IDamage _iDamage;

    bool _check;
    bool _isHit = false;

    /// <summary>
    /// Userの設定
    /// </summary>
    /// <param name="parent">対象者</param>
    public void SetUp(GameObject parent)
    {
        _parent = parent;
        GetComponent<Collider>().enabled = false;
        if (_parent.CompareTag("Player")) ParentID = Parent.Player;
        else ParentID = Parent.Enemy;
        Init();
    }
    
    /// <summary>
    /// AttackSettingsからのCallBack関数
    /// </summary>
    /// <returns>設定されたデータ</returns>
    public object[] CallBack()
    {
        object[] call = { _check, _iDamage, _hitObj };
        return call;
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public void Init()
    {
        _check = false;
        _iDamage = null;
        _hitObj = null;
        _isHit = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_parent == null || _isHit) return;

        if (_parent.CompareTag("Player") == other.CompareTag("Enemy")) SetParam(other);
        else if (_parent.CompareTag("Enemy") == other.CompareTag("Player")) SetParam(other);
    }

    void SetParam(Collider other)
    {
        _check = true;
        _isHit = true;
        _iDamage = other.GetComponent<IDamage>();
        _hitObj = other.gameObject;
    }
}
