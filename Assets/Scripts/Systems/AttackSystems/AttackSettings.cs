using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AttackSetting
{
    public enum ActionType
    {
        WeakGround,
        StrengthGround,
        Counter,
        Float,

        None,
    }

    public interface IAttack
    {
        void SetUp(GameObject parent);
        object[] CallBack();
        void Init();
    }

    /// <summary>
    /// 攻撃を管理するクラス
    /// </summary>

    public class AttackSettings : MonoBehaviour
    {
        public enum KnockType
        {
            Forward,
            Up,
            Down,

            None,
        }

        #region メンバー変数
        [SerializeField] GameObject _parent;
        [SerializeField] bool _requestEnemySystem;  
        [SerializeField] float _requestCoolTime;

        /// <summary>
        /// DefoultとなるWepon
        /// </summary>
        [SerializeField] AttackCollision _targetWeapon;
        [SerializeField] List<AttackData> _attacks = new List<AttackData>();

        Animator _anim;
        AudioSource _audio;
        CharaBase _charaBase;
        EnemyAttackSystem _enemySystem;
        
        AttackCollision _saveDefWeapon;
        ActionType _saveActionType = ActionType.None;
        IAttack _iTarget;
        #endregion

        [System.Serializable]
        public class AttackData
        {
            public string AnimName;
            public int GroupID;
            public int Power;
            public bool IsEndCombo;
            public KnockBackData KnockBackData;
            public float NextAcceptTime;
            public AudioClip SE;
            public float SEVol;
            public ActionType Action;
            public EffectType[] Effects;
        }

        [System.Serializable]
        public class KnockBackData
        {
            public float Power = 1;
            public KnockType Type = KnockType.Forward;
        }

        #region メンバー変数
        float _coolTime = 0;
        float _resetCombTime = 0;
        bool _isRequest = true;
        bool _attacking = false;
        bool _nextRequest = false;
        
        int _findIndex = 0;

        int _saveGroupID = 0;
        AttackData _data;
        #endregion

        /// <summary>
        /// 攻撃が当たった際のEffectの設定クラス
        /// </summary>
        #region Class EffectSetter
        class EffectSetter
        {
            static AttackCollision _weapon;
            static GameObject _hitObj;
            static GameObject _parent;
            static Animator _anim;
            static KnockBackData _backData;

            public static EffectData Set(EffectData effect, EffectType[] types, object[] target)
            {
                _weapon = (AttackCollision)target[0];
                _anim = (Animator)target[1];
                _hitObj = (GameObject)target[2];
                _parent = (GameObject)target[3];
                _backData = (KnockBackData)target[4];

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

            static void HitParticle() => Effects.HitParticle(_weapon.gameObject);
            static void HitStop() => Effects.HitStop(_anim);
            static void KnockBack() => Effects.KnockBack(_hitObj, _weapon.ParentID, _parent.transform, _backData);

            public static void Init()
            {
                _weapon = null;
                _anim = null;
                _hitObj = null;
            }
        }
        #endregion

        delegate void EffectData();
        
        public ActionType SetAction { private get; set; }
        public ActionType ReadAction { get => SetAction; }
        public bool EndCurrentAnim { get; private set; } = true;
        
        public bool IsNextRequest { get; private set; } = false;
        public bool IsCounter { get; private set; } = false;

        void Awake()
        {
            if (_requestEnemySystem)
            {
                _enemySystem = _parent.AddComponent<EnemyAttackSystem>();
            }
        }

        void Start()
        {
            _anim = _parent.GetComponent<Animator>();
            _audio = gameObject.AddComponent<AudioSource>();
            _charaBase = _parent.GetComponent<CharaBase>();
            
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
                _isRequest = true;
                _coolTime = 0;
            }
            
            if (_nextRequest)
            {
                // CheckComboTime
                if (_data != null)
                    if (_resetCombTime > _data.NextAcceptTime)
                    {
                        _isRequest = true;
                        _nextRequest = false;
                        _coolTime = 0;
                        InitParam();
                    }
            }
        }

        /// <summary> 攻撃の申請 </summary>
        /// <param name="type">どのアクションなのか</param>
        public void Request(ActionType type)
        {
            if (!_isRequest || _attacking)
            {
                Debug.Log("ReturnRequest");
                Debug.Log($"IsRequest{_isRequest} : Attaking {_attacking}");
                return;
            }

            _isRequest = false;
            IsNextRequest = false;

            if (_saveActionType == ActionType.None || _saveActionType != type)
            {
                _saveActionType = type;
                InitParam();
            }

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
                    _findIndex = i + 1;
                    SetData(_attacks[i]);
                    
                    return;
                }
            }
        }

        public void RequestAt(ActionType type, int id)
        {
            if (!_isRequest || _attacking)
            {
                Debug.Log("ReturnRequest");
                Debug.Log($"IsRequest{_isRequest} : Attaking {_attacking}");
                return;
            }

            _isRequest = false;
            IsNextRequest = false;

            if (_saveActionType == ActionType.None || _saveActionType != type)
            {
                _saveActionType = type;
                InitParam();
            }

            List<AttackData> list = new List<AttackData>();

            foreach (var item in _attacks)
            {
                if (item.Action == type) list.Add(item);
            }

            SetData(list[id]);
        }

        // 先行入力の受付
        public void NextRequest() { if (_nextRequest) IsNextRequest = true; }
        
        // 連続攻撃の最終段判定
        public bool GetEndCombo()
        {
            if (_data == null) return false;
            else return _data.IsEndCombo;
        }

        /// <summary> 武器の変更 </summary>
        /// <param name="set">NextWeapon</param>
        public void ChangeWeapon(AttackCollision set = null)
        {
            if (set == null)
            {
                _targetWeapon = _saveDefWeapon;
                return;
            }

            InitParam();
            set.GetComponent<Collider>().enabled = false;
            _targetWeapon = set;
            _saveGroupID = set.GroupID;
        }
         
        // 攻撃のキャンセル
        public void Cancel()
        {
            _attacking = false;
            _targetWeapon.GetComponent<Collider>().enabled = false;
            IsCounter = false;

            EndCurrentAnim = true;
        }

        void SetData(AttackData data)
        {
            if (data.Action == ActionType.Counter) IsCounter = true;
            else IsCounter = false;
            
            _nextRequest = false;
            EndCurrentAnim = false;
            _resetCombTime = 0;
            _anim.CrossFade(data.AnimName, 0.1f);
            
            _audio.volume = data.SEVol;
            if (data.SE != null) _audio.PlayOneShot(data.SE);
            
            _attacking = true;
            _data = data;

            //StartCoroutine(WaitAnim());
        }

        IEnumerator WaitAnim()
        {
            yield return null;
            yield return new WaitAnim(_anim);
            EndCurrentAnim = true;
            _nextRequest = false;
           
            if (!IsNextRequest) InitParam();
            if (IsCounter) IsCounter = false;
        }

        void IsAttack(IDamage iDamage, GameObject obj)
        {
            if (iDamage == null) return;

            object[] datas = { _targetWeapon, _anim, obj, _parent, _data.KnockBackData };
            EffectData effect = null;
            EffectSetter.Set(effect, _data.Effects, datas).Invoke();
            iDamage.GetDamage(_data.Power * _charaBase.Power, AttackType.Sword);
        }

        /// <summary> AnimEventでの呼び出し </summary>
        void ColliderActive()
        {
            Collider collider = _targetWeapon.GetComponent<Collider>();
            if (collider.enabled)
            {
                collider.enabled = false;
                _attacking = false;
                _nextRequest = true;
            }
            else
            {
                collider.enabled = true;
                _targetWeapon.RipllesRequest();
            }       
        }

        /// <summary> AnimEventでの呼び出し </summary>
        void EndAnim()
        {
            EndCurrentAnim = true;
            _attacking = false;
            _targetWeapon.GetComponent<Collider>().enabled = false;
            IsCounter = false;
        }
    }
}
