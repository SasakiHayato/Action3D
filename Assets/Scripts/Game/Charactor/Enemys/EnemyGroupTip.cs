using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Field�ɐ�������Enemy�̐ݒ�
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

