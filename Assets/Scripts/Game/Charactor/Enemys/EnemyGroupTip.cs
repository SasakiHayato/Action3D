using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Fieldに生成するEnemyの設定
/// </summary>

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
            public EnemyName[] Types;
        }
    }
}

