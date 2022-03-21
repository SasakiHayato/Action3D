using System.Collections.Generic;
using UnityEngine;
using System;
using Sounds;

namespace NewAttacks
{
    public partial class AttackSettings : MonoBehaviour
    {
        [SerializeField] GameObject _target;
        [SerializeField] AttackCollider _targetWeapon;
        [SerializeField] List<AttackDataList> _dataLists = new List<AttackDataList>();
        
        int _setID = 0;
        TrailRenderer _trail;
        AttackEffectSetter _effectSetter;
        CharaBase _charaBase;
        AttackData _data;
        EnemyAttackSystem _enemyAttackSystem;

        Animator _targetAnim;
        Collider _weaponCllider;
        Action _effectAction;

        string _saveAnimName;

        AttackType _saveType = default;
        public AttackType SetAttackType { set { _saveType = value; } }
        public AttackType ReadAttackType => _saveType;

        public bool EndCurrentAnim { get; private set; } = false;
        public bool IsNextRequest { get; private set; } = false;
        public bool IsSetNextRequest { get; private set; } = false;
        public bool WeaponColliderEnable => _weaponCllider.enabled;

        const float Duration = 0.1f;

        void Awake()
        {
            if (_target.tag == "Enemy")
                _enemyAttackSystem = gameObject.AddComponent<EnemyAttackSystem>();
        }

        void Start()
        {
            _charaBase = _target.GetComponent<CharaBase>();
            _targetAnim = _target.GetComponent<Animator>();

            _weaponCllider = _targetWeapon.GetComponent<Collider>();
            _weaponCllider.enabled = false;

            _trail = _targetWeapon.GetComponentInChildren<TrailRenderer>();
            if (_trail != null) _trail.enabled = false;

            _targetWeapon.SetUp(gameObject, this);

            _effectSetter = new AttackEffectSetter();
            _effectSetter.SetUpUserData(gameObject, _targetWeapon);
        }

        public void Request(AttackType type, int id = -1)
        {
            foreach (AttackDataList dataList in _dataLists)
            {
                if (dataList.AttackType == type)
                {
                    if (_saveType != dataList.AttackType)
                    {
                        _saveType = dataList.AttackType;
                        _setID = 0;
                    }

                    if (dataList.AttackDatas.Count <= _setID) _setID = 0;

                    if (id > -1) _setID = id;

                    Set(dataList.AttackDatas[_setID]);
                    _setID++;

                    return;
                }
            }
        }

        public void SetNextRequest()
        {
            IsSetNextRequest = true;
        }

        public void Cancel()
        {
            EndAnim();
            _weaponCllider.enabled = false;

            if (_trail != null) _trail.enabled = false;
        }

        public void IsHitCallBack(Collider collider)
        {
            CharaBase charaBase =  collider.GetComponent<CharaBase>();
            charaBase.KnonckUpPower = _data.KnockBackData.UpPower;
            charaBase.KnonckForwardPower = _data.KnockBackData.ForwardPower;
            charaBase.KnockDir = _target.transform.forward;

            collider.GetComponent<IDamage>()
                .GetDamage(_data.Power * _charaBase.Power, global::AttackType.Sword);
            _effectAction?.Invoke();
        }

        void Set(AttackData data)
        {
            _data = data;
            _weaponCllider.enabled = false;

            EndCurrentAnim = false;
            IsNextRequest = false;
            IsSetNextRequest = false;

            _effectAction = null;

            string animName = data.AnimName;
            
            if (_saveAnimName == "" || animName != _saveAnimName)
            {
                _saveAnimName = animName;
                _targetAnim.CrossFade(animName, Duration);
            }
            else
            {
                _targetAnim.Play(animName, 0, 0);
            }

            SoundMaster.PlayRequest(_targetWeapon.transform, data.SEID, SEDataBase.DataType.Player);
            _effectSetter.Set(ref _effectAction, data.Effects);
        }

        /// <summary> AnimEventÇ≈åƒÇ—èoÇµ </summary>
        void EndAnim()
        {
            EndCurrentAnim = true;
            _setID = 0;
            IsSetNextRequest = false;
            _saveType = AttackType.Weak;
        }

        /// <summary> AnimEventÇ≈åƒÇ—èoÇµ </summary>
        void ColliderActive()
        {
            if (!_weaponCllider.enabled)
            {
                _weaponCllider.enabled = true;

                if (_trail != null) _trail.enabled = true;
            }
            else
            {
                _weaponCllider.enabled = false;
                if (_trail != null) _trail.enabled = false;
                IsNextRequest = true;
            }
        }
    }
}
