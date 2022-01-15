using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemysData
{
    [CreateAssetMenu (fileName = "EnemyGroupTip")]
    public class EnemyGroupTip : ScriptableObject
    {
        [SerializeField] List<GroupDatas> _datas;
        public List<GroupDatas> GetDatas => _datas;

        [System.Serializable]
        public class GroupDatas
        {
            public EnemyType[] Types;
        }
    }
}

