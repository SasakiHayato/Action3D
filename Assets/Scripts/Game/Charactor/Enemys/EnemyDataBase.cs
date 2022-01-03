using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDataBase : ScriptableObject
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
}
