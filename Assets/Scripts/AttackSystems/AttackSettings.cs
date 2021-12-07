using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AttackSetting
{
    public enum ActionType
    {
        Ground,
        Float,

        None,
    }

    public interface IAttack
    {
        void SetUp(GameObject parent);
        object[] CallBack();
        void Init();
    }

    public class AttackSettings : MonoBehaviour
    {
        [SerializeField] GameObject _parent;
        [SerializeField] float _requestCoolTime;
        /// <summary>
        /// Defoult�ƂȂ�Wepon
        /// </summary>
        [SerializeField] AttackCollision _targetWeapon;
        [SerializeField] List<AttackData> _attacks = new List<AttackData>();

        Animator _anim;
        AudioSource _audio;

        AttackCollision _saveDefWeapon;
        ActionType _saveActionType = ActionType.None;
        IAttack _iTarget;

        [System.Serializable]
        class AttackData
        {
            public string AnimName;
            public int GroupID;
            public float Power;
            public float NextAcceptTime;
            public AudioClip SE;
            public float SEVol;
            public ActionType Action;
            public EffectType[] Effects;
        }

        
        float _coolTime = 0;
        float _resetCombTime = 0;
        bool _isRequest = true;
        bool _attacking = false;

        int _findIndex = 0;

        int _saveGroupID = 0;
        AttackData _data;

        class EffectSetter
        {
            static GameObject _weapon;
            static GameObject _hitObj;
            static Animator _anim;

            public static EffectData Set(EffectData effect, EffectType[] types, object[] target)
            {
                _weapon = (GameObject)target[0];
                _anim = (Animator)target[1];
                _hitObj = (GameObject)target[2];

                foreach (EffectType type in types)
                {
                    switch (type)
                    {
                        case EffectType.HitStop:
                            effect += HitStop;
                            break;
                        case EffectType.ShakeCm:
                            effect += Effects.ShakeCm;
                            break;
                        case EffectType.HitParticle:
                            effect += HitParticle;
                            break;
                        case EffectType.KnonkBack:
                            effect += KnockBack;
                            break;
                        case EffectType.None:
                            effect += Effects.None;
                            break;
                    }
                }

                return effect;
            }

            static void HitParticle() => Effects.HitParticle(_weapon);
            static void HitStop() => Effects.HitStop(_anim);
            static void KnockBack() => Effects.KnockBack(_hitObj);

            public static void Init()
            {
                _weapon = null;
                _anim = null;
                _hitObj = null;
            }
        }

        delegate void EffectData();
        
        void Start()
        {
            _anim = _parent.GetComponent<Animator>();
            _audio = gameObject.AddComponent<AudioSource>();
            _audio.loop = false;

            _iTarget = _targetWeapon.GetComponent<IAttack>();
            _iTarget.SetUp(_parent);
            _targetWeapon.GetComponent<Collider>().enabled = false;
            _saveDefWeapon = _targetWeapon;
            _saveGroupID = _targetWeapon.GroupID;
        }

        void InitParam()
        {
            _findIndex = 0;
            _resetCombTime = 0;
            EffectSetter.Init();
        }

        void Update()
        {
            if ((bool)_iTarget.CallBack()[0])
            {
                IDamage iDamage = (IDamage)_iTarget.CallBack()[1];
                GameObject obj = (GameObject)_iTarget.CallBack()[2];
                IsAttack(iDamage, obj);
                _iTarget.Init();
            }

            if (!_isRequest) _coolTime += Time.deltaTime;
            _resetCombTime += Time.deltaTime;

            //CheckAttackCoolTime
            if (_coolTime > _requestCoolTime)
            {
                Debug.Log("CanAttack");
                _isRequest = true;
                _coolTime = 0;
            }
            
            // CheckComboTime
            if (_data != null)
                if (_resetCombTime > _data.NextAcceptTime)
                {
                    Debug.Log("ResetCombo");
                    _isRequest = true;
                    _coolTime = 0;
                    InitParam();
                }
        }

        /// <summary> �U���̐\�� </summary>
        /// <param name="type">�ǂ̃A�N�V�����Ȃ̂�</param>
        public void Request(ActionType type)
        {
            if (!_isRequest || _attacking)
            {
                Debug.Log("IsRunning");
                return;
            }

            Debug.Log("AcceptRequest");
            _isRequest = false;

            if (_saveActionType == ActionType.None || _saveActionType != type)
            {
                Debug.Log($"Init. NextAttackType is {type}");
                _saveActionType = type;
                InitParam();
            }
            else
                Debug.Log($"NextAttck. CurrentType is {_saveActionType}");

            for (int i = _findIndex; i < _attacks.Count; i++)
            {
                if(_attacks[i].Action == type && _attacks[i].GroupID == _saveGroupID)
                {
                    _findIndex = i + 1;
                    SetData(_attacks[i]);
                    return;
                }
            }

            InitParam();
            for (int i = _findIndex; i < _attacks.Count; i++)
            {
                if (_attacks[i].Action == type && _attacks[i].GroupID == _saveGroupID)
                {
                    Debug.Log($"EndedCombo. Reset,CurrentAction");
                    _findIndex = i + 1;
                    SetData(_attacks[i]);
                    return;
                }
            }

            Debug.LogError("Nothing MacthDate");
        }

        /// <summary> ����̕ύX </summary>
        /// <param name="set">NextWeapon</param>
        public void ChangeWeapon(AttackCollision set = null)
        {
            if (set == null)
            {
                Debug.Log($"Nothing,ChangeData. AddDefWeapon{_saveDefWeapon}");
                _targetWeapon = _saveDefWeapon;
                return;
            }

            InitParam();
            set.GetComponent<Collider>().enabled = false;
            _targetWeapon = set;
            _saveGroupID = set.GroupID;
        }

        /// <summary> AnimEvent�ł̌Ăяo�� </summary>
        void ColliderActive()
        {
            Collider collider = _targetWeapon.GetComponent<Collider>();
            if (collider.enabled)
            {
                collider.enabled = false;
                _attacking = false;
            }
            else 
                collider.enabled = true;
        }

        void SetData(AttackData data)
        {
            _resetCombTime = 0;
            _anim.Play(data.AnimName);
            _audio.volume = data.SEVol;
            if (data.SE != null) _audio.PlayOneShot(data.SE);
            else Debug.Log("Nothing SEData.");
            _attacking = true;
            _data = data;
        }

        void IsAttack(IDamage iDamage, GameObject obj)
        {
            object[] datas = { _targetWeapon.gameObject, _anim, obj };
            EffectData effect = null;
            EffectSetter.Set(effect, _data.Effects, datas).Invoke();
            iDamage.GetDamage(_data.Power);
        }
    }
}
