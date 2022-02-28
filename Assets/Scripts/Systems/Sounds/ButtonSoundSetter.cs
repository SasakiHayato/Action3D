using UnityEngine;
using UnityEngine.UI;

namespace Sounds
{
    /// <summary>
    /// Button‚É‘Î‚·‚éSound‚Ìİ’è
    /// </summary>

    public class ButtonSoundSetter : MonoBehaviour
    {
        [SerializeField] int _id;
        [SerializeField] SEDataBase.DataType _dataType;

        void Start()
        {
            Button button = transform.parent.GetComponent<Button>();
            button.onClick.AddListener(() => SoundMaster.PlayRequest(null, _id, _dataType));
        }
    }
}
