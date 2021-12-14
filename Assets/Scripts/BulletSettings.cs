using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSettings : MonoBehaviour
{
    private static BulletSettings _instance = null;
    public static BulletSettings Instance = _instance;

    BulletPool<BulletData> _pool = new BulletPool<BulletData>();

    private void Awake()
    {
        #region InstanceÇàÍÇ¬Ç…ï€èÿ
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        #endregion

        //_pool = new BulletPool<BulletData>();

        for (int i = 0; i < _datas.Count; i++)
        {
            _pool.Create(_datas[i]);
        }
    }

    [SerializeField] List<BulletData> _datas = new List<BulletData>();

    [System.Serializable]
    public class BulletData
    {
        public string Name;
        public int ID;
        public GameObject Prefab;
        public float Power;
    }

    public static GameObject UseBullet(int id)
    {
        GameObject obj = Instance._pool.Use(id);
        
        return obj;
    }
}

public class BulletPool<T> where T : BulletSettings.BulletData
{
    T BulletData;

    GameObject _pool = null;
    List<GameObject> _bulletsPool = new List<GameObject>();

    int _count = 1;

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
            obj.name = $"ID:{data.ID}. Name.{data.Name}";
            obj.AddComponent<Bullet>().SetUp(data);

            _bulletsPool.Add(obj);
            obj.transform.SetParent(_pool.transform);
            obj.SetActive(false);

            _count++;
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

        Create(BulletData, 15);
        return Use(id);
    }
}
