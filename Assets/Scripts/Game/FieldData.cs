using System.Collections;
using System.Collections.Generic;
using EnemysData;
using UnityEngine;
using System.Linq;

public class FieldData 
{
    List<FieldManager.SpawnData> _spawnData;
    EnemyMasterData _enemyMasterData;
    List<EnemyGroupData> _datas;

    class EnemyGroupData
    {
        public EnemyGroupData(int id, bool set, List<IFieldEnemy> enemies)
        {
            SpawnID = id;
            IsSet = set;
            FieldEnemies = enemies;
        }

        public int SpawnID;
        public bool IsSet;
        public List<IFieldEnemy> FieldEnemies;
        public GameObject Point = null;
    }

    public int Level { get; private set; } = 1;

    public FieldData(List<FieldManager.SpawnData> spawn, EnemyMasterData masterDatas)
    {
        _spawnData = spawn;
        _enemyMasterData = masterDatas;
        _datas = new List<EnemyGroupData>();

        for (int i = 0; i < spawn.Count(); i++)
        {
            _datas.Add(new EnemyGroupData(spawn[i].ID, false, new List<IFieldEnemy>()));
        }
        
        Level = 1;
    }

    public void Update()
    {
        Level++;
        Check();
        UpdateEnemy();
    }

    public void UpdateEnemy()
    {
        foreach (EnemyGroupData groupData in _datas)
        {
            if (!groupData.IsSet)
            {
                SetEnemy(groupData);
            }
            else
            {

            }
        }
    }

    void SetEnemy(EnemyGroupData groupData)
    {
        FieldManager.SpawnData spawnData = _spawnData[groupData.SpawnID - 1];
        
        if (groupData.Point == null) groupData.Point = new GameObject("Point");
        groupData.Point.transform.position = spawnData.Point.position;
        List<IFieldEnemy> enemies = new List<IFieldEnemy>();

        foreach (EnemyData enemyData in _enemyMasterData.GetData)
        {
            foreach (EnemyType enemyType in spawnData.Enemys)
            {
                if (enemyData.Name == enemyType.ToString())
                {
                    #region SetEnemyData
                    float rate = spawnData.Range / 2;
                    float x = Random.Range(-rate, rate);
                    float y = Random.Range(-rate, rate);
                    float z = Random.Range(-rate, rate);

                    GameObject get = Object.Instantiate(enemyData.Prefab);
                    Vector3 dataPos = spawnData.Point.position;
                    Vector3 setVec = new Vector3(dataPos.x + x, dataPos.y + y, dataPos.z + z);
                    get.transform.position = setVec;
                    get.transform.SetParent(groupData.Point.transform);

                    int level = spawnData.Level + Level;
                    get.GetComponent<CharaBase>()
                        .SetParam(enemyData.HP, enemyData.Power, enemyData.Speed, level);
                    #endregion

                    IFieldEnemy iEnemy = get.GetComponent<IFieldEnemy>();
                    iEnemy.GroupID = spawnData.ID;
                    iEnemy.IEnemyDead = false;

                    enemies.Add(iEnemy);
                }
            }
        }
        groupData.FieldEnemies = enemies;
        groupData.IsSet = true;
    }

    void Check()
    {
        
    }
}
