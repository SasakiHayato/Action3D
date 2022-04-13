using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SEのデータクラス
/// </summary>

namespace Sounds
{
    [CreateAssetMenu(fileName = "SEDatas")]
    public class SEDataBase : ScriptableObject
    {
        public enum DataType
        {
            BGM,
            UI,
            Field,
            Enemys,
            Player,
        }

        [SerializeField] MasterType _masterType;
        public MasterType Master => _masterType; 

        [SerializeField] DataType _type;
        public DataType GetDataType => _type;

        [SerializeField] List<SEData> _datas;
        public List<SEData> GetData => _datas;
    }

    [System.Serializable]
    public class SEData
    {
        public string Name;
        public int ID;
        public AudioClip Clip;
        [Range(0, 1)] public float Volume;
        [Range(0, 1)] public float SpatialBrend;
        public bool Loop;
    }
}
