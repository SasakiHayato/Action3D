using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Bullet��Pool�Ǘ��N���X
/// </summary>
/// <typeparam name="T">Bullet�̃f�[�^</typeparam>

public class BulletPool<T> where T : BulletSettings.BulletData
{
    GameObject _pool = null;
    List<GameObject> _bulletsPool = new List<GameObject>();
    List<T> _datas;

    // Pool������f�[�^������
    public void SetUp(List<T> datas)
    {
        _datas = datas;
    }

    /// <summary>
    /// BulletSettings.BulletData������Pool���쐬
    /// </summary>
    /// <param name="data">���ƂȂ�f�[�^</param>
    /// <param name="count">����邩�̎w��</param>
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
    /// �g�p�\Object�𒲂ׂĉ\�Ȃ炻��Object��Ԃ�
    /// </summary>
    /// <param name="id">�g������BulletData��ID</param>
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
    /// �g���I������f�[�^��Pool�ɖ߂�
    /// </summary>
    /// <param name="obj">�߂��ΏۂƂȂ�Object</param>
    public void Delete(GameObject obj)
    {
        obj.GetComponent<Bullet>().Init();
        obj.SetActive(false);
    }
}
