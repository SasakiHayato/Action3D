using UnityEngine;
using AttackSetting;

/// <summary>
/// AttackSettings�Ƃ̂���������N���X
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
    /// User�̐ݒ�
    /// </summary>
    /// <param name="parent">�Ώێ�</param>
    public void SetUp(GameObject parent)
    {
        _parent = parent;
        GetComponent<Collider>().enabled = false;
        if (_parent.CompareTag("Player")) ParentID = Parent.Player;
        else ParentID = Parent.Enemy;
        Init();
    }
    
    /// <summary>
    /// AttackSettings�����CallBack�֐�
    /// </summary>
    /// <returns>�ݒ肳�ꂽ�f�[�^</returns>
    public object[] CallBack()
    {
        object[] call = { _check, _iDamage, _hitObj };
        return call;
    }

    /// <summary>
    /// ������
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
