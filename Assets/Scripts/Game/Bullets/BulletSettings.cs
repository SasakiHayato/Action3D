using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSettings : MonoBehaviour
{
    // どこからでも呼び出せるように
    private static BulletSettings _instance = null;
    public static BulletSettings Instance => _instance;

    BulletPool<BulletData> _pool = new BulletPool<BulletData>();

    private void Awake()
    {
        _instance = this;

        Instance._pool.SetUp(_datas);

        for (int i = 0; i < _datas.Count; i++)
            Instance._pool.Create(_datas[i], _createOneData);
    }

    [SerializeField] int _createOneData = 0;
    [SerializeField] List<BulletData> _datas = new List<BulletData>();

    // バレットのデータ
    [System.Serializable]
    public class BulletData
    {
        public string Name;
        public int ID;
        public GameObject Prefab;
        public int Power;
    }

    /// <summary>
    /// 使いたい側で呼び出す
    /// </summary>
    /// <param name="id">BulletDataのIDを指定</param>
    /// <returns></returns>
    public static GameObject UseRequest(int id)
    {
        GameObject obj = Instance._pool.Use(id);
        return obj;
    }
}
