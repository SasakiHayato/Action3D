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
        bool CallBack();
        void Init();
    }

    public class AttackSettings : MonoBehaviour
    {
        [SerializeField] GameObject _parent;
        /// <summary>
        /// Defoult‚Æ‚È‚éWepon
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
            public float Power;
            public AudioClip SE;
            public float SEVol;
            public ActionType Action;
            public EffectType[] Effects;
        }

        int _combo = 0;
        int _findIndex = 0;

        // SetUp
        void Start()
        {
            _saveDefWeapon = _targetWeapon;
            _anim = _parent.GetComponent<Animator>();
            _audio = gameObject.AddComponent<AudioSource>();
            _audio.loop = false;
            _iTarget = _targetWeapon.GetComponent<IAttack>();
            _iTarget.SetUp(_parent);
        }

        void InitParam()
        {
            _combo = 0;
            _findIndex = 0;
        }

        void Update()
        {
            if (_iTarget.CallBack())
            {
                
            }
        }

        public void Request(ActionType type, int id = 0)
        {
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
                if(_attacks[i].Action == type)
                {
                    _findIndex = i + 1;
                    SetData(_attacks[i]);
                    return;
                }
            }

            Debug.Log($"EndCombo. Reset,CurrentAction");
            InitParam();
            for (int i = _findIndex; i < _attacks.Count; i++)
            {
                if (_attacks[i].Action == type)
                {
                    _findIndex = i + 1;
                    SetData(_attacks[i]);
                    return;
                }
            }
        }

        void SetData(AttackData data)
        {
            _anim.Play(data.AnimName);
            _audio.volume = data.SEVol;
            _audio.PlayOneShot(data.SE);

        }
    }
}
