using UnityEngine;

/// <summary>
/// SoundMasterÇ©ÇÁéÛÇØéÊÇ¡ÇΩDataÇÃä«óùÉNÉâÉX
/// </summary>

namespace Sounds
{
    public class SoundEffect : MonoBehaviour, IPool
    {
        AudioSource _source;
        Transform _parent;

        public SEDataBase.DataType Type { get; private set; }
        public SEData SEData { get; private set; }

        public bool IsUse { get; private set; }

        void Update()
        {
            if (!IsUse) return;
            if (!_source.isPlaying) Delete();
        }

        public void Use(SEData data, Transform user, SEDataBase.DataType type)
        {
            SEData = data;
            Type = type;

            transform.SetParent(user);
            _source = GetComponent<AudioSource>();
            _source.clip = data.Clip;
            _source.volume = data.Volume / SoundMaster.Instance.MasterVolumeRate;
            _source.spatialBlend = data.SpatialBrend;
            _source.loop = data.Loop;

            _source.Play();
            IsUse = true;
        }

        public void SetUp(Transform parent)
        {
            _source = gameObject.AddComponent<AudioSource>();
            _parent = parent;
            IsUse = false;
        }

        public void Delete()
        {
            SEData = null;
            IsUse = false;
            _source.clip = null;
            _source = null;
            transform.SetParent(_parent);
        }
    }
}
