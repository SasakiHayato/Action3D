using System.Collections;
using System.Collections.Generic;
using EnemysData;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class FieldData
{
    public FieldData() { }

    public bool DebugBool { get; set; }
    public int DebugLevel { get; set; }
    
    List<FieldManager.SpawnData> _spawnData;
    EnemyMasterData _enemyMasterData;
    List<EnemyGroupData> _enemyGroupDatas;

    const float SetDist = 50;

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
        
        if (DebugLevel > 0) Level = DebugLevel;
        else Level = 0;
    }

    public void Update()
    {
        Level++;
        UpdateEnemy();
    }

    public void UpdateEnemy()
    {
        Transform player = GameObject.FindWithTag("Player").transform;
        foreach (EnemyGroupData groupData in _enemyGroupDatas)
        {
            FieldManager.SpawnData spawnData = _spawnData[groupData.SpawnID - 1];

            float dist = Vector3.Distance(spawnData.Point.position, player.position);
            if (dist < SetDist) continue;

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

        int random = Random.Range(0, spawnData.GroupTip.GetDatas.Count());
        foreach (EnemyData enemyData in _enemyMasterData.GetData)
        {
            foreach (EnemyType enemyType in spawnData.GroupTip.GetDatas[random].Types)
            {
                if (enemyData.Name == enemyType.ToString())
                {
                    float rate = spawnData.Range / 2;
                    float x = Random.Range(-rate, rate);
                    float y = Random.Range(-rate, rate);
                    float z = Random.Range(-rate, rate);
                    
                    GameObject obj = Object.Instantiate(enemyData.Prefab);
                    obj.transform.SetParent(spawnData.Point);
                    
                    int level = spawnData.Level + Level;
                    CharaBase charaBase = obj.GetComponent<CharaBase>();
                    charaBase.Character.ChangeLocalPos(new Vector3(x, y, z), obj);
                    charaBase.SetParam(enemyData.HP, enemyData.Power, enemyData.Speed, level);
                    IFieldEnemy iEnemy = obj.GetComponent<IFieldEnemy>();

                    if (iEnemy != null)
                    {
                        iEnemy.GroupID = spawnData.ID;
                        iEnemy.Target = obj;
                        iEnemy.EnemyData = enemyData;

                        groupData.FieldEnemies.Add(iEnemy);
                    }
                }
            }
        }
        
        groupData.IsSet = true;
    }

    void UpStatus(int id, List<IFieldEnemy> enemies)
    {
        int level = _spawnData[id - 1].Level + Level;
        
        foreach (IFieldEnemy enemy in enemies)
        {
            EnemyBase enemyBase = enemy.Target.GetComponent<EnemyBase>();
            enemyBase.SetParam(enemy.EnemyData.HP, enemy.EnemyData.Power, enemy.EnemyData.Speed, level);

            enemy.Target.GetComponentInChildren<EnemyCanvas>().UpdateState(enemyBase.MaxHP, enemyBase.Level);
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
