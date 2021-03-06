using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemyの基底データクラス
/// </summary>

namespace EnemysData
{
    public enum EnemyName
    {
        CubeEnemy = 0,
        WalkSkeleton = 1,
        RunSkeleton = 2,
        Boss = 3,
        BrackCubeEnemy = 4,

        None,
    }

    public enum EnemyType
    { 
        Boss,
        Mob,
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
        public string DisplayName;
        public int ID;
        public EnemyType EnemyType;
        public GameObject Prefab;
        public GameObject DummyPrefab;
        public int HP;
        public int Power;
        public float Speed;
        public int Exp;
    }
}