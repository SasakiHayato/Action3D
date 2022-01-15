using System.Collections;
using System.Collections.Generic;
using EnemysData;
using UnityEngine;
using System.Linq;

public class FieldEnemyData 
{
    List<FieldManager.SpawnData> _spawnData;
    EnemyMasterData _enemyMasterData;

    Dictionary<bool, List<IFieldEnemy>> _fieldEnemiesDatas;
    
    public int Level { get; private set; } = 1;

    public FieldEnemyData(List<FieldManager.SpawnData> spawn, EnemyMasterData masterDatas)
    {
        _spawnData = spawn;
        _enemyMasterData = masterDatas;
        _fieldEnemiesDatas = new Dictionary<bool, List<IFieldEnemy>>();
        for (int i = 0; i < spawn.Count(); i++)
        {
            List<IFieldEnemy> enemies = new List<IFieldEnemy>();
            _fieldEnemiesDatas[false] = enemies;
        }
        
        Level = 1;
    }

    public void Update()
    {
        Level++;
        Check();
        SetEnemy();
    }

    public void SetEnemy()
    {
        Debug.Log(_fieldEnemiesDatas.Count());
        //Set();
    }

    void Set()
    {
        int groupCount = 0;
        
        foreach (FieldManager.SpawnData spawnData in _spawnData)
        {
            GameObject spownPoint = new GameObject("Point");
            spownPoint.transform.position = spawnData.Point.position;
            List<IFieldEnemy> fieldEnemies = new List<IFieldEnemy>();

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
                        get.transform.SetParent(spownPoint.transform);

                        int level = spawnData.Level + Level;
                        get.GetComponent<CharaBase>()
                            .SetParam(enemyData.HP, enemyData.Power, enemyData.Speed, level);
                        #endregion

                        IFieldEnemy iEnemy = get.GetComponent<IFieldEnemy>();
                        iEnemy.GroupID = spawnData.ID;
                        iEnemy.IEnemyDead = false;

                        fieldEnemies.Add(iEnemy);
                    }
                }
            }

            _fieldEnemiesDatas.Add(true, fieldEnemies);
            groupCount++;
        }
    }

    void Check()
    {
        
    }
}
