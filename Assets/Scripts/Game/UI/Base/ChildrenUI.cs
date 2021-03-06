using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 子となるUIの管理クラス
/// </summary>

public abstract class ChildrenUI : MonoBehaviour
{
    [SerializeField] string _path;
    public string Path => _path;

    public int ID { get; set; } = 0;
    public Image ParentPanel { get; set; }
    
    public abstract void SetUp();
    public abstract void CallBack(object[] datas = null);
}
