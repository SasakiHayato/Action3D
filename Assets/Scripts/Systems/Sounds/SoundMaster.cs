using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Sound�̊Ǘ��N���X
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
        /// SE��Request
        /// </summary>
        /// <param name="user">�g���l</param>
        /// <param name="id">SEData��ID</param>
        /// <param name="groupID">SEDataBase��ID</param>
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
        /// SE��Request
        /// </summary>
        /// <param name="user">�g���l</param>
        /// <param name="name">SEData��Name</param>
        /// <param name="groupID">SEDataBase��ID</param>
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
