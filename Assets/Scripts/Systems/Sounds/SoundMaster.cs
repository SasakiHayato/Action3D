using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Soundの管理クラス
/// </summary>

namespace Sounds
{
    public class SoundMaster : MonoBehaviour
    {
        private static SoundMaster _instance = null;
        public static SoundMaster Instance => _instance;

        [SerializeField, Range(0, 100)] float _volumRate = 100;

        [SerializeField] SoundEffect _se;
        [SerializeField] List<SEDataBase> _dataBases;

        ObjectPool<SoundEffect> _pool;
        public float MasterVolumeRate { get => _volumRate; }

        void Awake()
        {
            _instance = this;
            _pool = new ObjectPool<SoundEffect>();
            _pool.SetUp(_se, transform);
        }

        /// <summary>
        /// SEのRequest
        /// </summary>
        /// <param name="user">使う人</param>
        /// <param name="id">SEDataのID</param>
        /// <param name="groupID">SEDataBaseのID</param>
        public static void Request(Transform user, int id, SEDataBase.DataType type)
        {
            SEDataBase dataBase = Instance._dataBases.First(d => d.GetDataType == type);
            foreach (SEData se in dataBase.GetData)
            {
                if (se.ID == id)
                {
                    SoundEffect sound = Instance._pool.Respons();
                    sound.Use(se, user);
                    return;
                }
            }
        }

        /// <summary>
        /// SEのRequest
        /// </summary>
        /// <param name="user">使う人</param>
        /// <param name="name">SEDataのName</param>
        /// <param name="groupID">SEDataBaseのID</param>
        public static void Request(Transform user, string name, SEDataBase.DataType type)
        {
            SEDataBase dataBase = Instance._dataBases.First(d => d.GetDataType == type);
            foreach (SEData se in dataBase.GetData)
            {
                if (se.Name == name)
                {
                    SoundEffect sound = Instance._pool.Respons();
                    sound.Use(se, user);
                    return;
                }
            }
        }
    }
}
