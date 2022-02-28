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

        List<SoundEffect> _soundList;

        void Awake()
        {
            _instance = this;
            _pool = new ObjectPool<SoundEffect>();
            _soundList = new List<SoundEffect>();
            _pool.SetUp(_se, transform);
        }

        /// <summary>
        /// SEのRequest
        /// </summary>
        /// <param name="user">使う人</param>
        /// <param name="id">SEDataのID</param>
        /// <param name="type">DataType</param>
        public static void PlayRequest(Transform user, int id, SEDataBase.DataType type)
        {
            SEDataBase dataBase = Instance._dataBases.First(d => d.GetDataType == type);
            foreach (SEData se in dataBase.GetData)
            {
                if (se.ID == id)
                {
                    SoundEffect sound = Instance._pool.Respons();
                    Instance._soundList.Add(sound);
                    sound.Use(se, user, type);
                    return;
                }
            }
        }

        /// <summary>
        /// SEのRequest
        /// </summary>
        /// <param name="user">使う人</param>
        /// <param name="name">SEDataのName</param>
        /// <param name="type">DataType</param>
        public static void PlayRequest(Transform user, string name, SEDataBase.DataType type)
        {
            SEDataBase dataBase = Instance._dataBases.First(d => d.GetDataType == type);
            foreach (SEData se in dataBase.GetData)
            {
                if (se.Name == name)
                {
                    SoundEffect sound = Instance._pool.Respons();
                    Instance._soundList.Add(sound);
                    sound.Use(se, user, type);
                    return;
                }
            }
        }

        /// <summary>
        /// 指定したSEの停止
        /// </summary>
        /// <param name="id">SEDataのID</param>
        /// <param name="type">DataTyep</param>
        public static void StopRequest(int id, SEDataBase.DataType type)
        {
            foreach (SoundEffect sound in Instance._soundList)
            {
                if (sound.IsUse)
                {
                    if (sound.SEData.ID == id && sound.Type == type)
                    {
                        sound.Delete();
                        Instance._soundList.Remove(sound);
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// 指定したSEの停止
        /// </summary>
        /// <param name="name">SEDataの名前</param>
        /// <param name="type">DataType</param>
        public static void StopRequest(string name, SEDataBase.DataType type)
        {
            foreach (SoundEffect sound in Instance._soundList)
            {
                if (sound.IsUse)
                {
                    if (sound.SEData.Name == name && sound.Type == type)
                    {
                        sound.Delete();
                        Instance._soundList.Remove(sound);
                        return;
                    }
                }
            }
        }
    }
}
