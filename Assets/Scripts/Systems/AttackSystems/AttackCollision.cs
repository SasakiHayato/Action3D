using UnityEngine;
using AttackSetting;
using System.Linq;
using System.Collections.Generic;
using DG.Tweening;

/// <summary>
/// AttackSettingsとのやり取りをするクラス
/// </summary>

public class AttackCollision : MonoBehaviour, IAttack, IAttackCollision
{
    // IAttackCollision
    public string ParentTagName { get; private set; }
    public GameObject Target { get; private set; }
    public Collider Collider { get; private set; }

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

    const float EffectDist = 2f;
    const float SetSize = 2;

    /// <summary>
    /// Userの設定
    /// </summary>
    /// <param name="parent">対象者</param>
    public void SetUp(GameObject parent)
    {
        _parent = parent;
        Collider collider = GetComponent<Collider>();
        if (_parent.CompareTag("Player")) ParentID = Parent.Player;
        else ParentID = Parent.Enemy;

        ParentTagName = parent.tag;
        Target = parent;
        Collider = collider;

        collider.enabled = false;
        GameManager.Instance.AddIAttackCollision(gameObject.GetComponent<IAttackCollision>());

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

    public void RipllesRequest()
    {
        var collisions = GameManager.Instance.AttackCollisions
            .Where(a =>
            {
                if (ParentTagName != a.ParentTagName) return true;
                else return false;
            })
            .Where(a =>
            {
                float dist = Vector3.Distance(a.Target.transform.position, Target.transform.position);
                if (dist < EffectDist && a.Collider.enabled) return true;
                else return false;
            });

        SetParticle(new List<IAttackCollision>(collisions));
    }

    void SetParticle(List<IAttackCollision> attacks)
    {
        attacks.ForEach(a =>
        {
            Vector3 midPos = (a.Target.transform.position + Target.transform.position) / 2;
            ParticleUser user = FieldManager.Instance.GetRipplesParticle.Respons();

            float rotateX = Random.Range(-180, 180);
            Vector3 rotate = new Vector3(rotateX, 90, 0);

            user.Use(midPos, Quaternion.Euler(rotate));

            Vector3 scale = user.transform.localScale;
            user.transform.DOScale(scale * SetSize, 0.4f)
                .SetEase(Ease.Linear)
                .OnComplete(() => user.Delete());
        });
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
