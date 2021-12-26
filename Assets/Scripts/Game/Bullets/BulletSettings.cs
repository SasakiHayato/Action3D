using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSettings : MonoBehaviour
{
    // �ǂ�����ł��Ăяo����悤��
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

    // �o���b�g�̃f�[�^
    [System.Serializable]
    public class BulletData
    {
        public string Name;
        public int ID;
        public GameObject Prefab;
        public int Power;
    }

    /// <summary>
    /// �g���������ŌĂяo��
    /// </summary>
    /// <param name="id">BulletData��ID���w��</param>
    /// <returns></returns>
    public static GameObject UseRequest(int id)
    {
        GameObject obj = Instance._pool.Use(id);
        return obj;
    }
}
