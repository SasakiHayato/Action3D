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

    public class AttackSettings : MonoBehaviour
    {
        #region メンバー変数
        [SerializeField] GameObject _parent;
        [SerializeField] float _requestCoolTime;
        /// <summary>
        /// DefoultとなるWepon
        /// </summary>
        [SerializeField] AttackCollision _targetWeapon;
        [SerializeField] List<AttackData> _attacks = new List<AttackData>();

        Animator _anim;
        AudioSource _audio;

        AttackCollision _saveDefWeapon;
        ActionType _saveActionType = ActionType.None;
        IAttack _iTarget;
        #endregion

        [System.Serializable]
        class AttackData
        {
            public string AnimName;
            public int GroupID;
            public int Power;
            public float KnockBackPower;
            public float NextAcceptTime;
            public AudioClip SE;
            public float SEVol;
            public ActionType Action;
            public EffectType[] Effects;
        }

        #region メンバー変数
        float _coolTime = 0;
        float _resetCombTime = 0;
        bool _isRequest = true;
        bool _attacking = false;

        int _findIndex = 0;

        int _saveGroupID = 0;
        AttackData _data;
        #endregion

        #region Class EffectSetter
        class EffectSetter
        {
            static AttackCollision _weapon;
            static GameObject _hitObj;
            static GameObject _parent;
            static Animator _anim;
            static float _knockBackPower;

            public static EffectData Set(EffectData effect, EffectType[] types, object[] target)
            {
                _weapon = (AttackCollision)target[0];
                _anim = (Animator)target[1];
                _hitObj = (GameObject)target[2];
                _parent = (GameObject)target[3];
                _knockBackPower = (float)target[4];

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
            static void KnockBack() => Effects.KnockBack(_hitObj, _weapon.ParentID, _parent.transform, _knockBackPower);

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
                _isRequest = true;
                _coolTime = 0;
            }
            
            // CheckComboTime
            if (_data != null)
                if (_resetCombTime > _data.NextAcceptTime)
                {
                    _isRequest = true;
                    _coolTime = 0;
                    InitParam();
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
            Debug.Log("IsRequest");
            _isRequest = false;

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

        public void Cansel() => _attacking = false;

        void SetData(AttackData data)
        {
            EndCurrentAnim = false;
            _resetCombTime = 0;
            _anim.Play(data.AnimName);
            _audio.volume = data.SEVol;
            if (data.SE != null) _audio.PlayOneShot(data.SE);
            else Debug.Log("Nothing SEData.");
            _attacking = true;
            _data = data;

            StartCoroutine(WaitAnim());
        }

        IEnumerator WaitAnim()
        {
            yield return null;
            yield return new WaitAnim(_anim);
            EndCurrentAnim = true;
        }

        void IsAttack(IDamage iDamage, GameObject obj)
        {
            object[] datas = { _targetWeapon, _anim, obj, _parent, _data.KnockBackPower };
            EffectData effect = null;
            EffectSetter.Set(effect, _data.Effects, datas).Invoke();
            iDamage.GetDamage(_data.Power);
        }

        /// <summary> AnimEventでの呼び出し </summary>
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

    }
}
