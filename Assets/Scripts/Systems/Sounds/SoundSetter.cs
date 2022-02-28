using UnityEngine;
using UnityEngine.UI;

namespace Sounds
{
    /// <summary>
    /// Buttonに対するSoundの設定
    /// </summary>

    public class SoundSetter : MonoBehaviour
    {
        [SerializeField] int _id;
        [SerializeField] SEDataBase.DataType _dataType;

        void Start()
        {
            Button button = transform.parent.GetComponent<Button>();
            button.onClick.AddListener(() => SoundMaster.Request(null, _id, _dataType));
        }
    }
}
