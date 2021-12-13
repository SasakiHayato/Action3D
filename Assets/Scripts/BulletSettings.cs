using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSettings : MonoBehaviour
{
    private static BulletSettings _instance = null;
    public static BulletSettings Instance = _instance;

    BulletPool<BulletData> _pool;

    private void Awake()
    {
        _pool = new BulletPool<BulletData>();
        _pool.Create();
        #region InstanceÇàÍÇ¬Ç…ï€èÿ
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }

        Destroy(gameObject);
        #endregion
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
}

public class BulletPool<T> where T : BulletSettings.BulletData
{
    public void Create()
    {

    }
}
