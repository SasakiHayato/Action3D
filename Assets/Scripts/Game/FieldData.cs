using System.Collections;
using System.Collections.Generic;
using EnemysData;
using UnityEngine;
using System.Linq;

public class FieldData 
{
    List<FieldManager.SpawnData> _spawnData;
    EnemyMasterData _enemyMasterData;
    List<EnemyGroupData> _enemyGroupDatas;

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
        _enemyGroupDatas = new List<EnemyGroupData>();

        for (int i = 0; i < spawn.Count(); i++)
        {
            _enemyGroupDatas.Add(new EnemyGroupData(spawn[i].ID, false, new List<IFieldEnemy>()));
        }
        
        Level = 1;
    }

    public void Update()
    {
        Level++;
        UpdateEnemy();
    }

    public void UpdateEnemy()
    {
        foreach (EnemyGroupData groupData in _enemyGroupDatas)
        {
            if (!groupData.IsSet)
            {
                SetEnemy(groupData);
            }
            else
            {
                int id = groupData.SpawnID;
                UpStatus(id, groupData.FieldEnemies);
            }
        }
    }

    void SetEnemy(EnemyGroupData groupData)
    {
        FieldManager.SpawnData spawnData = _spawnData[groupData.SpawnID - 1];
        
        if (groupData.Point == null) groupData.Point = new GameObject("Point");
        groupData.Point.transform.position = spawnData.Point.position;
        List<IFieldEnemy> enemies = new List<IFieldEnemy>();

        int random = Random.Range(0, spawnData.GroupTip.GetDatas.Count());
        foreach (EnemyData enemyData in _enemyMasterData.GetData)
        {
            foreach (EnemyType enemyType in spawnData.GroupTip.GetDatas[random].Types)
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
                    iEnemy.Target = get;
                    iEnemy.EnemyData = enemyData;

                    enemies.Add(iEnemy);
                }
            }
        }

        groupData.FieldEnemies = enemies;
        groupData.IsSet = true;
    }

    void UpStatus(int id, List<IFieldEnemy> enemies)
    {
        int level = _spawnData[id - 1].Level + Level;
        
        foreach (IFieldEnemy enemy in enemies)
        {
            enemy.Target.GetComponent<EnemyBase>()
                .SetParam(enemy.EnemyData.HP, enemy.EnemyData.Power, enemy.EnemyData.Speed, level);
        }
    }

    public void Delete(int id, IFieldEnemy enemy)
    {
        foreach (var data in _enemyGroupDatas)
        {
            if (data.SpawnID == id)
            {
                data.FieldEnemies.Remove(enemy);
                if (data.FieldEnemies.Count() == 0) data.IsSet = false;
            }
        }
    }
}
