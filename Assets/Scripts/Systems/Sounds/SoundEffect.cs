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

        MasterType _master;

        public SEDataBase.DataType Type { get; private set; }
        public SEData SEData { get; private set; }

        public bool IsUse { get; private set; }

        void Update()
        {
            if (!IsUse) return;

            if (!_source.isPlaying) Delete();
            else SetVolume();
        }

        public void Use(SEData data, Transform user, SEDataBase.DataType type, MasterType master)
        {
            SEData = data;
            Type = type;
            _master = master;

            transform.SetParent(user);
            _source = GetComponent<AudioSource>();
            _source.clip = data.Clip;
            SetVolume();
            _source.spatialBlend = data.SpatialBrend;
            _source.loop = data.Loop;

            _source.Play();
            IsUse = true;
        }

        void SetVolume()
        {
            float masterVol = SEData.Volume * SoundMaster.Instance.MasterVolumeRate;
            float setVol = 0;

            if (_master == MasterType.BGM) setVol = masterVol * SoundMaster.Instance.BGMVolumeRate;
            else setVol = masterVol * SoundMaster.Instance.SEVolumeRate;

            _source.volume = setVol;
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
