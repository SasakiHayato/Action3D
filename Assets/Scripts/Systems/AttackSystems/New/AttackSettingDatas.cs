using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewAttacks
{
    public enum AttackType
    {
        Weak,
        Strength,
        Counter,
        Float,
    }

    public enum Effect
    {
        HitParticle,
        HitStop,
        ShakeCm,
    }

    public partial class NewAttackSettings : MonoBehaviour
    {
        [System.Serializable]
        public class AttackDataList
        {
            public AttackType AttackType;
            public List<AttackData> AttackDatas;
        }

        [System.Serializable]
        public class AttackData
        {
            public string AnimName;
            public int ID;
            public int Power;
            public int SEID;
            public KnockBackData KnockBackData;
            public Effect[] Effects;
        }

        [System.Serializable]
        public class KnockBackData
        {
            public float UpPower;
            public float ForwardPower;
        }
    }
}


