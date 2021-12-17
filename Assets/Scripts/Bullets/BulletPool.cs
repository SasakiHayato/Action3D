using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool<T> where T : BulletSettings.BulletData
{
    GameObject _pool = null;
    List<GameObject> _bulletsPool = new List<GameObject>();
    List<BulletSettings.BulletData> _datas;

    public void SetUp(List<BulletSettings.BulletData> datas)
    {
        _datas = datas;
    }

    public void Create(BulletSettings.BulletData data, int count = 50)
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

    public void Delete(GameObject obj)
    {
        obj.GetComponent<Bullet>().Init();
        obj.SetActive(false);
    }
}
