using System.Collections.Generic;
using UnityEngine;

namespace EnemysData
{
    public enum EnemyType
    {
        CubeEnemy = 0,
        WalkSkeleton = 1,
        RunSkeleton = 2,
        Boss = 3,
        BrackCubeEnemy = 4,
    }

    [CreateAssetMenu(fileName = "EnemyMasterData")]
    public class EnemyMasterData : ScriptableObject
    {
        [SerializeField] List<EnemyData> _datas;
        public List<EnemyData> GetData => _datas;
    }

    [System.Serializable]
    public class EnemyData
    {
        public string Name;
        public int ID;
        public GameObject Prefab;
        public GameObject DummyPrefab;
        public int HP;
        public int Power;
        public float Speed;
        public int Exp;
    }
}