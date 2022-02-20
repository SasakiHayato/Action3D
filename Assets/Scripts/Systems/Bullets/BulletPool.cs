using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BulletのPool管理クラス
/// </summary>
/// <typeparam name="T">Bulletのデータ</typeparam>

public class BulletPool<T> where T : BulletSettings.BulletData
{
    GameObject _pool = null;
    List<GameObject> _bulletsPool = new List<GameObject>();
    List<T> _datas;

    // Poolさせるデータを入れる
    public void SetUp(List<T> datas)
    {
        _datas = datas;
    }

    /// <summary>
    /// BulletSettings.BulletDataを元にPoolを作成
    /// </summary>
    /// <param name="data">元となるデータ</param>
    /// <param name="count">何個作るかの指定</param>
    public void Create(T data, int count = 50)
    {
        if (_pool == null)
        {
            GameObject obj = new GameObject("BulletPool");
            _pool = obj;
        }

        for (int i = 0; i < count; i++)
        {
            GameObject obj = Object.Instantiate(data.Prefab);
            obj.GetComponent<Collider>().isTrigger = true;
            obj.name = $"ID:{data.ID}. Name.{data.Name}";


            obj.AddComponent<Bullet>().SetUp(data, Delete);

            _bulletsPool.Add(obj);
            obj.transform.SetParent(_pool.transform);
            obj.SetActive(false);
        }
    }

    /// <summary>
    /// 使用可能Objectを調べて可能ならそのObjectを返す
    /// </summary>
    /// <param name="id">使いたいBulletDataのID</param>
    /// <returns></returns>
    public GameObject Use(int id)
    {
        foreach (GameObject obj in _bulletsPool)
        {
            if (!obj.activeSelf)
            {
                obj.SetActive(true);
                if (obj.GetComponent<Bullet>().GetID == id) return obj;
                else obj.SetActive(false);
            }
        }

        Create(_datas[id], 15);
        return Use(id);
    }

    /// <summary>
    /// 使い終わったデータをPoolに戻す
    /// </summary>
    /// <param name="obj">戻す対象となるObject</param>
    public void Delete(GameObject obj)
    {
        obj.GetComponent<Bullet>().Init();
        obj.SetActive(false);
    }
}
